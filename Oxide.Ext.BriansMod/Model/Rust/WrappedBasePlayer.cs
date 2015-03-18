namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;
	using Oxide.Rust.Libraries;

	public class WrappedBasePlayer : WrappedBaseCombatEntity, IBasePlayer
	{
		private static readonly Rust RustLib = new Rust();
		private readonly BasePlayer _basePlayer;
		private IPlayerInventory _inventory;

		public WrappedBasePlayer([NotNull] BasePlayer basePlayer)
			: base(basePlayer)
		{
			_basePlayer = basePlayer;
		}

		public string DisplayName => _basePlayer.displayName;
		public ulong UserId => _basePlayer.userID;

		public void ForcePosition(float x, float y, float z)
		{
			RustLib.ForcePlayerPosition(_basePlayer, x, y, z);
		}

		public IPlayerInventory Inventory => _inventory ?? (_inventory = new WrappedPlayerInventory(_basePlayer.inventory));

		public bool IsDead()
		{
			return _basePlayer.IsDead();
		}

		public void SendConsoleCommand(string command, params object[] options)
		{
			_basePlayer.SendConsoleCommand(command, options);
		}

		public override string ToString()
		{
			return _basePlayer.ToString();
		}
	}
}