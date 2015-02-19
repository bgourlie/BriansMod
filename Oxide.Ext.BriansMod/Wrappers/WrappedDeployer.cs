namespace Oxide.Ext.BriansMod.Wrappers
{
	using JetBrains.Annotations;
	using Model;

	public class WrappedDeployer : WrappedHeldEntity, IDeployer
	{
		public WrappedDeployer([NotNull] Deployer deployer)
			: base(deployer)
		{
		}
	}
}