namespace Oxide.Ext.BriansMod.Services
{
	using System.Collections.Generic;
	using Model;

	public interface IPlayers
	{
		IEnumerable<IBasePlayer> ActivePlayers { get; }
		IEnumerable<IBasePlayer> Sleepers { get; }
		bool TryFindPlayerById(ulong userId, out IBasePlayer player);
	}
}