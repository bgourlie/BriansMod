namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Data.SQLite;
	using System.IO;

	internal class Data : IData
	{
		private const string Module = "Data";

		private static Data instance;

		private static SQLiteConnection conn;

		private readonly IConfiguration config;

		private readonly ILogger logger;

		public Data()
			: this(Logger.Instance, Configuration.Instance)
		{
		}

		public Data(ILogger logger, IConfiguration config)
		{
			this.config = config;
			this.logger = logger;
		}

		public static Data Instance => instance ?? (instance = new Data());

		public SQLiteConnection Connection => conn;

		public void InitializeStore()
		{
			this.logger.Info(Module, "Initializing stats data");
			var filename = Path.Combine(this.config.DataDirectory, "stats.db");
			if (!filename.StartsWith(this.config.DataDirectory, StringComparison.Ordinal))
			{
				throw new Exception("Only access to oxide directory!");
			}

			if (conn != null)
			{
				throw new Exception("Store already initialized.");
			}

			conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", filename));
			conn.Open();
			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandText =
					"CREATE TABLE IF NOT EXISTS pvpdeaths (victimid INT NOT NULL, killerid INT NOT NULL, cause INT NOT NULL, time INTEGER NOT NULL)";
				cmd.ExecuteNonQuery();
			}

			using (var cmd = conn.CreateCommand())
			{
				cmd.CommandText = "CREATE TABLE IF NOT EXISTS traps (id INT PRIMARY KEY, ownerid INT NOT NULL)";
				cmd.ExecuteNonQuery();
			}
		}
	}
}