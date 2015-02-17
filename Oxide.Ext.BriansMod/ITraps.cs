namespace Oxide.Ext.BriansMod
{
	public interface ITraps
	{
		void RecordTrap(BasePlayer player, BearTrap trap);

		void DeleteTrap(BearTrap trap);

		ulong GetOwnerId(BearTrap trap);
	}
}