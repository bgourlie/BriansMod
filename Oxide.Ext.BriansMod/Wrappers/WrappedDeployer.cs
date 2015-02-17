namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedDeployer : WrappedHeldEntity, IDeployer
	{
		public WrappedDeployer(Deployer deployer)
			: base(deployer)
		{
		}
	}
}