﻿namespace Oxide.Ext.BriansMod.Model.Rust.Contracts
{
	using global::Rust;
	using JetBrains.Annotations;

	public interface IHitInfo
	{
		[CanBeNull]
		IBaseEntity HitEntity { get; }

		IBaseEntity Initiator { get; }
		IAttackEntity Weapon { get; }
		DamageTypeList DamageTypes { get; }
	}
}