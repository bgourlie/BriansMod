namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Text;

	using Oxide.Ext.BriansMod.Model;

	public class Deaths : IDeaths
	{
		private const string Module = "Deaths";

		private static Deaths instance;

		private readonly IData data;

		private readonly IInjuries injuries;

		private readonly ILogger logger;

		public Deaths()
			: this(Logger.Instance, Injuries.Instance, Data.Instance)
		{
		}

		public Deaths(ILogger logger, IInjuries injuries, IData data)
		{
			this.logger = logger;
			this.injuries = injuries;
			this.data = data;
		}

		public static Deaths Instance => instance ?? (instance = new Deaths());

		public bool TryResolvePvpDeath(IMonoBehavior entity, IHitInfo hitInfo, out PvpDeath pvpDeath)
		{
			var victim = entity as IBasePlayer;
			if (victim != null)
			{
				Injury lastInjury;
				if (!this.injuries.TryGetLastRelevantInjury(victim, out lastInjury))
				{
					lastInjury = this.injuries.ResolveInjury(hitInfo);
				}

				if (lastInjury.Attacker is IBasePlayer)
				{
					var killer = (IBasePlayer)lastInjury.Attacker;
					if (!killer.Equals(victim))
					{
						pvpDeath = new PvpDeath(victim, killer, lastInjury);
						return true;
					}
				}
			}

			pvpDeath = null;
			return false;
		}

		public void Record(PvpDeath pvpDeath)
		{
			this.logger.Info(Module, "Recording death: {0}", pvpDeath);
			this.data.SaveDeath(pvpDeath.Victim.UserId, pvpDeath.Killer.UserId, DateTime.UtcNow);
		}

		public string GetDeathMessage(PvpDeath death)
		{
			var sb = new StringBuilder(
				string.Format("{0} was killed by {1}", death.Victim.DisplayName, death.Killer.DisplayName));
			if (death.Injury.CausedByTrap)
			{
				sb.Append("'s trap.");
			}
			else
			{
				sb.Append(string.Format(" with a {0}", death.Injury.Weapon.HoldType));
				sb.Append(
					death.Injury.AttackDistance < 2f
						? " at point blank range."
						: string.Format(" at a distance of {0} meters.", death.Injury.AttackDistance));
			}
			return sb.ToString();
		}
	}
}