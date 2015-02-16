namespace Oxide.Ext.BriansMod
{
	using Network;

	public interface IChat
	{
		void Send(Connection conn, string message, params object[] args);

		void Broadcast(string message, params object[] args);
	}
}