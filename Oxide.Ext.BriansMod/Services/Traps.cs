namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using Contracts;
	using Model.Rust.Contracts;

	public class Traps : ITraps
	{
		private const string Module = "Traps";
		// ReSharper disable once InconsistentNaming
		private static Traps _instance;

		public static Traps Instance => _instance ?? (_instance = new Traps());

		private readonly ILogger _logger;

		private readonly IData _data;

		public Traps()
			: this(Logger.Instance, Data.Instance)
		{
		}

		public Traps(ILogger logger, IData data)
		{
			_logger = logger;
			_data = data;
		}

		public void RecordTrap(IBasePlayer player, ITrap trap)
		{
			ulong trapId = GetTrapId(trap);
			_logger.Info(Module, "Saving bear trap (id = {0}, owner = {1})", trapId, player.UserId);
			_data.SaveTrap(trapId, player.UserId);
		}

		public ulong GetOwnerId(ITrap trap)
		{
			ulong trapId = GetTrapId(trap);
			return _data.GetTrapOwnerId(trapId);
		}

		public void DestroyTrap(ITrap trap)
		{
			ulong trapId = GetTrapId(trap);
			_logger.Info(Module, "Deleting bear trap (id = {0})", trapId);
			_data.SetTrapDestroyed(trapId);
		}

		// Generate a reasonably unique id based on the trap's x and y coords
		public ulong GetTrapId(ITrap trap)
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