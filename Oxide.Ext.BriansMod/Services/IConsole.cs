namespace Oxide.Ext.BriansMod.Services
{
	using Network;

	public interface IConsole
	{
		void Send(BasePlayer player, string message, params object[] args);
		void Send(Connection conn, string message, params object[] args);
	}
}