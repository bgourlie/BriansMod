namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;

	public class WrappedAttackEntity : WrappedHeldEntity, IAttackEntity
	{
		private readonly AttackEntity _attackEntity;

		public WrappedAttackEntity(AttackEntity attackEntity)
			: base(attackEntity)
		{
			_attackEntity = attackEntity;
		}

		public override string ToString()
		{
			return _attackEntity.ToString();
		}
	}
}