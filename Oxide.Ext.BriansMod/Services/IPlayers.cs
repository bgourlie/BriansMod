namespace Oxide.Ext.BriansMod.Services
{
	using System.Collections.Generic;
	using Model;

	public interface IPlayers
	{
		bool TryFindPlayerById(ulong userId, out IBasePlayer player);
		IEnumerable<IBasePlayer> ActivePlayers { get; }
		IEnumerable<IBasePlayer> Sleepers { get; }
	}
}