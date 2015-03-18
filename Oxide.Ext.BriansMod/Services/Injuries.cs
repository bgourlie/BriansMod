namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Collections.Generic;
	using Contracts;
	using global::Rust;
	using Model;
	using Model.Rust.Contracts;
	using UnityEngine;

	public class Injuries : IInjuries
	{
		private const string Module = "Injuries";
		private static Injuries _instance;
		private readonly ILogger _logger;
		private readonly IPlayers _players;
		private readonly ITraps _traps;
		public readonly Dictionary<ulong, Injury> State = new Dictionary<ulong, Injury>();

		public Injuries()
			: this(Logger.Instance, Traps.Instance, Players.Instance)
		{
		}

		public Injuries(ILogger logger, ITraps traps, IPlayers players)
		{
			_logger = logger;
			_traps = traps;
			_players = players;
		}

		public static Injuries Instance => _instance ?? (_instance = new Injuries());

		public void UpdateInjuryStatus(IHitInfo hitInfo)
		{
			var player = hitInfo.HitEntity as IBasePlayer;

			if (player == null
			    || (player.IsDead()
			        || Equals(player, hitInfo.Initiator) && hitInfo.DamageTypes.GetMajorityDamageType() == DamageType.Bleeding))
			{
				// Don't update anything if
				//   - Not a player
				//   - The player is dead 
				//   - The "injury" is bleeding caused by a previous injury.
				return;
			}

			var newInjury = ResolveInjury(hitInfo);
			if (!State.ContainsKey(player.UserId))
			{
				State.Add(player.UserId, newInjury);
			}
			else
			{
				State[player.UserId] = newInjury;
			}
		}

		public bool TryGetLastRelevantInjury(IBasePlayer player, TimeSpan timeframe, out Injury injury)
		{
			if (!State.TryGetValue(player.UserId, out injury))
			{
				return false;
			}

			if (DateTime.UtcNow - injury.InjuryTime > timeframe)
			{
				return false;
			}

			return true;
		}

		public Injury ResolveInjury(IHitInfo hitInfo)
		{
			IBaseEntity initiator;
			float injuryDistance;
			ulong? trapId;

			// If it's a trap, we attribute the injury to the trap's owner.
			var trap = hitInfo.Initiator as ITrap;
			if (trap != null)
			{
				trapId = _traps.GetTrapId(trap);
				ulong ownerId = _traps.GetOwnerId(trap);
				IBasePlayer player;
				if (!_players.TryFindPlayerById(ownerId, out player))
				{
					_logger.Error(Module, "Unable to find player in ResolveInjury.");
				}
				injuryDistance = 0;
				initiator = player;
			}
			else
			{
				initiator = hitInfo.Initiator;
				trapId = null;
				injuryDistance = hitInfo.HitEntity != null
					? Vector3.Distance(hitInfo.HitEntity.Transform.position, initiator.Transform.position)
					: 0f;
			}

			var majorityDamageType = hitInfo.DamageTypes.GetMajorityDamageType();
			return new Injury(initiator, hitInfo.Weapon, majorityDamageType, trapId, injuryDistance, DateTime.UtcNow);
		}
	}
}