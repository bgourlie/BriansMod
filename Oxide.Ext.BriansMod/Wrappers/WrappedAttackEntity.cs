namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedAttackEntity : WrappedHeldEntity, IAttackEntity
	{
		private readonly AttackEntity attackEntity;

		public WrappedAttackEntity(AttackEntity attackEntity)
			: base(attackEntity)
		{
			this.attackEntity = attackEntity;
		}

		public override string ToString()
		{
			return this.attackEntity.ToString();
		}
	}
}