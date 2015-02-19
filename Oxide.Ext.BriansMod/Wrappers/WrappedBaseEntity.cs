namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;
	using UnityEngine;

	public class WrappedBaseEntity : WrappedMonoBehavior, IBaseEntity
	{
		private readonly BaseEntity _baseEntity;

		public WrappedBaseEntity(BaseEntity baseEntity) : base(baseEntity)
		{
			_baseEntity = baseEntity;
		}

		public Transform Transform => _baseEntity.transform;

		public override string ToString()
		{
			return _baseEntity.ToString();
		}
	}
}