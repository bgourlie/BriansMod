namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedBaseNpc : WrappedBaseCombatEntity, IBaseNpc
	{
		public WrappedBaseNpc(BaseNPC baseNpc)
			: base(baseNpc)
		{
		}
	}
}