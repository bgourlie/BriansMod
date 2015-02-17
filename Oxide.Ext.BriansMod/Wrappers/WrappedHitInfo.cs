namespace Oxide.Ext.BriansMod.Wrappers
{
	using global::Rust;

	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Services;

	public class WrappedHitInfo : IHitInfo
	{
		private readonly HitInfo hitInfo;

		private IBaseEntity hitEntity;

		private IBaseEntity initiator;

		public WrappedHitInfo(HitInfo hitInfo)
		{
			this.hitInfo = hitInfo;
		}

		public IBaseEntity HitEntity
		{
			get
			{
				if (this.hitInfo.HitEntity == null)
				{
					return null;
				}

				if (this.hitEntity == null)
				{
					IMonoBehavior b;
					if (Wrapper.Instance.TryWrap(this.hitInfo.HitEntity, out b))
					{
						this.hitEntity = (IBaseEntity)b;
					}
					else
					{
						this.hitEntity = new WrappedBaseEntity(this.hitInfo.HitEntity);
					}
				}
				return this.hitEntity;
			}
		}

		public IBaseEntity Initiator
		{
			get
			{
				if (this.initiator == null)
				{
					IMonoBehavior b;
					if (Wrapper.Instance.TryWrap(this.hitInfo.Initiator, out b))
					{
						this.initiator = (IBaseEntity)b;
					}
					else
					{
						this.initiator = new WrappedBaseEntity(this.hitInfo.Initiator);
					}
				}
				return this.initiator;
			}
		}

		public IAttackEntity Weapon => new WrappedAttackEntity(this.hitInfo.Weapon);

		public DamageTypeList DamageTypes => this.hitInfo.damageTypes;

		public override string ToString()
		{
			return this.hitInfo.ToString();
		}
	}
}