namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Text;
	using Model;

	public class Deaths : IDeaths
	{
		private const string Module = "Deaths";
		private static Deaths _instance;
		private readonly IData _data;
		private readonly IInjuries _injuries;
		private readonly ILogger _logger;

		public Deaths()
			: this(Logger.Instance, Injuries.Instance, Data.Instance)
		{
		}

		public Deaths(ILogger logger, IInjuries injuries, IData data)
		{
			_logger = logger;
			_injuries = injuries;
			_data = data;
		}

		public static Deaths Instance => _instance ?? (_instance = new Deaths());

		public bool TryResolvePvpDeath(IMonoBehavior entity, IHitInfo hitInfo, out PvpDeath pvpDeath)
		{
			var victim = entity as IBasePlayer;
			if (victim != null)
			{
				Injury lastInjury;
				if (!_injuries.TryGetLastRelevantInjury(victim, out lastInjury))
				{
					lastInjury = _injuries.ResolveInjury(hitInfo);
				}

				var player = lastInjury.Attacker as IBasePlayer;
				if (player != null)
				{
					var killer = player;
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
			_logger.Info(Module, "Recording death: {0}", pvpDeath);
			_data.SaveDeath(pvpDeath.Victim.UserId, pvpDeath.Killer.UserId, pvpDeath.Injury.Weapon?.HoldType.ToString(),
				pvpDeath.Injury.TrapId, pvpDeath.Injury.AttackDistance, DateTime.UtcNow);
		}

		public string GetDeathMessage(PvpDeath death)
		{
			var sb = new StringBuilder(
				string.Format("{0} was killed by {1}", death.Victim.DisplayName, death.Killer.DisplayName));
			if (death.Injury.TrapId.HasValue)
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