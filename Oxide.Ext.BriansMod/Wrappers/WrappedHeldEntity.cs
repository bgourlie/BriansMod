namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedHeldEntity : WrappedBaseEntity, IHeldEntity
	{
		private readonly HeldEntity _heldEntity;

		public WrappedHeldEntity(HeldEntity heldEntity)
			: base(heldEntity)
		{
			_heldEntity = heldEntity;
		}

		public HoldType HoldType => _heldEntity.holdType;
		public IBasePlayer OwnerPlayer => new WrappedBasePlayer(_heldEntity.ownerPlayer);

		public override string ToString()
		{
			return _heldEntity.ToString();
		}
	}
}