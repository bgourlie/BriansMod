namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;

	public class WrappedTrap : WrappedBaseCombatEntity, ITrap
	{
		public WrappedTrap([NotNull] BearTrap trap)
			: base(trap)
		{
		}
	}
}