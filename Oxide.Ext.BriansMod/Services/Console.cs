namespace Oxide.Ext.BriansMod.Services
{
	using Contracts;
	using Core;
	using Core.Plugins;
	using Model.Rust.Contracts;
	using Rust.Libraries;

	public class Console : IConsole
	{
		private static Console _instance;
		private readonly Command _cmd = Interface.GetMod().GetLibrary<Command>("Command");
		public static Console Instance => _instance ?? (_instance = new Console());

		public void Send(IBasePlayer player, string message, params object[] args)
		{
			player?.SendConsoleCommand("echo " + string.Format(message, args));
		}

		public void Send(IConnection conn, string message, params object[] args)
		{
			Send(conn.Player, message, args);
		}

		public void AddCommand(string name, Plugin plugin, string callbackName)
		{
			_cmd.AddConsoleCommand(name, plugin, callbackName);
		}
	}
}