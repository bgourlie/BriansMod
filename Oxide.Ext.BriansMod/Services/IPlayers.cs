namespace Oxide.Ext.BriansMod.Services
{
	using Model;

	public interface IPlayers
	{
		bool TryFindPlayerById(ulong userId, out IBasePlayer player);
	}
}