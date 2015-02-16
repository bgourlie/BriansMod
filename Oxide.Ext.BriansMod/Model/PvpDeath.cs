﻿namespace Oxide.Ext.BriansMod.Model
{
	public class PvpDeath
	{
		public override string ToString()
		{
			return string.Format("{0} killed by {1} with {2} from a distance of {3}", this.Victim, this.Killer, this.Injury.PrimaryDamageType, this.Injury.AttackDistance);
		}

		public readonly BasePlayer Victim;

		public readonly BasePlayer Killer;

		public readonly Injury Injury;

		public PvpDeath(BasePlayer victim, BasePlayer killer, Injury injury)
		{
			this.Victim = victim;
			this.Killer = killer;
			this.Injury = injury;
		}
	}
}