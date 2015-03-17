namespace Oxide.Ext.BriansMod
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Model;
	using Services;

	public class BriansModPlugin
	{
		private const string Module = "Plugin";
		private readonly IChat _chat;
		private readonly IConfiguration _config;
		private readonly IConsole _console;
		private readonly IData _data;
		private readonly IDeaths _deaths;
		private readonly IInjuries _injuries;
		private readonly ILogger _logger;
		private readonly IPlayers _players;
		private readonly ITraps _traps;

		public BriansModPlugin(
			ILogger logger,
			IData data,
			IChat chat,
			IConfiguration config,
			IDeaths deaths,
			IInjuries injuries,
			ITraps traps,
			IConsole console,
			IPlayers players)
		{
			_deaths = deaths;
			_logger = logger;
			_config = config;
			_data = data;
			_injuries = injuries;
			_chat = chat;
			_traps = traps;
			_console = console;
			_players = players;
		}

		public BriansModPlugin()
			: this(
				Logger.Instance,
				Data.Instance,
				Chat.Instance,
				Configuration.Instance,
				Deaths.Instance,
				Injuries.Instance,
				Traps.Instance,
				Console.Instance,
				Players.Instance)
		{
		}

		public void Init()
		{
			string connString = string.Format("Data Source={0};Version=3;", _config.DatabaseLocation);
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

		public void OnPlayerInit(IBasePlayer player)
		{
			_chat.Broadcast("{0} has connected.", player.DisplayName);
		}

		public void OnPlayerDisconnected(IBasePlayer player)
		{
			_chat.Broadcast("{0} has disconnected.", player.DisplayName);
		}

		public void OnStats(IBasePlayer player)
		{
			var stats = _data.GetWeaponStats().ToList();
			var table = new TextTable("Weapon", "Total Kills", "Best Kill Distance");
			foreach (var stat in stats)
			{
				table.AddRow(stat.Weapon, stat.NumKills, stat.BestDistance);
			}
			_chat.Send(player, "View stats in the F1 console.");
			_console.Send(player, table.ToString());
		}

		public void OnWho(IBasePlayer player)
		{
			var sb = new StringBuilder();
			var activePlayers = _players.ActivePlayers.ToList();
			_chat.Send(player,
				string.Format("{0} active players.  Check the F1 console for a list of all players.", activePlayers.Count));
			sb.AppendLine();
			sb.AppendLine("----------------");
			sb.AppendLine(" ACTIVE PLAYERS");
			sb.AppendLine("----------------");
			foreach (var activePlayer in activePlayers)
			{
				sb.AppendLine(activePlayer.DisplayName);
			}
			sb.AppendLine();
			_console.Send(player, sb.ToString());
		}

		#region Debug Only Methods (will not be hooked in release)

		public void OnTp(IBasePlayer player, List<IConnection> conns)
		{
			var toLoc = player.Transform.position;
			for (var i = 1; i < conns.Count; i++)
			{
				var playerToMove = conns[i].Player;
				playerToMove.ForcePosition(toLoc.x, toLoc.y, toLoc.z);
			}
		}

		public void OnListItems(IBasePlayer player)
		{
			var itemsDefinition = ItemManager.GetItemDefinitions();
			foreach (var item in itemsDefinition.OrderBy(i => i.displayName.english))
			{
				_console.Send(player, "{0} ({1}): {2}", item.displayName.english, item.rarity, item.itemid);
			}
		}

		public void OnArm(IBasePlayer player)
		{
			player.Inventory.GiveItem(4131848, 1000); // pistol rounds
			player.Inventory.GiveItem(-2076242409, 1000); // rifle rounds
			player.Inventory.GiveItem(-1536699499, 1000); // shotgun rounds
			player.Inventory.GiveItem(117733644, 100); // arrows
			player.Inventory.GiveItem(-1355335174); // bow
			player.Inventory.GiveItem(498591726, 10); // explosives
			player.Inventory.GiveItem(-809130169); // building plan
			player.Inventory.GiveItem(-1224598842); // hammer plan
			player.Inventory.GiveItem(-51678842, 100); // lanterns
			player.Inventory.GiveItem(-1461461759); // assault rifle
			player.Inventory.GiveItem(-10407508); // bolt action
			player.Inventory.GiveItem(698465195); // revolver
			player.Inventory.GiveItem(-964239615); // shotgun
			player.Inventory.GiveItem(2085492774); // thompson
			player.Inventory.GiveItem(1091325318, 20); // snap traps
			player.Inventory.GiveItem(3655341, 10000); // wood
			player.Inventory.GiveItem(-1153983671, 20); // wolf meat
			player.Inventory.GiveItem(-892070738, 10000); // stones
			player.Inventory.GiveItem(-351194901, 10000); // metal frags
		}

		#endregion
	}
}