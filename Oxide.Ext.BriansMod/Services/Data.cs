namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Data.SQLite;

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
					"CREATE TABLE IF NOT EXISTS pvpdeaths (victimid INT NOT NULL, killerid INT NOT NULL, weapon TEXT, trapid INT, distance REAL NOT NULL, time INTEGER NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					"CREATE TABLE IF NOT EXISTS traps (id INT PRIMARY KEY, ownerid INT NOT NULL, destroyed INT NOT NULL)";
				cmd.ExecuteNonQuery();
			}
		}

		public void SaveDeath(ulong victimId, ulong killerId, string weapon, ulong? trapId, float distance, DateTime time)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText =
					"INSERT INTO pvpdeaths (victimid, killerid, weapon, trapid, distance, time) VALUES (@victimid, @killerid, @weapon, @trapid, @distance, @time)";
				cmd.Parameters.AddWithValue("@victimid", victimId);
				cmd.Parameters.AddWithValue("@killerid", killerId);
				cmd.Parameters.AddWithValue("@weapon", weapon);
				cmd.Parameters.AddWithValue("@trapid", trapId);
				cmd.Parameters.AddWithValue("@distance", distance);
				cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToUnixEpoch());
				cmd.ExecuteNonQuery();
			}
		}

		public void SaveTrap(ulong trapId, ulong ownerId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO traps (id, ownerid, destroyed) VALUES (@id, @ownerid, 0);";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.Parameters.AddWithValue("@ownerid", ownerId);
				cmd.ExecuteNonQuery();
			}
		}

		public ulong GetTrapOwnerId(ulong trapId)
		{
			using (var cmd = _conn.CreateCommand())
			{
				cmd.CommandText = "SELECT ownerid FROM traps WHERE id = @id";
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
	}
}