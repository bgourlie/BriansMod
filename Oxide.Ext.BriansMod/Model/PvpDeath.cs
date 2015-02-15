namespace Oxide.Ext.BriansMod.Model
{
	using global::Rust;

	public class PvpDeath
	{
		public override string ToString()
		{
			return string.Format("{0} killed by {1} [{2}]", this.Victim, this.Killer, this.DeathCause);
		}

		public readonly DamageType DeathCause;

		public readonly BasePlayer Victim;

		public readonly BasePlayer Killer;

		public PvpDeath(BasePlayer victim, BasePlayer killer, DamageType deathCause)
		{
			this.DeathCause = deathCause;
			this.Victim = victim;
			this.Killer = killer;
		}
	}
}