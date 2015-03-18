namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using global::Rust;
	using JetBrains.Annotations;
	using Services;

	public class WrappedHitInfo : IHitInfo
	{
		private readonly HitInfo _hitInfo;
		private IBaseEntity _hitEntity;
		private IBaseEntity _initiator;

		public WrappedHitInfo([NotNull] HitInfo hitInfo)
		{
			_hitInfo = hitInfo;
		}

		public IBaseEntity HitEntity
		{
			get
			{
				if (_hitInfo.HitEntity == null)
				{
					return null;
				}

				if (_hitEntity == null)
				{
					IMonoBehavior b;
					if (Wrapper.Instance.TryWrap(_hitInfo.HitEntity, out b))
					{
						_hitEntity = (IBaseEntity) b;
					}
					else
					{
						_hitEntity = new WrappedBaseEntity(_hitInfo.HitEntity);
					}
				}
				return _hitEntity;
			}
		}

		public IBaseEntity Initiator
		{
			get
			{
				if (_initiator == null)
				{
					IMonoBehavior b;
					if (Wrapper.Instance.TryWrap(_hitInfo.Initiator, out b))
					{
						_initiator = (IBaseEntity) b;
					}
					else
					{
						_initiator = new WrappedBaseEntity(_hitInfo.Initiator);
					}
				}
				return _initiator;
			}
		}

		public IAttackEntity Weapon => new WrappedAttackEntity(_hitInfo.Weapon);
		public DamageTypeList DamageTypes => _hitInfo.damageTypes;

		public override string ToString()
		{
			return _hitInfo.ToString();
		}
	}
}