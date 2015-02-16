namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public class PvpDeathResolver : IPvpDeathResolver
	{
		private const string Module = "PvpDeathResolver";

		private readonly ILogger logger;

		private readonly IInjuryTracker injuryTracker;

		public PvpDeathResolver()
			: this(Logger.Instance, InjuryTracker.Instance)
		{
		}

		public PvpDeathResolver(ILogger logger, IInjuryTracker injuryTracker)
		{
			this.logger = logger;
			this.injuryTracker = injuryTracker;
		}

		public bool TryResolve(MonoBehaviour entity, HitInfo hitinfo, out PvpDeath pvpDeath)
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