namespace Oxide.Ext.BriansMod.Wrappers
{
	using JetBrains.Annotations;
	using Model;

	public class WrappedTrap : WrappedBaseCombatEntity, ITrap
	{
		public WrappedTrap([NotNull] BearTrap trap)
			: base(trap)
		{
		}
	}
}