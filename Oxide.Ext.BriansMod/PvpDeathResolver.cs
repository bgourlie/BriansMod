namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public class PvpDeathResolver : IPvpDeathResolver
	{
		private const string Module = "PvpDeathResolver";

		private readonly ILogger logger;

		private readonly IPlayerResolver playerResolver;

		public PvpDeathResolver()
			: this(Logger.Instance, PlayerResolver.Instance)
		{
		}

		public PvpDeathResolver(ILogger logger, IPlayerResolver playerResolver)
		{
			this.logger = logger;
			this.playerResolver = playerResolver;
		}

		public bool TryResolve(MonoBehaviour entity, HitInfo hitinfo, out PvpDeath pvpDeath)
		{
			BasePlayer victim;
			if (this.playerResolver.TryResolvePlayer(entity, out victim))
			{
				BasePlayer killer;
				if (this.playerResolver.TryResolvePlayer(hitinfo.Initiator, out killer))
				{
					if (!killer.Equals(victim))
					{
						pvpDeath = new PvpDeath(victim, killer, victim.lastDamage);
						return true;
					}
				}
			}

			pvpDeath = null;
			return false;
		}
	}
}