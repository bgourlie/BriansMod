namespace Oxide.Ext.BriansMod.Wrappers
{
	using JetBrains.Annotations;
	using Model;

	public class WrappedBaseNpc : WrappedBaseCombatEntity, IBaseNpc
	{
		public WrappedBaseNpc([NotNull] BaseNPC baseNpc)
			: base(baseNpc)
		{
		}
	}
}