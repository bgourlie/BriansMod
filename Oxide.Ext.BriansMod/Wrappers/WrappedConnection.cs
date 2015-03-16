﻿namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;
	using Network;

	public class WrappedConnection : IConnection
	{
		private readonly Connection _conn;

		public WrappedConnection(Connection conn)
		{
			_conn = conn;
			Player = new WrappedBasePlayer((BasePlayer) conn.player);
		}

		public IBasePlayer Player { get; }
	}
}