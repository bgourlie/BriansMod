namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;

	public class WrappedBaseNpc : WrappedBaseCombatEntity, IBaseNpc
	{
		public WrappedBaseNpc([NotNull] BaseNPC baseNpc)
			: base(baseNpc)
		{
		}
	}
}