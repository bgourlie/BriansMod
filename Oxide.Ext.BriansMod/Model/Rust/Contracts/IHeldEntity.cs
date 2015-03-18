namespace Oxide.Ext.BriansMod.Model.Rust.Contracts
{
	public interface IHeldEntity : IBaseEntity
	{
		HoldType HoldType { get; }
		IBasePlayer OwnerPlayer { get; }
	}
}