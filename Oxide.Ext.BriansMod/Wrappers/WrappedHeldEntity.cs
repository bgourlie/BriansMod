namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedHeldEntity : WrappedBaseEntity, IHeldEntity
	{
		private readonly HeldEntity heldEntity;

		public WrappedHeldEntity(HeldEntity heldEntity)
			: base(heldEntity)
		{
			this.heldEntity = heldEntity;
		}

		public HoldType HoldType => this.heldEntity.holdType;

		public IBasePlayer OwnerPlayer => new WrappedBasePlayer(this.heldEntity.ownerPlayer);

		public override string ToString()
		{
			return this.heldEntity.ToString();
		}
	}
}