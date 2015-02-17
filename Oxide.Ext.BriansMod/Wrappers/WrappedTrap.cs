namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedTrap : WrappedBaseCombatEntity, ITrap
	{
		public WrappedTrap(BearTrap bearTrap)
			: base(bearTrap)
		{
		}
	}
}