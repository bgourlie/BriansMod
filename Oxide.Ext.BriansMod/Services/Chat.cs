namespace Oxide.Ext.BriansMod.Services
{
	using Network;

	using Oxide.Core;
	using Oxide.Core.Plugins;
	using Oxide.Rust.Libraries;

	public class Chat : IChat
	{
		private static Chat instance;

		private static readonly Command Command = Interface.GetMod().GetLibrary<Command>("Command");

		public static Chat Instance => instance ?? (instance = new Chat());

		public void Send(Connection conn, string message, params object[] args)
		{
			ConsoleSystem.SendClientCommand(conn, "chat.add", "Brian's Mod", string.Format(message, args));
		}

		public void Send(BasePlayer player, string message, params object[] args)
		{
			player.SendConsoleCommand("chat.add", 0, string.Format(message, args), 1.0);
		}

		public void Broadcast(string message, params object[] args)
		{
			ConsoleSystem.Broadcast("chat.add", "Brian's Mod", string.Format(message, args));
		}

		public void AddCommand(string name, Plugin plugin, string callbackName)
		{
			Command.AddChatCommand(name, plugin, callbackName);
		}
	}
}