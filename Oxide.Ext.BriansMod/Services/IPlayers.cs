namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;

	public interface IPlayers
	{
		bool TryFindPlayerById(ulong userId, out IBasePlayer player);
	}
}