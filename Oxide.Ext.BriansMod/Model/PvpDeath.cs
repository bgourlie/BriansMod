namespace Oxide.Ext.BriansMod.Model
{
	using System;
	using JetBrains.Annotations;
	using Rust.Contracts;

	public class PvpDeath
	{
		public readonly Injury Injury;
		public readonly IBasePlayer Killer;
		public readonly DateTime TimeOfDeath;
		public readonly IBasePlayer Victim;

		public PvpDeath([NotNull] IBasePlayer victim, [NotNull] IBasePlayer killer, [NotNull] Injury injury,
			DateTime timeOfDeath)
		{
			Victim = victim;
			Killer = killer;
			Injury = injury;
			TimeOfDeath = timeOfDeath;
		}
	}
}