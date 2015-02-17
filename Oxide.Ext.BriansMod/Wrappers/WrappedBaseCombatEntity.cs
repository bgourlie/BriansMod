namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedBaseCombatEntity : WrappedBaseEntity, IBaseCombatEntity
	{
		public WrappedBaseCombatEntity(BaseCombatEntity baseCombatEntity)
			: base(baseCombatEntity)
		{
		}
	}
}