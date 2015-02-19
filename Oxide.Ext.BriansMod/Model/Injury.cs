namespace Oxide.Ext.BriansMod.Model
{
	using System;

	using global::Rust;

	public class Injury
	{
		public readonly float AttackDistance;

		public readonly IMonoBehavior Attacker;

		public readonly bool CausedByTrap;

		public readonly DateTime InjuryTime;

		public readonly DamageType PrimaryDamageType;

		// Can be null
		public readonly IAttackEntity Weapon;

		public Injury(
			IMonoBehavior attacker,
			IAttackEntity weapon,
			DamageType primaryDamageType,
			bool causedByTrap,
			float attackDistance,
			DateTime injuryTime)
		{
			this.Attacker = attacker;
			this.Weapon = weapon;
			this.InjuryTime = injuryTime;
			this.PrimaryDamageType = primaryDamageType;
			this.CausedByTrap = causedByTrap;
			this.AttackDistance = attackDistance;
		}
	}
}