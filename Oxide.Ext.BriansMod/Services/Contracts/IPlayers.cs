namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using System.Collections.Generic;
	using Model;
	using Model.Rust.Contracts;

	public interface IPlayers
	{
		IEnumerable<IBasePlayer> ActivePlayers { get; }
		IEnumerable<IBasePlayer> Sleepers { get; }
		bool TryFindPlayerById(ulong userId, out IBasePlayer player);
		PlayerFindResult FindPlayer(string startsWith);
	}
}