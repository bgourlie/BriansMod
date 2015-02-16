namespace Oxide.Ext.BriansMod.Model
{
	using System;

	using global::Rust;

	using UnityEngine;

	public class Injury
	{
		public readonly MonoBehaviour CausedBy;

		public readonly DateTime InjuryTime;

		public readonly DamageType PrimaryDamageType;

		public readonly float AttackDistance;

		public Injury(MonoBehaviour causedBy, DamageType primaryDamageType, float attackDistance, DateTime injuryTime)
		{
			this.CausedBy = causedBy;
			this.InjuryTime = injuryTime;
			this.PrimaryDamageType = primaryDamageType;
			this.AttackDistance = attackDistance;
		}

		public override string ToString()
		{
			return string.Format(
				"{0} damage inflicted by {1} from a distance of {2}",
				this.PrimaryDamageType,
				this.CausedBy.GetDisplayName(),
				this.AttackDistance);
		}
	}
}