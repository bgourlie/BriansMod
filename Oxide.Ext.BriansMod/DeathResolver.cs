namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public class DeathResolver : IDeathResolver
	{
		private const string Module = "DeathResolver";

		private readonly ILogger logger;

		private readonly IInjuryTracker injuryTracker;

		public DeathResolver()
			: this(Logger.Instance, InjuryTracker.Instance)
		{
		}

		public DeathResolver(ILogger logger, IInjuryTracker injuryTracker)
		{
			this.logger = logger;
			this.injuryTracker = injuryTracker;
		}

		public bool TryResolvePvpDeath(MonoBehaviour entity, HitInfo hitinfo, out PvpDeath pvpDeath)
		{
			var victim = entity as BasePlayer;
			if (victim != null)
			{
				Injury lastInjury;
				if (!this.injuryTracker.TryGetLastInjury(victim, out lastInjury))
				{
					this.logger.Warn(Module, "Unable to determine lastest injury.");
					goto failed;
				}

				var killer = lastInjury.CausedBy as BasePlayer;
				if (killer != null)
				{
					if (!killer.Equals(victim))
					{
						pvpDeath = new PvpDeath(victim, killer, lastInjury);
						return true;
					}
				}
			}

			failed:
			pvpDeath = null;
			return false;
		}
	}
}