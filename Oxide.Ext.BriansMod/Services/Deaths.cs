namespace Oxide.Ext.BriansMod.Services
{
	using System;

	using Oxide.Ext.BriansMod.Model;

	public class Deaths : IDeaths
	{
		private const string Module = "Deaths";

		private static Deaths _instance;

		private readonly IData data;

		private readonly IInjuries injuries;

		private readonly ILogger logger;

		private readonly IPlayers players;

		private readonly ITraps traps;

		public Deaths()
			: this(Logger.Instance, Players.Instance, Injuries.Instance, Data.Instance, Traps.Instance)
		{
		}

		public Deaths(ILogger logger, IPlayers players, IInjuries injuries, IData data, ITraps traps)
		{
			this.logger = logger;
			this.players = players;
			this.injuries = injuries;
			this.data = data;
			this.traps = traps;
		}

		public static Deaths Instance => _instance ?? (_instance = new Deaths());

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

				IBasePlayer killer = null;
				if (lastInjury.CausedBy is IBasePlayer)
				{
					killer = (IBasePlayer)lastInjury.CausedBy;
				}
				else if (lastInjury.CausedBy is ITrap)
				{
					var ownerId = this.traps.GetOwnerId((ITrap)lastInjury.CausedBy);
					this.players.TryFindPlayerById(ownerId, out killer);
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
				cmd.Parameters.AddWithValue("@victimid", pvpDeath.Victim.UserId);
				cmd.Parameters.AddWithValue("@killerid", pvpDeath.Killer.UserId);
				cmd.Parameters.AddWithValue("@cause", (int)pvpDeath.Injury.PrimaryDamageType);
				cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToUnixEpoch());
				cmd.ExecuteNonQuery();
			}
		}
	}
}