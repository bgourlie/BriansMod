namespace Oxide.Ext.BriansMod.Model
{
	using System;

	using global::Rust;

	using Oxide.Ext.BriansMod.Services;

	public class Injury
	{
		public readonly float AttackDistance;

		public readonly IMonoBehavior CausedBy;

		public readonly DateTime InjuryTime;

		public readonly DamageType PrimaryDamageType;

		public readonly IAttackEntity Weapon;

		public Injury(
			IMonoBehavior causedBy,
			IAttackEntity weapon,
			DamageType primaryDamageType,
			float attackDistance,
			DateTime injuryTime)
		{
			this.CausedBy = causedBy;
			this.Weapon = weapon;
			this.InjuryTime = injuryTime;
			this.PrimaryDamageType = primaryDamageType;
			this.AttackDistance = attackDistance;
		}

		public override string ToString()
		{
			return string.Format(
				"{0} damage inflicted by {1} from a distance of {2}",
				this.PrimaryDamageType,
				this.Weapon?.GetDisplayName() ?? this.CausedBy.GetDisplayName(),
				this.AttackDistance);
		}
	}
}