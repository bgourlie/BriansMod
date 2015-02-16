namespace Oxide.Ext.BriansMod
{
	using Core.Plugins;

	using Oxide.Ext.BriansMod.Model;

	using UnityEngine;

	public class BriansModPlugin : CSPlugin
	{
		private const string Module = "Plugin";

		private readonly IChat chat;

		private readonly ILogger logger;

		private readonly IDeathResolver deathResolver;

		private readonly IInjuryTracker injuryTracker;

		private readonly IData data;

		public BriansModPlugin(ILogger logger, IData data, IChat chat, IInjuryTracker injuryTracker)
		{
			this.Name = "Brian's Mod";
			this.Title = "Brian's Mod";
			this.Author = "W. Brian Gourlie";
			this.HasConfig = false;
			this.deathResolver = new DeathResolver();
			this.logger = logger;
			this.data = data;
			this.injuryTracker = injuryTracker;
			this.chat = chat;
		}

		public BriansModPlugin()
			: this(Logger.Instance, Data.Instance, Chat.Instance, InjuryTracker.Instance)
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

		[HookMethod("OnEntityAttacked")]
		private void OnEntityAttacked(MonoBehaviour entity, HitInfo hitInfo)
		{
			var player = entity as BasePlayer;
			if (player != null)
			{
				this.injuryTracker.UpdateInjuryStatus(player, hitInfo);
			}
		}

		[HookMethod("OnEntityDeath")]
		private void OnEntityDeath(MonoBehaviour entity, HitInfo hitinfo)
		{
			PvpDeath pvpDeath;
			if (this.deathResolver.TryResolvePvpDeath(entity, hitinfo, out pvpDeath))
			{
				this.chat.Broadcast(pvpDeath.ToString());
				this.data.RecordDeath(pvpDeath);
			}
			else
			{
				this.logger.Info(Module, "Ignoring non-pvp death.");
			}
		}
	}
}