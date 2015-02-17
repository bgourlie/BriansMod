namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	using UnityEngine;

	public class WrappedBaseEntity : WrappedMonoBehavior, IBaseEntity
	{
		private readonly BaseEntity baseEntity;

		public WrappedBaseEntity(BaseEntity baseEntity)
		{
			this.baseEntity = baseEntity;
		}

		public Transform Transform => this.baseEntity.transform;

		public override string ToString()
		{
			return this.baseEntity.ToString();
		}
	}
}