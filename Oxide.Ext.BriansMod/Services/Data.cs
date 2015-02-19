namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Data.SQLite;

	public class Data : IData
	{
		private const string Module = "Data";

		private static Data instance;

		private readonly ILogger logger;

		private SQLiteConnection conn;

		public Data()
			: this(Logger.Instance)
		{
		}

		public Data(ILogger logger)
		{
			this.logger = logger;
		}

		public static Data Instance => instance ?? (instance = new Data());

		public void InitializeStore(string connectionString)
		{
			this.logger.Info(Module, "Initializing stats data");
			if (this.conn != null)
			{
				throw new Exception("Store already initialized.");
			}

			this.conn = new SQLiteConnection(connectionString);
			this.conn.Open();
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText =
					"CREATE TABLE IF NOT EXISTS pvpdeaths (victimid INT NOT NULL, killerid INT NOT NULL, trapid INT, time INTEGER NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText =
					"CREATE TABLE IF NOT EXISTS traps (id INT PRIMARY KEY, ownerid INT NOT NULL, destroyed INT NOT NULL)";
				cmd.ExecuteNonQuery();
			}
		}

		public void SaveDeath(ulong victimId, ulong killerId, DateTime time)
		{
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO pvpdeaths (victimid, killerid, time) VALUES (@victimid, @killerid, @time)";
				cmd.Parameters.AddWithValue("@victimid", victimId);
				cmd.Parameters.AddWithValue("@killerid", killerId);
				cmd.Parameters.AddWithValue("@time", DateTime.UtcNow.ToUnixEpoch());
				cmd.ExecuteNonQuery();
			}
		}

		public void SaveTrap(ulong trapId, ulong ownerId)
		{
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO traps (id, ownerid, destroyed) VALUES (@id, @ownerid, 0);";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.Parameters.AddWithValue("@ownerid", ownerId);
				cmd.ExecuteNonQuery();
			}
		}

		public ulong GetTrapOwnerId(ulong trapId)
		{
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText = "SELECT ownerid FROM traps WHERE id = @id";
				cmd.Parameters.AddWithValue("@id", trapId);
				using (var reader = cmd.ExecuteReader())
				{
					reader.Read();
					var ownerId = reader.GetInt64(0);
					this.logger.Info(Module, "owner id = {0}", ownerId);
					return (ulong)ownerId;
				}
			}
		}

		public void SetTrapDestroyed(ulong trapId)
		{
			using (var cmd = this.conn.CreateCommand())
			{
				cmd.CommandText = "UPDATE traps set destroyed = 1 WHERE id = @id";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.ExecuteNonQuery();
			}
		}
	}
}