namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Collections.Generic;
	using System.Data.SQLite;
	using Contracts;
	using Model.Data;

	public class Data : IData
	{
		private const string Module = "Data";
		private static Data _instance;
		private readonly ILogger _logger;
		private SQLiteConnection _conn;

		public Data()
			: this(Logger.Instance)
		{
		}

		public Data(ILogger logger)
		{
			_logger = logger;
		}

		public static Data Instance => _instance ?? (_instance = new Data());

		public void InitializeStore(string connectionString)
		{
			_logger.Info(Module, "Initializing stats data");
			if (_conn != null)
			{
				throw new Exception("Store already initialized.");
			}

			_conn = new SQLiteConnection(connectionString);
			_conn.Open();
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS pvpdeaths (
						victimid INT NOT NULL, 
						killerid INT NOT NULL, 
						victimlocx REAL NOT NULL,
						victimlocy REAL NOT NULL,
						time INTEGER NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS pvpweapondeaths (
						killerlocx REAL NOT NULL,
						killerlocy REAL NOT NULL,
						distance REAL NOT NULL, 
						weapon TEXT NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS pvptrapdeaths (
						trapid INT NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"CREATE TABLE IF NOT EXISTS traps (
						id INT PRIMARY KEY, 
						ownerid INT NOT NULL, 
						destroyed INT NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"DROP VIEW IF EXISTS weaponstats;
					  CREATE VIEW weaponstats AS
						SELECT *
						FROM
						  (SELECT
							 killerid AS bestdistanceuser,
							 bestdistance,
							 numkills,
							 weapon
						   FROM pvpweapondeaths w1
							 JOIN pvpdeaths pvp ON w1.rowid = pvp.rowid
							 INNER JOIN
							 (
							   SELECT
								 MAX(distance) AS bestdistance,
								 COUNT(*)      AS numkills
							   FROM pvpweapondeaths
							   GROUP BY weapon
							 ) w2 ON w1.distance = w2.bestdistance
						   GROUP BY weapon
						   UNION SELECT
								   0        AS bestdistanceuser,
								   0        AS bestdistance,
								   COUNT(*) AS numkills,
								   'trap'   AS weapon
								 FROM pvptrapdeaths
								 GROUP BY weapon
								 HAVING COUNT(*) > 0)
						ORDER BY numkills
						  DESC;";

				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"  DROP VIEW IF EXISTS weaponstatsbyuser; 
					    CREATE VIEW weaponstatsbyuser AS
						SELECT *
						FROM
						  (SELECT
							 killerid      AS userid,
							 MAX(distance) AS bestdistance,
							 COUNT(*)      AS numkills,
							 weapon
						   FROM pvpweapondeaths pw
							 JOIN pvpdeaths p ON pw.rowid = p.rowid
						   GROUP BY userid, weapon
						   UNION SELECT
								   killerid AS userid,
								   0        AS bestdistance,
								   COUNT(*) AS numkills,
								   'trap'   AS weapon
								 FROM pvptrapdeaths pt
								   JOIN pvpdeaths p ON pt.rowid = p.rowid
								 GROUP BY userid, weapon
								 HAVING COUNT(*) > 0)
						ORDER BY numkills
						  DESC";

				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"	DROP VIEW IF EXISTS leaderboard;
						CREATE VIEW leaderboard AS
						  SELECT
							killerid                AS userid,
							count(*)                AS kills,
							deaths,
							count(*) * 1.0 / deaths AS kdr
						  FROM pvpdeaths p1
							INNER JOIN
							(
							  SELECT
								victimid AS userid,
								count(*) AS deaths
							  FROM pvpdeaths
							  GROUP BY victimid
							) p2 ON p1.killerid = p2.userid
						  GROUP BY userid
						  ORDER BY kills
							DESC;";

				cmd.ExecuteNonQuery();
			}
		}

		public void SaveWeaponDeath(ulong victimId, ulong killerId, float victimLocationX, float victimLocationY,
			float killerLocationX, float killerLocationY, string weapon, float distance, DateTime time)
		{
			using (var trans = _conn.BeginTransaction())
			{
				using (var cmd = _conn.CreateCommand())
				{
					cmd.Transaction = trans;
					ulong pvpDeathId = SavePvpDeath(victimId, killerId, victimLocationX, victimLocationY, time, cmd);

					cmd.CommandText =
						@"INSERT INTO pvpweapondeaths 
							(rowid, killerlocx, killerlocy, weapon, distance) VALUES 
							(@rowid, @killerlocx, @killerlocy, @weapon, @distance)";
					cmd.Parameters.AddWithValue("@rowid", pvpDeathId);
					cmd.Parameters.AddWithValue("@killerlocx", killerLocationX);
					cmd.Parameters.AddWithValue("@killerlocy", killerLocationY);
					cmd.Parameters.AddWithValue("@weapon", weapon);
					cmd.Parameters.AddWithValue("@distance", distance);
					cmd.ExecuteNonQuery();
					trans.Commit();
				}
			}
		}

		public void SaveTrapDeath(ulong victimId, ulong killerId, float victimLocationX, float victimLocationY, ulong trapId,
			DateTime time)
		{
			using (var trans = _conn.BeginTransaction())
			{
				using (var cmd = _conn.CreateCommand())
				{
					cmd.Transaction = trans;
					ulong pvpDeathId = SavePvpDeath(victimId, killerId, victimLocationX, victimLocationY, time, cmd);

					cmd.CommandText =
						@"INSERT INTO pvptrapdeaths 
							(rowid, trapid) VALUES 
							(@rowid, @trapid)";
					cmd.Parameters.AddWithValue("@rowid", pvpDeathId);
					cmd.Parameters.AddWithValue("@trapid", trapId);
					cmd.ExecuteNonQuery();
					trans.Commit();
				}
			}
		}

		public void SaveTrap(ulong trapId, ulong ownerId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					@"INSERT INTO traps 
						(id, ownerid, destroyed) VALUES 
						(@id, @ownerid, 0);";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.Parameters.AddWithValue("@ownerid", ownerId);
				cmd.ExecuteNonQuery();
			}
		}

		public ulong GetTrapOwnerId(ulong trapId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = @"SELECT ownerid FROM traps WHERE id = @id";
				cmd.Parameters.AddWithValue("@id", trapId);
				using (var reader = cmd.ExecuteReader())
				{
					reader.Read();
					long ownerId = reader.GetInt64(0);
					_logger.Info(Module, "owner id = {0}", ownerId);
					return (ulong) ownerId;
				}
			}
		}

		public void SetTrapDestroyed(ulong trapId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "UPDATE traps set destroyed = 1 WHERE id = @id";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.ExecuteNonQuery();
			}
		}

		public IEnumerable<WeaponStatsRow> GetWeaponStats()
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "SELECT bestdistance, numkills, weapon, bestdistanceuser FROM weaponstats";
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						float maxDistance = reader.GetFloat(0);
						int numKills = reader.GetInt32(1);
						string weapon = reader.GetString(2);
						// SQLiteDataReader doesn't have a GetUInt64... so GetInt64 and casting.
						// This could def cause issues...
						var bestDistanceUser = (ulong) reader.GetInt64(3);
						yield return new WeaponStatsRow(weapon, numKills, maxDistance, bestDistanceUser);
					}
				}
			}
		}

		public IEnumerable<LeaderBoardRow> GetLeaderBoard()
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "SELECT userid, kills, deaths, kdr FROM leaderboard";
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						// SQLiteDataReader doesn't have a GetUInt64... so GetInt64 and casting.
						// This could def cause issues...
						var userId = (ulong) reader.GetInt64(0);
						int numKills = reader.GetInt32(1);
						int numDeaths = reader.GetInt32(2);
						float killDeathRatio = reader.GetFloat(3);
						yield return new LeaderBoardRow(userId, numKills, numDeaths, killDeathRatio);
					}
				}
			}
		}

		public IEnumerable<WeaponStatsRow> GetWeaponStatsByUser(ulong userId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "SELECT bestdistance, numkills, weapon FROM weaponstatsbyuser WHERE userid = @userId";
				cmd.Parameters.AddWithValue("@userId", userId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						float maxDistance = reader.GetFloat(0);
						int numKills = reader.GetInt32(1);
						string weapon = reader.GetString(2);
						yield return new WeaponStatsRow(weapon, numKills, maxDistance, userId);
					}
				}
			}
		}

		private ulong SavePvpDeath(ulong victimId, ulong killerId, float victimLocationX, float victimLocationY, DateTime time,
			SQLiteCommand cmd)
		{
			cmd.CommandText =
				@"INSERT INTO pvpdeaths 
					(victimid, killerid, victimlocx, victimlocy, time) VALUES 
					(@victimid, @killerid, @victimlocx, @victimlocy, @time);
				SELECT last_insert_rowid()";

			cmd.Parameters.AddWithValue("@victimid", victimId);
			cmd.Parameters.AddWithValue("@killerid", killerId);
			cmd.Parameters.AddWithValue("@victimlocx", victimLocationX);
			cmd.Parameters.AddWithValue("@victimlocy", victimLocationY);
			cmd.Parameters.AddWithValue("@time", time.ToUnixEpoch());
			using (var reader = cmd.ExecuteReader())
			{
				reader.Read();
				return (ulong) reader.GetInt64(0);
			}
		}
	}
}