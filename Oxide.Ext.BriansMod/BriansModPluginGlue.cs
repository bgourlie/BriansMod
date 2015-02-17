namespace Oxide.Ext.BriansMod
{
	using System.Linq;

	using Network;

	using Oxide.Core.Plugins;
	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Services;
	using Oxide.Ext.BriansMod.Wrappers;
	using Oxide.Rust.Libraries;

	using UnityEngine;

	public class BriansModPluginGlue : CSPlugin
	{
		private const string Module = "PluginGlue";

		private readonly IChat chat;

		private readonly IConsole console;

		private readonly ILogger logger;

		private readonly BriansModPlugin plugin = new BriansModPlugin();

		private readonly IWrapper wrapper;

		public BriansModPluginGlue(ILogger logger, IConsole console, IChat chat, IWrapper wrapper)
		{
			this.Name = "bmod";
			this.Title = "Brian's Mod";
			this.Author = "W. Brian Gourlie";
			this.HasConfig = false;
			this.logger = logger;
			this.console = console;
			this.chat = chat;
			this.wrapper = wrapper;
		}

		public BriansModPluginGlue()
			: this(Logger.Instance, Console.Instance, Chat.Instance, Wrapper.Instance)
		{
		}

		[HookMethod("Init")]
		private void Init()
		{
			this.plugin.Init();
#if DEBUG
			// Add a bunch of commands that make debugging easier
			this.logger.Warn(Module, "BUILT IN DEBUG MODE.  DO NOT USE ON PRODUCTION SERVERS.");
			this.chat.AddCommand("tp", this, "OnTp");
			this.chat.AddCommand("arm", this, "OnArm");
			this.chat.AddCommand("listitems", this, "OnListItems");
#endif
		}

		[HookMethod("OnItemDeployed")]
		private void OnItemDeployed(Deployer deployer, BaseEntity deployedEntity)
		{
			IMonoBehavior baseEntity;
			if (this.wrapper.TryWrap(deployedEntity, out baseEntity))
			{
				this.plugin.OnItemDeployed(new WrappedDeployer(deployer), (IBaseEntity)baseEntity);
			}
		}

		[HookMethod("OnEntityAttacked")]
		private void OnEntityAttacked(MonoBehaviour entity, HitInfo hitInfo)
		{
			IMonoBehavior wrappedEntity;
			if (this.wrapper.TryWrap(entity, out wrappedEntity))
			{
				this.plugin.OnEntityAttacked(wrappedEntity, new WrappedHitInfo(hitInfo));
			}
		}

		[HookMethod("OnEntityDeath")]
		private void OnEntityDeath(MonoBehaviour entity, HitInfo hitInfo)
		{
			IMonoBehavior wrappedEntity;
			if (this.wrapper.TryWrap(entity, out wrappedEntity))
			{
				this.plugin.OnEntityDeath(wrappedEntity, new WrappedHitInfo(hitInfo));
			}
		}

#if DEBUG
		// Add a bunch of commands that make debugging easier
		[HookMethod("OnTp")]
		private void OnTp(BasePlayer player, string command, string[] args)
		{
			var blah = new Rust();
			var conns = Net.sv.connections;
			var toLoc = player.transform.position;
			for (var i = 1; i < conns.Count; i++)
			{
				var playerToMove = (BasePlayer)conns[i].player;
				blah.ForcePlayerPosition(playerToMove, toLoc.x, toLoc.y, toLoc.z);
			}
		}

		[HookMethod("OnArm")]
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
		}

		[HookMethod("OnListItems")]
		private void OnListItems(BasePlayer player, string command, string[] args)
		{
			var itemsDefinition = ItemManager.GetItemDefinitions();
			foreach (var item in itemsDefinition.OrderBy(i => i.displayName.english))
			{
				this.console.Send(player, "{0} ({1}): {2}", item.displayName.english, item.rarity, item.itemid);
			}
		}
#endif
	}
}