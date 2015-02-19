namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedTrap : WrappedBaseCombatEntity, ITrap
	{
		public WrappedTrap(BearTrap bearTrap)
			: base(bearTrap)
		{
		}
	}
}