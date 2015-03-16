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
				var lastInjury = _injuries.ResolveInjury(hitInfo);
				var killer = lastInjury.Attacker as IBasePlayer;

				// trivial case: 
				// The death occurred as the direct result of a player attacking another player
				if (killer != null && !Equals(lastInjury.Attacker, victim))
				{
					pvpDeath = new PvpDeath(victim, killer, lastInjury, DateTime.UtcNow);
					return true;
				}

				// Non-trivial case:
				// The death may have been caused indirectly (bled out) as the result of another player
				// attacking a player, limited to a timeframe of one minute
				if (!_injuries.TryGetLastRelevantInjury(victim, TimeSpan.FromMinutes(1), out lastInjury))
				{
					pvpDeath = null;
					return false;
				}

				killer = lastInjury.Attacker as IBasePlayer;
				// Was the attacker a player?
				if (killer != null)
				{
					// Make sure it wasn't self inflicted...
					if (!killer.Equals(victim))
					{
						pvpDeath = new PvpDeath(victim, killer, lastInjury, DateTime.UtcNow);
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
			if (pvpDeath.Injury.TrapId.HasValue)
			{
				_data.SaveTrapDeath(pvpDeath.Victim.UserId, pvpDeath.Killer.UserId, pvpDeath.Victim.Transform.position.x,
					pvpDeath.Victim.Transform.position.y, pvpDeath.Injury.TrapId.Value, pvpDeath.TimeOfDeath);
			}
			else
			{
				_data.SaveWeaponDeath(pvpDeath.Victim.UserId, pvpDeath.Killer.UserId, pvpDeath.Victim.Transform.position.x,
					pvpDeath.Victim.Transform.position.y, pvpDeath.Killer.Transform.position.x, pvpDeath.Killer.Transform.position.y,
					pvpDeath.Injury.Weapon?.HoldType.ToString(), pvpDeath.Injury.AttackDistance, pvpDeath.TimeOfDeath);
			}
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