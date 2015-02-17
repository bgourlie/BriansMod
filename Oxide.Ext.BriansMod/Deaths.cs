namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Linq;

	using Model;

	using UnityEngine;

	public class Deaths : IDeaths
	{
		private static Deaths _instance;

		private const string Module = "Deaths";

		private readonly ILogger logger;

		private readonly IData data;

		private readonly ITraps traps;

		private readonly IInjuries injuries;

		public static Deaths Instance => _instance ?? (_instance = new Deaths());

		public Deaths()
			: this(Logger.Instance, Injuries.Instance, Data.Instance, Traps.Instance)
		{
		}

		public Deaths(ILogger logger, IInjuries injuries, IData data, ITraps traps)
		{
			this.logger = logger;
			this.injuries = injuries;
			this.data = data;
			this.traps = traps;
		}

		public bool TryResolvePvpDeath(MonoBehaviour entity, HitInfo hitInfo, out PvpDeath pvpDeath)
		{
			var victim = entity as BasePlayer;
			if (victim != null)
			{
				Injury lastInjury;
				if (!this.injuries.TryGetLastRelevantInjury(victim, out lastInjury))
				{
					lastInjury = this.injuries.ResolveInjury(hitInfo);
				}

				BasePlayer killer = null;
				if (lastInjury.CausedBy is BasePlayer)
				{
					killer = (BasePlayer)lastInjury.CausedBy;
				}
				else if (lastInjury.CausedBy is BearTrap)
				{
					this.logger.Info(Module, "");
					var ownerId = this.traps.GetOwnerId((BearTrap)lastInjury.CausedBy);
					killer = BasePlayer.activePlayerList.FirstOrDefault(p => p.userID == ownerId)
					         ?? BasePlayer.sleepingPlayerList.FirstOrDefault(p => p.userID == ownerId);

					// TODO: add kill to trap
				}

				if (killer != null)
				{
					if (!killer.Equals(victim))
					{
						pvpDeath = new PvpDeath(victim, killer, lastInjury);
						return true;
					}
				}
			}

			failed:
			pvpDeath = null;
			return false;
		}

		public void Record(PvpDeath pvpDeath)
		{
			this.logger.Info(Module, "Recording death: {0}", pvpDeath);
			using (var cmd = this.data.Connection.CreateCommand())
			{
				cmd.CommandText =
					"INSERT INTO pvpdeaths (victimid, killerid, cause, time) VALUES (@victimid, @killerid, @cause, @time)";
				cmd.Parameters.AddWithValue("@victimid", pvpDeath.Victim.userID);
				cmd.Parameters.AddWithValue("@killerid", pvpDeath.Killer.userID);
				cmd.Parameters.AddWithValue("@cause", (int)pvpDeath.Injury.PrimaryDamageType);
				cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToUnixEpoch());
				cmd.ExecuteNonQuery();
			}
		}
	}
}