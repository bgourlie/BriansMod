namespace Oxide.Ext.BriansMod
{
	using Network;

	public class Console : IConsole
	{
		// ReSharper disable once InconsistentNaming
		private static Console _instance;

		public static Console Instance => _instance ?? (_instance = new Console());

		public void Send(BasePlayer player, string message, params object[] args)
		{
			player?.SendConsoleCommand("echo " + string.Format(message, args));
		}

		public void Send(Connection conn, string message, params object[] args)
		{
			this.Send(conn.player as BasePlayer, message, args);
		}
	}
}