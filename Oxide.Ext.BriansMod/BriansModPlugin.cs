namespace Oxide.Ext.BriansMod
{
	using System.CodeDom;
	using System.Collections.Generic;

	using Core.Plugins;

	using Model;

	using Network;

	using Oxide.Core;
	using Oxide.Rust.Libraries;

	using UnityEngine;

	public class BriansModPlugin : CSPlugin
	{
		private const string Module = "Plugin";

		private readonly IChat chat;

		private readonly IConsole console;

		private readonly ILogger logger;

		private readonly IDeathResolver deathResolver;

		private readonly IInjuryTracker injuryTracker;

		private readonly IData data;

		public BriansModPlugin(ILogger logger, IData data, IChat chat, IConsole console, IInjuryTracker injuryTracker)
		{
			this.Name = "Brian's Mod";
			this.Title = "Brian's Mod";
			this.Author = "W. Brian Gourlie";
			this.HasConfig = false;
			this.deathResolver = new DeathResolver();
			this.logger = logger;
			this.console = console;
			this.data = data;
			this.injuryTracker = injuryTracker;
			this.chat = chat;
		}

		public BriansModPlugin()
			: this(Logger.Instance, Data.Instance, Chat.Instance, Console.Instance, InjuryTracker.Instance)
		{
		}

		[HookMethod("Init")]
		private void Init()
		{
			this.data.InitializeStore();

#if DEBUG
			// Add a bunch of commands that make debugging easier
			this.logger.Warn(Module, "BUILT IN DEBUG MODE.  DO NOT USE ON PRODUCTION SERVERS.");
			var cmd = Interface.GetMod().GetLibrary<Command>("Command");
			cmd.AddChatCommand("tp", this, "OnTp");
			cmd.AddChatCommand("arm", this, "OnArm");
			cmd.AddChatCommand("listitems", this, "OnListItems");
#endif
		}

#if DEBUG
		// Add a bunch of commands that make debugging easier
		[HookMethod("OnTp")]
		private void OnTp(BasePlayer player, string command, string[] args)
		{
			var blah = new Rust();
			var conns = Net.sv.connections;
			var toLoc = player.transform.position;
			for (int i = 1; i < conns.Count; i++)
			{
				var playerToMove = (BasePlayer)conns[i].player;
				blah.ForcePlayerPosition(playerToMove, toLoc.x, toLoc.y, toLoc.z);
			}
		}

		[HookMethod("OnArm")]
		private void OnArm(BasePlayer player, string command, string[] args)
		{
			player.inventory.GiveItem(14101, 1000); // pistol rounds
			player.inventory.GiveItem(13987, 1000); // rifle rounds
			player.inventory.GiveItem(14100, 1000); // shotgun rounds
			player.inventory.GiveItem(11904, 100); // arrows
			player.inventory.GiveItem(6296); // bow
			player.inventory.GiveItem(14387, 10); // explosives
			player.inventory.GiveItem(14291); // building plan
			player.inventory.GiveItem(13851); // hammer plan
			player.inventory.GiveItem(14038, 100); // lanterns
			player.inventory.GiveItem(14343); // assault rifle
			player.inventory.GiveItem(11864); // bolt action
			player.inventory.GiveItem(14103); // revolver
			player.inventory.GiveItem(14391); // shotgun
			player.inventory.GiveItem(14102); // thompson
			player.inventory.GiveItem(836, 20); // snap traps
			player.inventory.GiveItem(514, 10000); // wood
			player.inventory.GiveItem(14086, 20); // wolf meat
			player.inventory.GiveItem(11964, 10000); // stones
		}

		[HookMethod("OnListItems")]
		private void OnListItems(BasePlayer player, string command, string[] args)
		{
			var itemsDefinition = ItemManager.GetItemDefinitions();
			foreach (var item in itemsDefinition)
			{
				this.console.Send(player, "{0}: {1}", item.displayname, item.itemid);
			}
		}
#endif

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