namespace Oxide.Ext.BriansMod.Model
{
	public class PvpDeath
	{
		public readonly Injury Injury;
		public readonly IBasePlayer Killer;
		public readonly IBasePlayer Victim;

		public PvpDeath(IBasePlayer victim, IBasePlayer killer, Injury injury)
		{
			Victim = victim;
			Killer = killer;
			Injury = injury;
		}
	}
}