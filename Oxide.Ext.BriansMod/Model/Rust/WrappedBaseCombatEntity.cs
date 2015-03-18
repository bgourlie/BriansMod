namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;

	public class WrappedBaseCombatEntity : WrappedBaseEntity, IBaseCombatEntity
	{
		public WrappedBaseCombatEntity(BaseCombatEntity baseCombatEntity)
			: base(baseCombatEntity)
		{
		}
	}
}