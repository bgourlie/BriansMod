namespace Oxide.Ext.BriansMod.Model
{
	using Oxide.Ext.BriansMod.Services;

	public class PvpDeath
	{
		public readonly Injury Injury;

		public readonly IBasePlayer Killer;

		public readonly IBasePlayer Victim;

		public PvpDeath(IBasePlayer victim, IBasePlayer killer, Injury injury)
		{
			this.Victim = victim;
			this.Killer = killer;
			this.Injury = injury;
		}

		public override string ToString()
		{
			return string.Format(
				"{0} killed by {1} with {2} from a distance of {3} meters.",
				this.Victim.DisplayName,
				this.Killer.DisplayName,
				this.Injury.Weapon?.GetDisplayName() ?? this.Injury.PrimaryDamageType.ToString(),
				this.Injury.AttackDistance);
		}
	}
}