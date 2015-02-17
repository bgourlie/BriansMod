namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Collections.Generic;

	using global::Rust;

	using Model;

	using UnityEngine;

	public class Injuries : IInjuries
	{
		private const string Module = "Injuries";

		private static Injuries instance;

		public static Injuries Instance => instance ?? (instance = new Injuries());

		public readonly Dictionary<ulong, Injury> State = new Dictionary<ulong, Injury>();

		private readonly ILogger logger;

		public Injuries()
			: this(Logger.Instance)
		{
		}

		public Injuries(ILogger logger)
		{
			this.logger = logger;
		}

		public void UpdateInjuryStatus(HitInfo hitInfo)
		{
			var player = hitInfo.HitEntity as BasePlayer;

			if (player == null
			    || (player.IsDead()
			        || (player == hitInfo.Initiator && hitInfo.damageTypes.GetMajorityDamageType() == DamageType.Bleeding)))
			{
				// Don't update anything if
				//   - Not a player
				//   - The player is dead 
				//   - The "injury" is bleeding caused by a previous injury.
				return;
			}

			var newInjury = this.ResolveInjury(hitInfo);
			bool updated = false;
			Injury prevInjury;
			if (!this.State.TryGetValue(player.userID, out prevInjury))
			{
				this.State.Add(player.userID, newInjury);
				updated = true;
			}
			else
			{
				// If the previous injury and the new injury were both self-inflicted
				// and have the same damage type, don't update it (very spammy).
				// This is probably a micro optimization, but fuck it.
				if (
					!(prevInjury.CausedBy == player && newInjury.CausedBy == player
					  && prevInjury.PrimaryDamageType == newInjury.PrimaryDamageType))
				{
					this.State[player.userID] = newInjury;
					updated = true;
				}
			}

			if (updated)
			{
				this.logger.Debug(Module, "Updated injury for {0} to {1}", player.displayName, newInjury);
			}
		}

		public bool TryGetLastRelevantInjury(BasePlayer player, out Injury injury)
		{
			return this.State.TryGetValue(player.userID, out injury);
		}

		public Injury ResolveInjury(HitInfo hitInfo)
		{
			// hitInfo.HitEntity can be null (in case of BearTrap injury I've noticed so far)
			var injuryDistance = hitInfo.HitEntity != null
				                     ? Vector3.Distance(hitInfo.HitEntity.transform.position, hitInfo.Initiator.transform.position)
				                     : 0f;
			var majorityDamageType = hitInfo.damageTypes.GetMajorityDamageType();
			return new Injury(hitInfo.Initiator, hitInfo.Weapon, majorityDamageType, injuryDistance, DateTime.UtcNow);
		}
	}
}