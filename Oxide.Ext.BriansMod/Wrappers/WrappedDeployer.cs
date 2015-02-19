namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedDeployer : WrappedHeldEntity, IDeployer
	{
		public WrappedDeployer(Deployer deployer)
			: base(deployer)
		{
		}
	}
}