namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;

	public interface ITraps
	{
		void RecordTrap(IBasePlayer player, ITrap trap);

		void DeleteTrap(ITrap trap);

		ulong GetOwnerId(ITrap trap);
	}
}