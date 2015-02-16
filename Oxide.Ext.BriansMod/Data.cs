namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Data.SQLite;
	using System.IO;

	using Model;

	internal class Data : IData
	{
		private const string Module = "Data";

		private static Data instance;

		private readonly IConfiguration config;

		private readonly ILogger logger;

		private SQLiteConnection conn;

		public static Data Instance => instance ?? (instance = new Data());

		public Data()
			: this(Logger.Instance, Configuration.Instance)
		{
		}

		public Data(ILogger logger, IConfiguration config)
		{
			this.config = config;
			this.logger = logger;
		}

		public void InitializeStore()
		{
			this.logger.Info(Module, "Initializing stats data");
			var filename = Path.Combine(this.config.DataDirectory, "stats.db");
			if (!filename.StartsWith(this.config.DataDirectory, StringComparison.Ordinal))
			{
				throw new Exception("Only access to oxide directory!");
			}

			this.conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", filename));
			this.conn.Open();
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS players (steamid INT PRIMARY KEY, displayName TEXT NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText =
					"CREATE TABLE IF NOT EXISTS pvpdeaths (victimSteamId INT NOT NULL, killerSteamid INT NOT NULL, cause INT NOT NULL, time INTEGER NOT NULL)";
				cmd.ExecuteNonQuery();
			}
		}

		public void RecordPlayer(BasePlayer player)
		{
			this.logger.Info(Module, "Recording player: {0}", player.displayName);
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText =
					"INSERT INTO players (steamid, displayName) SELECT @steamid, @displayName WHERE NOT EXISTS(SELECT 1 FROM players WHERE steamid = @steamid)";
				cmd.Parameters.AddWithValue("@steamid", player.userID);
				cmd.Parameters.AddWithValue("@displayName", player.displayName);
				cmd.ExecuteNonQuery();
			}
		}

		public void RecordDeath(PvpDeath pvpDeath)
		{
			this.logger.Info(Module, "Recording death: {0}", pvpDeath);
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText =
					"INSERT INTO pvpdeaths (victimSteamId, killerSteamId, cause, time) VALUES (@victimSteamId, @killerSteamId, @cause, @time)";
				cmd.Parameters.AddWithValue("@victimSteamId", pvpDeath.Victim.userID);
				cmd.Parameters.AddWithValue("@killerSteamId", pvpDeath.Killer.userID);
				cmd.Parameters.AddWithValue("@cause", (int)pvpDeath.DeathCause);
				cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToUnixEpoch());
				cmd.ExecuteNonQuery();
			}
		}
	}
}