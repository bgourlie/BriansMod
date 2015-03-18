namespace Oxide.Ext.BriansMod
{
	using System.Linq;
	using Core.Plugins;
	using JetBrains.Annotations;
	using Model.Rust;
	using Model.Rust.Contracts;
	using Network;
	using Services;
	using Services.Contracts;
	using UnityEngine;

	public class BriansModPluginGlue : CSPlugin
	{
		private const string Module = "PluginGlue";
		private readonly IChat _chat;
		private readonly IConsole _console;
		private readonly ILogger _logger;
		private readonly BriansModPlugin _plugin = new BriansModPlugin();
		private readonly IWrapper _wrapper;

		public BriansModPluginGlue(ILogger logger, IConsole console, IChat chat, IWrapper wrapper)
		{
			Name = "bmod";
			Title = "Brian's Mod";
			Author = "bgzee";
			HasConfig = false;
			_logger = logger;
			_console = console;
			_chat = chat;
			_wrapper = wrapper;
		}

		public BriansModPluginGlue()
			: this(Logger.Instance, Console.Instance, Chat.Instance, Wrapper.Instance)
		{
		}

		[HookMethod("Init"), UsedImplicitly]
		private void Init()
		{
			_plugin.Init();
			_chat.AddCommand("stats", this, "OnStats");
			_chat.AddCommand("leaderboard", this, "OnLeaderBoard");
			_chat.AddCommand("who", this, "OnWho");
#if DEBUG
			// Add a bunch of commands that make debugging easier
			_logger.Warn(Module, "BUILT IN DEBUG MODE.  DO NOT USE ON PRODUCTION SERVERS.");
			_chat.AddCommand("tp", this, "OnTp");
			_chat.AddCommand("arm", this, "OnArm");
			_console.AddCommand("bmod.listitems", this, "OnListItems");
#endif
		}

		[HookMethod("OnPlayerInit"), UsedImplicitly]
		private void OnPlayerInit(BasePlayer player)
		{
			_plugin.OnPlayerInit(new WrappedBasePlayer(player));
		}

		[HookMethod("OnPlayerDisconnected"), UsedImplicitly]
		private void OnPlayerDisconnected(BasePlayer player)
		{
			_plugin.OnPlayerDisconnected(new WrappedBasePlayer(player));
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
			_plugin.OnStats(new WrappedBasePlayer(player));
		}

		[HookMethod("OnLeaderBoard"), UsedImplicitly]
		private void OnLeaderBoard(BasePlayer player, string command, string[] args)
		{
			_plugin.OnLeaderBoard(new WrappedBasePlayer(player));
		}

		// Add a bunch of commands that make debugging easier
		[HookMethod("OnWho"), UsedImplicitly]
		private void OnWho(BasePlayer player, string command, string[] args)
		{
			_plugin.OnWho(new WrappedBasePlayer(player));
		}

#if DEBUG
		// Add a bunch of commands that make debugging easier
		[HookMethod("OnTp"), UsedImplicitly]
		private void OnTp(BasePlayer player, string command, string[] args)
		{
			var conns = Net.sv.connections.Select(conn => new WrappedConnection(conn)).Cast<IConnection>().ToList();
			_plugin.OnTp(new WrappedBasePlayer(player), conns);
		}

		[HookMethod("OnArm"), UsedImplicitly]
		private void OnArm(BasePlayer player, string command, string[] args)
		{
			_plugin.OnArm(new WrappedBasePlayer(player));
		}

		[HookMethod("OnListItems"), UsedImplicitly]
		private void OnListItems(ConsoleSystem.Arg args)
		{
			_plugin.OnListItems(new WrappedBasePlayer(args.Player()));
		}
#endif
	}
}