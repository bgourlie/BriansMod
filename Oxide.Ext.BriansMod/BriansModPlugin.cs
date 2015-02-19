namespace Oxide.Ext.BriansMod
{
	using System;
	using System.IO;

	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Services;

	public class BriansModPlugin
	{
		private const string Module = "Plugin";

		private readonly IChat chat;

		private readonly IConfiguration config;

		private readonly IData data;

		private readonly IDeaths deaths;

		private readonly IInjuries injuries;

		private readonly ILogger logger;

		private readonly ITraps traps;

		public BriansModPlugin(
			ILogger logger,
			IData data,
			IChat chat,
			IConfiguration config,
			IDeaths deaths,
			IInjuries injuries,
			ITraps traps)
		{
			this.deaths = deaths;
			this.logger = logger;
			this.config = config;
			this.data = data;
			this.injuries = injuries;
			this.chat = chat;
			this.traps = traps;
		}

		public BriansModPlugin()
			: this(
				Logger.Instance,
				Data.Instance,
				Chat.Instance,
				Configuration.Instance,
				Deaths.Instance,
				Injuries.Instance,
				Traps.Instance)
		{
		}

		public void Init()
		{
			var filename = Path.Combine(this.config.DataDirectory, "stats.db");
			if (!filename.StartsWith(this.config.DataDirectory, StringComparison.Ordinal))
			{
				throw new Exception("Only access to oxide directory!");
			}
			var connString = string.Format("Data Source={0};Version=3;", filename);
			this.data.InitializeStore(connString);
		}

		public void OnItemDeployed(IDeployer deployer, IBaseEntity deployedEntity)
		{
			if (deployedEntity is ITrap)
			{
				this.traps.RecordTrap(deployer.OwnerPlayer, (ITrap)deployedEntity);
			}
		}

		public void OnEntityAttacked(IMonoBehavior entity, IHitInfo hitInfo)
		{
			var player = entity as IBasePlayer;
			if (player != null)
			{
				this.injuries.UpdateInjuryStatus(hitInfo);
			}
		}

		public void OnEntityDeath(IMonoBehavior entity, IHitInfo hitinfo)
		{
			PvpDeath pvpDeath;
			if (this.deaths.TryResolvePvpDeath(entity, hitinfo, out pvpDeath))
			{
				this.deaths.Record(pvpDeath);
				this.chat.Broadcast(this.deaths.GetDeathMessage(pvpDeath));
			}
			else if (entity is ITrap)
			{
				this.traps.DestroyTrap((ITrap)entity);
			}
			else
			{
				this.logger.Debug(Module, "Ignoring non-pvp death.");
			}
		}
	}
}