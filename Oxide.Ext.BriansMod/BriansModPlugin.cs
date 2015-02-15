namespace Oxide.Ext.BriansMod
{
	using Core.Plugins;

	using Oxide.Ext.BriansMod.Model;

	using UnityEngine;

	public class BriansModPlugin : CSPlugin
	{
		private const string Module = "Plugin";

		private readonly ILogger logger;

		private readonly IPvpDeathResolver pvpDeathResolver;

		private readonly IPlayerResolver playerResolver;

		private readonly IData data;

		public BriansModPlugin(ILogger logger, IData data, IPlayerResolver playerResolver)
		{
			this.Name = "Brian's Mod";
			this.Title = "Brian's Mod";
			this.Author = "W. Brian Gourlie";
			this.HasConfig = false;
			this.pvpDeathResolver = new PvpDeathResolver();
			this.logger = logger;
			this.data = data;
			this.playerResolver = playerResolver;
		}

		public BriansModPlugin()
			: this(Logger.Instance, Data.Instance, PlayerResolver.Instance)
		{
		}

		[HookMethod("Init")]
		private void Init()
		{
			this.data.InitializeStore();
		}

		[HookMethod("OnPlayerSpawn")]
		private void OnPlayerSpawn(BasePlayer basePlayer)
		{
			this.data.RecordPlayer(basePlayer);
		}

		[HookMethod("OnEntityDeath")]
		private void OnEntityDeath(MonoBehaviour entity, HitInfo hitinfo)
		{
			PvpDeath pvpDeath;
			if (this.pvpDeathResolver.TryResolve(entity, hitinfo, out pvpDeath))
			{
				this.data.RecordDeath(pvpDeath);
			}
			else
			{
				this.logger.Info(Module, "Ignoring non-pvp death.");
			}
		}
	}
}