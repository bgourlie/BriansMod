namespace Oxide.Ext.BriansMod
{
	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Services;

	public class BriansModPlugin
	{
		private const string Module = "Plugin";

		private readonly IChat chat;

		private readonly IConsole console;

		private readonly IData data;

		private readonly IDeaths deaths;

		private readonly IInjuries injuries;

		private readonly ILogger logger;

		private readonly ITraps traps;

		public BriansModPlugin(
			ILogger logger,
			IData data,
			IChat chat,
			IConsole console,
			IDeaths deaths,
			IInjuries injuries,
			ITraps traps)
		{
			this.deaths = deaths;
			this.logger = logger;
			this.console = console;
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
				Console.Instance,
				Deaths.Instance,
				Injuries.Instance,
				Traps.Instance)
		{
		}

		public void Init()
		{
			this.data.InitializeStore();
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
				this.chat.Broadcast(pvpDeath.ToString());
				this.deaths.Record(pvpDeath);
			}
			else if (entity is ITrap)
			{
				this.traps.DeleteTrap((ITrap)entity);
			}
			else
			{
				this.logger.Info(Module, "Ignoring non-pvp death.");
			}
		}
	}
}