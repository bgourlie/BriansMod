namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedBaseCorpse : WrappedBaseCombatEntity, IBaseCorpse
	{
		public WrappedBaseCorpse(BaseCorpse baseCorpse)
			: base(baseCorpse)
		{
		}
	}
}