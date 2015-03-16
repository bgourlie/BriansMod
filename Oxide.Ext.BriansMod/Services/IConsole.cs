namespace Oxide.Ext.BriansMod.Services
{
	using Core.Plugins;
	using Model;

	public interface IConsole
	{
		void Send(IBasePlayer player, string message, params object[] args);
		void Send(IConnection conn, string message, params object[] args);
		void AddCommand(string name, Plugin plugin, string callbackName);
	}
}