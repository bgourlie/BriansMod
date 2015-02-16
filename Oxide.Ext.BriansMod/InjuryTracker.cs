namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Collections.Generic;

	using global::Rust;

	using Model;

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
			if (player == hitInfo.Initiator && hitInfo.damageTypes.GetMajorityDamageType() == DamageType.Bleeding)
			{
				// Don't update anything if the "injury" is bleeding caused by a previous injury.
				return;
			}

			var lastInjury = new Injury(DateTime.UtcNow, hitInfo);
			this.logger.Info(Module, "Damage updated for {0}: {1}", player.displayName, lastInjury);
			lock (this.State)
			{
				if (!this.State.ContainsKey(player.userID))
				{
					this.State.Add(player.userID, lastInjury);
				}
				else
				{
					this.State[player.userID] = lastInjury;
				}
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