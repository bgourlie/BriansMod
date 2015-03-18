namespace Oxide.Ext.BriansMod.Model
{
	using System;
	using global::Rust;
	using JetBrains.Annotations;
	using Rust.Contracts;

	public class Injury
	{
		public readonly float AttackDistance;
		public readonly IMonoBehavior Attacker;
		public readonly DateTime InjuryTime;
		public readonly DamageType PrimaryDamageType;
		public readonly ulong? TrapId;
		public readonly IAttackEntity Weapon;

		public Injury(
			[NotNull] IMonoBehavior attacker,
			IAttackEntity weapon,
			DamageType primaryDamageType,
			ulong? trapId,
			float attackDistance,
			DateTime injuryTime)
		{
			Attacker = attacker;
			Weapon = weapon;
			InjuryTime = injuryTime;
			PrimaryDamageType = primaryDamageType;
			TrapId = trapId;
			AttackDistance = attackDistance;
		}
	}
}