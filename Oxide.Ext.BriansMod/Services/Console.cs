namespace Oxide.Ext.BriansMod.Services
{
	using Core;
	using Core.Plugins;
	using Network;
	using Rust.Libraries;

	public class Console : IConsole
	{
		private static Console _instance;
		private readonly Command _cmd = Interface.GetMod().GetLibrary<Command>("Command");

		public static Console Instance => _instance ?? (_instance = new Console());

		public void Send(BasePlayer player, string message, params object[] args)
		{
			player?.SendConsoleCommand("echo " + string.Format(message, args));
		}

		public void Send(Connection conn, string message, params object[] args)
		{
			Send(conn.player as BasePlayer, message, args);
		}

		public void AddCommand(string name, Plugin plugin, string callbackName)
		{
			_cmd.AddConsoleCommand(name, plugin, callbackName);
		}
	}
}