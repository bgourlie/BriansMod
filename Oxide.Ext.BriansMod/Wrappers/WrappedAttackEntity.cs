namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

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