namespace Oxide.Ext.BriansMod
{
	using Network;

	public interface IConsole
	{
		void Send(BasePlayer player, string message, params object[] args);
	}
}
