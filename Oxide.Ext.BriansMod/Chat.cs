namespace Oxide.Ext.BriansMod
{
	using Network;

	public class Chat : IChat
	{
		private static Chat instance;

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
	}
}