namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;

	public class WrappedBaseCorpse : WrappedBaseCombatEntity, IBaseCorpse
	{
		public WrappedBaseCorpse([NotNull] BaseCorpse baseCorpse)
			: base(baseCorpse)
		{
		}
	}
}