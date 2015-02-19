namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedBaseNpc : WrappedBaseCombatEntity, IBaseNpc
	{
		public WrappedBaseNpc(BaseNPC baseNpc)
			: base(baseNpc)
		{
		}
	}
}