namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Collections.Generic;

	using global::Rust;

	using Model;

	using UnityEngine;

	public class InjuryTracker : IInjuryTracker
	{
		private const string Module = "InjuryTracker";

		private static InjuryTracker instance;

		public static InjuryTracker Instance => instance ?? (instance = new InjuryTracker());

		public readonly Dictionary<ulong, Injury> State = new Dictionary<ulong, Injury>();

		private readonly ILogger logger;

		public InjuryTracker()
			: this(Logger.Instance)
		{
		}

		public InjuryTracker(ILogger logger)
		{
			this.logger = logger;
		}

		public void UpdateInjuryStatus(BasePlayer player, HitInfo hitInfo)
		{
			if (player.IsDead() 
				|| (player == hitInfo.Initiator && hitInfo.damageTypes.GetMajorityDamageType() == DamageType.Bleeding))
			{
				// Don't update anything if the player is dead or the "injury" is bleeding caused by a previous injury.
				return;
			}

			// Todo: Have some sort of InjuryFactory that takes over this logic?
			var injuryDistance = Vector3.Distance(player.transform.position, hitInfo.Initiator.transform.position);
			var majorityDamageType = hitInfo.damageTypes.GetMajorityDamageType();
			var newInjury = new Injury(hitInfo.Initiator, majorityDamageType, injuryDistance, DateTime.UtcNow);
			bool updated = false;

			lock (this.State)
			{
				Injury prevInjury;
				if(!this.State.TryGetValue(player.userID, out prevInjury))
				{
					this.State.Add(player.userID, newInjury);
					updated = true;
				}
				else
				{
					// If the previous injury and the new injury were both self-inflicted
					// and have the same damage type, don't update it (very spammy).
					// This is probably a micro optimization, but fuck it.
					if (!(prevInjury.CausedBy == player && 
						newInjury.CausedBy == player && 
						prevInjury.PrimaryDamageType == newInjury.PrimaryDamageType))
					{
						this.State[player.userID] = newInjury;
						updated = true;
					}
				}
			}

			if (updated)
			{
				this.logger.Info(Module, "Damage updated for {0}: {1}", player.displayName, newInjury);
			}
		}

		public bool TryGetLastInjury(BasePlayer player, out Injury injury)
		{
			lock (this.State)
			{
				return this.State.TryGetValue(player.userID, out injury);
			}
		}
	}
}