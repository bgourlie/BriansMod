namespace Oxide.Ext.BriansMod.Model
{
	using System;

	using global::Rust;

	public class Injury
	{
		public readonly DateTime InjuryTime;

		public readonly HitInfo HitInfo;

		public Injury(DateTime injuryTime, HitInfo hitInfo)
		{
			this.InjuryTime = injuryTime;
			this.HitInfo = hitInfo;
		}

		public override string ToString()
		{
			return string.Format(
				"{0} damage inflicted by {1}",
				this.HitInfo.damageTypes.GetMajorityDamageType(),
				this.HitInfo.Initiator.GetDisplayName());
		}
	}
}