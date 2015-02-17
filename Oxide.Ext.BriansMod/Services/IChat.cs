namespace Oxide.Ext.BriansMod.Services
{
	using Network;

	using Oxide.Core.Plugins;

	public interface IChat
	{
		void Send(Connection conn, string message, params object[] args);

		void Send(BasePlayer player, string message, params object[] args);

		void Broadcast(string message, params object[] args);

		void AddCommand(string name, Plugin plugin, string callbackName);
	}
}