﻿namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using Core.Plugins;
	using JetBrains.Annotations;
	using Model.Rust.Contracts;
	using Network;

	public interface IChat
	{
		void Send([NotNull] Connection conn, string message, params object[] args);
		void Send([NotNull] IBasePlayer player, string message, params object[] args);
		void Broadcast([NotNull] string message, params object[] args);
		void AddCommand([NotNull] string name, [NotNull] Plugin plugin, [NotNull] string callbackName);
	}
}