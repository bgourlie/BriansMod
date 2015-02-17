namespace Oxide.Ext.BriansMod
{
	using Network;

	using UnityEngine;

	public interface IConsole
	{
		void Send(BasePlayer player, string message, params object[] args);

		void Send(Connection conn, string message, params object[] args);
	}
}