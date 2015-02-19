namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Collections.Generic;

	using global::Rust;

	using Oxide.Ext.BriansMod.Model;

	using UnityEngine;

	public class Injuries : IInjuries
	{
		private const string Module = "Injuries";

		private static Injuries instance;

		private readonly ILogger logger;

		private readonly IPlayers players;

		public readonly Dictionary<ulong, Injury> State = new Dictionary<ulong, Injury>();

		private readonly ITraps traps;

		public Injuries()
			: this(Logger.Instance, Traps.Instance, Players.Instance)
		{
		}

		public Injuries(ILogger logger, ITraps traps, IPlayers players)
		{
			this.logger = logger;
			this.traps = traps;
			this.players = players;
		}

		public static Injuries Instance => instance ?? (instance = new Injuries());

		public void UpdateInjuryStatus(IHitInfo hitInfo)
		{
			var player = hitInfo.HitEntity as IBasePlayer;

			if (player == null
			    || (player.IsDead()
			        || (player == hitInfo.Initiator && hitInfo.DamageTypes.GetMajorityDamageType() == DamageType.Bleeding)))
			{
				// Don't update anything if
				//   - Not a player
				//   - The player is dead 
				//   - The "injury" is bleeding caused by a previous injury.
				return;
			}

			var newInjury = this.ResolveInjury(hitInfo);
			var updated = false;
			Injury prevInjury;
			if (!this.State.TryGetValue(player.UserId, out prevInjury))
			{
				this.State.Add(player.UserId, newInjury);
				updated = true;
			}
			else
			{
				// If the previous injury and the new injury were both self-inflicted
				// and have the same damage type, don't update it (very spammy).
				// This is probably a micro optimization, but fuck it.
				if (
					!(prevInjury.Attacker == player && newInjury.Attacker == player
					  && prevInjury.PrimaryDamageType == newInjury.PrimaryDamageType))
				{
					this.State[player.UserId] = newInjury;
					updated = true;
				}
			}

			if (updated)
			{
				this.logger.Debug(Module, "Updated injury for {0} to {1}", player.DisplayName, newInjury);
			}
		}

		public bool TryGetLastRelevantInjury(IBasePlayer player, out Injury injury)
		{
			return this.State.TryGetValue(player.UserId, out injury);
		}

		public Injury ResolveInjury(IHitInfo hitInfo)
		{
			IMonoBehavior killer;
			float injuryDistance;
			bool causedByTrap;

			// If it's a trap, we attribute the injury to the trap's owner.
			if (hitInfo.Initiator is ITrap)
			{
				var ownerId = this.traps.GetOwnerId((ITrap)hitInfo.Initiator);
				IBasePlayer player;
				if (!this.players.TryFindPlayerById(ownerId, out player))
				{
					this.logger.Error(Module, "Unable to find player in ResolveInjury.");
				}
				injuryDistance = 0;
				killer = player;
				causedByTrap = true;
			}
			else
			{
				killer = hitInfo.Initiator;
				causedByTrap = false;
				injuryDistance = Vector3.Distance(hitInfo.HitEntity.Transform.position, hitInfo.Initiator.Transform.position);
			}

			var majorityDamageType = hitInfo.DamageTypes.GetMajorityDamageType();
			return new Injury(killer, hitInfo.Weapon, majorityDamageType, causedByTrap, injuryDistance, DateTime.UtcNow);
		}
	}
}