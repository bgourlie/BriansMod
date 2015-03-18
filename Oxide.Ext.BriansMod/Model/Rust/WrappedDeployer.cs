namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;

	public class WrappedDeployer : WrappedHeldEntity, IDeployer
	{
		public WrappedDeployer([NotNull] Deployer deployer)
			: base(deployer)
		{
		}
	}
}