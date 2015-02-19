namespace Oxide.Ext.BriansMod
{
	using System;
	using System.IO;
	using Model;
	using Services;

	public class BriansModPlugin
	{
		private const string Module = "Plugin";
		private readonly IChat _chat;
		private readonly IConfiguration _config;
		private readonly IData _data;
		private readonly IDeaths _deaths;
		private readonly IInjuries _injuries;
		private readonly ILogger _logger;
		private readonly ITraps _traps;

		public BriansModPlugin(
			ILogger logger,
			IData data,
			IChat chat,
			IConfiguration config,
			IDeaths deaths,
			IInjuries injuries,
			ITraps traps)
		{
			_deaths = deaths;
			_logger = logger;
			_config = config;
			_data = data;
			_injuries = injuries;
			_chat = chat;
			_traps = traps;
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
			string filename = Path.Combine(_config.DataDirectory, "stats.db");
			if (!filename.StartsWith(_config.DataDirectory, StringComparison.Ordinal))
			{
				throw new Exception("Only access to oxide directory!");
			}
			string connString = string.Format("Data Source={0};Version=3;", filename);
			_data.InitializeStore(connString);
		}

		public void OnItemDeployed(IDeployer deployer, IBaseEntity deployedEntity)
		{
			var trap = deployedEntity as ITrap;
			if (trap != null)
			{
				_traps.RecordTrap(deployer.OwnerPlayer, trap);
			}
		}

		public void OnEntityAttacked(IMonoBehavior entity, IHitInfo hitInfo)
		{
			var player = entity as IBasePlayer;
			if (player != null)
			{
				_injuries.UpdateInjuryStatus(hitInfo);
			}
		}

		public void OnEntityDeath(IMonoBehavior entity, IHitInfo hitinfo)
		{
			PvpDeath pvpDeath;
			if (_deaths.TryResolvePvpDeath(entity, hitinfo, out pvpDeath))
			{
				_deaths.Record(pvpDeath);
				_chat.Broadcast(_deaths.GetDeathMessage(pvpDeath));
			}
			else
			{
				var trap = entity as ITrap;
				if (trap != null)
				{
					_traps.DestroyTrap(trap);
				}
				else
				{
					_logger.Debug(Module, "Ignoring non-pvp death.");
				}
			}
		}
	}
}