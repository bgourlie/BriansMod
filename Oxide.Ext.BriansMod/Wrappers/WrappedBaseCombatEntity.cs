namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedBaseCombatEntity : WrappedBaseEntity, IBaseCombatEntity
	{
		public WrappedBaseCombatEntity(BaseCombatEntity baseCombatEntity)
			: base(baseCombatEntity)
		{
		}
	}
}