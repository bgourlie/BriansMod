namespace Oxide.Ext.BriansMod
{
	using System.Linq;
	using System.Text;
	using Core.Plugins;
	using JetBrains.Annotations;
	using Model;
	using Network;
	using Rust.Libraries;
	using Services;
	using UnityEngine;
	using Wrappers;

	public class BriansModPluginGlue : CSPlugin
	{
		private const string Module = "PluginGlue";
		private readonly IChat _chat;
		private readonly IData _data;
		private readonly IConsole _console;
		private readonly ILogger _logger;
		private readonly BriansModPlugin _plugin = new BriansModPlugin();
		private readonly IWrapper _wrapper;
		private readonly IPlayers _players;

		public BriansModPluginGlue(ILogger logger, IConsole console, IChat chat, IWrapper wrapper, IData data, IPlayers players)
		{
			Name = "bmod";
			Title = "Brian's Mod";
			Author = "bgzee";
			HasConfig = false;
			_logger = logger;
			_console = console;
			_chat = chat;
			_wrapper = wrapper;
			_data = data;
			_players = players;
		}

		public BriansModPluginGlue()
			: this(Logger.Instance, Console.Instance, Chat.Instance, Wrapper.Instance, Data.Instance, Players.Instance)
		{
		}

		[HookMethod("Init"), UsedImplicitly]
		private void Init()
		{
			_plugin.Init();
			_chat.AddCommand("stats", this, "OnStats");
			_chat.AddCommand("who", this, "OnWho");
#if DEBUG
			// Add a bunch of commands that make debugging easier
			_logger.Warn(Module, "BUILT IN DEBUG MODE.  DO NOT USE ON PRODUCTION SERVERS.");
			_chat.AddCommand("tp", this, "OnTp");
			_chat.AddCommand("arm", this, "OnArm");
			_console.AddCommand("bmod.listitems", this, "OnListItems");
#endif
		}

		[HookMethod("OnItemDeployed"), UsedImplicitly]
		private void OnItemDeployed(Deployer deployer, BaseEntity deployedEntity)
		{
			IMonoBehavior baseEntity;
			if (_wrapper.TryWrap(deployedEntity, out baseEntity))
			{
				_plugin.OnItemDeployed(new WrappedDeployer(deployer), (IBaseEntity) baseEntity);
			}
		}

		[HookMethod("OnEntityAttacked"), UsedImplicitly]
		private void OnEntityAttacked(MonoBehaviour entity, HitInfo hitInfo)
		{
			IMonoBehavior wrappedEntity;
			if (_wrapper.TryWrap(entity, out wrappedEntity))
			{
				_plugin.OnEntityAttacked(wrappedEntity, new WrappedHitInfo(hitInfo));
			}
		}

		[HookMethod("OnEntityDeath"), UsedImplicitly]
		private void OnEntityDeath(MonoBehaviour entity, HitInfo hitInfo)
		{
			IMonoBehavior wrappedEntity;
			if (_wrapper.TryWrap(entity, out wrappedEntity))
			{
				_plugin.OnEntityDeath(wrappedEntity, new WrappedHitInfo(hitInfo));
			}
		}

		[HookMethod("OnStats"), UsedImplicitly]
		private void OnStats(BasePlayer player, string command, string[] args)
		{
			var stats = _data.GetWeaponStats().ToList();
			if (stats.Count == 0)
			{
				player.ChatMessage("There are no stats to display");
				return;
			}
			
			player.ChatMessage("View stats in the F1 console.");

			var sb = new StringBuilder();
			foreach (var stat in stats)
			{
				sb.AppendLine("--------");
				sb.AppendLine(stat.Weapon);
				sb.AppendLine("--------");
				if (stat.BestDistance > 0f)
				{
					sb.AppendLine(string.Format("Longest Kill: {0} meters", stat.BestDistance));
				}
				sb.AppendLine(string.Format("Total Kills: {0}", stat.NumKills));
			}
			_console.Send(player, sb.ToString());
		}

		// Add a bunch of commands that make debugging easier
		[HookMethod("OnWho"), UsedImplicitly]
		private void OnWho(BasePlayer player, string command, string[] args)
		{
			var sb = new StringBuilder();
			var activePlayers = _players.ActivePlayers.ToList();
			player.ChatMessage(string.Format("{0} active players.  Check the F1 console for a list of all players.", activePlayers.Count));
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

#if DEBUG
		// Add a bunch of commands that make debugging easier
		[HookMethod("OnTp"), UsedImplicitly]
		private void OnTp(BasePlayer player, string command, string[] args)
		{
			var blah = new Rust();
			var conns = Net.sv.connections;
			var toLoc = player.transform.position;
			for (var i = 1; i < conns.Count; i++)
			{
				var playerToMove = (BasePlayer) conns[i].player;
				blah.ForcePlayerPosition(playerToMove, toLoc.x, toLoc.y, toLoc.z);
			}
		}

		[HookMethod("OnArm"), UsedImplicitly]
		private void OnArm(BasePlayer player, string command, string[] args)
		{
			player.inventory.GiveItem(4131848, 1000); // pistol rounds
			player.inventory.GiveItem(-2076242409, 1000); // rifle rounds
			player.inventory.GiveItem(-1536699499, 1000); // shotgun rounds
			player.inventory.GiveItem(117733644, 100); // arrows
			player.inventory.GiveItem(-1355335174); // bow
			player.inventory.GiveItem(498591726, 10); // explosives
			player.inventory.GiveItem(-809130169); // building plan
			player.inventory.GiveItem(-1224598842); // hammer plan
			player.inventory.GiveItem(-51678842, 100); // lanterns
			player.inventory.GiveItem(-1461461759); // assault rifle
			player.inventory.GiveItem(-10407508); // bolt action
			player.inventory.GiveItem(698465195); // revolver
			player.inventory.GiveItem(-964239615); // shotgun
			player.inventory.GiveItem(2085492774); // thompson
			player.inventory.GiveItem(1091325318, 20); // snap traps
			player.inventory.GiveItem(3655341, 10000); // wood
			player.inventory.GiveItem(-1153983671, 20); // wolf meat
			player.inventory.GiveItem(-892070738, 10000); // stones
			player.inventory.GiveItem(-351194901, 10000); // metal frags
		}

		[HookMethod("OnListItems"), UsedImplicitly]
		private void OnListItems(ConsoleSystem.Arg args)
		{
			var itemsDefinition = ItemManager.GetItemDefinitions();
			foreach (var item in itemsDefinition.OrderBy(i => i.displayName.english))
			{
				_console.Send(args.connection, "{0} ({1}): {2}", item.displayName.english, item.rarity, item.itemid);
			}
		}
#endif
	}
}