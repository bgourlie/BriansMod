namespace Oxide.Ext.BriansMod.Model
{
	public interface IHeldEntity : IBaseEntity
	{
		IBasePlayer OwnerPlayer { get; }
	}
}