namespace Oxide.Ext.BriansMod.Model
{
	using global::Rust;

	public interface IHitInfo
	{
		IBaseEntity HitEntity { get; }

		IBaseEntity Initiator { get; }

		IAttackEntity Weapon { get; }

		DamageTypeList DamageTypes { get; }
	}
}