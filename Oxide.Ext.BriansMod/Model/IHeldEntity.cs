namespace Oxide.Ext.BriansMod.Model
{
	public interface IHeldEntity : IBaseEntity
	{
		HoldType HoldType { get; }
		IBasePlayer OwnerPlayer { get; }
	}
}