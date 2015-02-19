namespace Oxide.Ext.BriansMod.Wrappers
{
	using JetBrains.Annotations;
	using Model;

	public class WrappedBaseCorpse : WrappedBaseCombatEntity, IBaseCorpse
	{
		public WrappedBaseCorpse([NotNull] BaseCorpse baseCorpse)
			: base(baseCorpse)
		{
		}
	}
}