namespace Oxide.Ext.BriansMod.Services
{
	using System;

	using Oxide.Ext.BriansMod.Model;

	public class Traps : ITraps
	{
		private const string Module = "Traps";

		// ReSharper disable once InconsistentNaming
		private static Traps _instance;

		public static Traps Instance => _instance ?? (_instance = new Traps());

		private readonly ILogger logger;

		private readonly IData data;

		public Traps()
			: this(Logger.Instance, Data.Instance)
		{
		}

		public Traps(ILogger logger, IData data)
		{
			this.logger = logger;
			this.data = data;
		}

		public void RecordTrap(IBasePlayer player, ITrap trap)
		{
			using (var cmd = this.data.Connection.CreateCommand())
			{
				var trapId = this.GetTrapId(trap);
				this.logger.Info(Module, "Saving bear trap (id = {0}, owner = {1})", trapId, player.UserId);
				cmd.CommandText = "INSERT INTO traps (id, ownerid) VALUES (@id, @ownerid);";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.Parameters.AddWithValue("@ownerid", player.UserId);
				cmd.ExecuteNonQuery();
			}
		}

		public void DeleteTrap(ITrap trap)
		{
			using (var cmd = this.data.Connection.CreateCommand())
			{
				var trapId = this.GetTrapId(trap);
				this.logger.Info(Module, "Deleting bear trap (id = {0})", trapId);
				cmd.CommandText = "DELETE FROM traps WHERE id = @id";
				cmd.Parameters.AddWithValue("@id", trapId);
				cmd.ExecuteNonQuery();
			}
		}

		public ulong GetOwnerId(ITrap trap)
		{
			using (var cmd = this.data.Connection.CreateCommand())
			{
				var trapId = this.GetTrapId(trap);
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

		// Generate a reasonably unique id based on the trap's x and y coords
		private ulong GetTrapId(ITrap trap)
		{
			var leftBytes = BitConverter.GetBytes(trap.Transform.position.x);
			var rightBytes = BitConverter.GetBytes(trap.Transform.position.y);
			var idBytes = new byte[8];
			Array.Copy(leftBytes, idBytes, 4);
			Array.Copy(rightBytes, 0, idBytes, 4, 4);
			return BitConverter.ToUInt64(idBytes, 0);
		}
	}
}