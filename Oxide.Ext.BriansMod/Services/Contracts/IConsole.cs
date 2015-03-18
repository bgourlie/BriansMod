namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using Core.Plugins;
	using Model.Rust.Contracts;

	public interface IConsole
	{
		void Send(IBasePlayer player, string message, params object[] args);
		void Send(IConnection conn, string message, params object[] args);
		void AddCommand(string name, Plugin plugin, string callbackName);
	}
}