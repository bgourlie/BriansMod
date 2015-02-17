namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedBaseCorpse : WrappedBaseCombatEntity, IBaseCorpse
	{
		public WrappedBaseCorpse(BaseCorpse baseCorpse)
			: base(baseCorpse)
		{
		}
	}
}