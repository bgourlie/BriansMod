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
			var trapId = this.GetTrapId(trap);
			this.logger.Info(Module, "Saving bear trap (id = {0}, owner = {1})", trapId, player.UserId);
			this.data.SaveTrap(trapId, player.UserId);
		}

		public ulong GetOwnerId(ITrap trap)
		{
			var trapId = this.GetTrapId(trap);
			return this.data.GetTrapOwnerId(trapId);
		}

		public void DestroyTrap(ITrap trap)
		{
			var trapId = this.GetTrapId(trap);
			this.logger.Info(Module, "Deleting bear trap (id = {0})", trapId);
			this.data.SetTrapDestroyed(trapId);
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