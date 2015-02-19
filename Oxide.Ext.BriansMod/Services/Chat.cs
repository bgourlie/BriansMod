namespace Oxide.Ext.BriansMod.Services
{
	using Core;
	using Core.Plugins;
	using Network;
	using Rust.Libraries;

	public class Chat : IChat
	{
		private static Chat _instance;
		private static readonly Command Command = Interface.GetMod().GetLibrary<Command>("Command");
		public static Chat Instance => _instance ?? (_instance = new Chat());

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