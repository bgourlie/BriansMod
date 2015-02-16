namespace Oxide.Ext.BriansMod
{
	class Console : IConsole
	{
		// ReSharper disable once InconsistentNaming
		private static Console _instance;

		public static Console Instance => _instance ?? (_instance = new Console());

		public void Send(BasePlayer player, string message, params object[] args)
		{
			player.SendConsoleCommand("echo " + string.Format(message, args));
		}
	}
}