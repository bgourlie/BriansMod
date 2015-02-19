namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedBasePlayer : WrappedBaseCombatEntity, IBasePlayer
	{
		private readonly BasePlayer _basePlayer;

		public WrappedBasePlayer(BasePlayer basePlayer)
			: base(basePlayer)
		{
			_basePlayer = basePlayer;
		}

		public string DisplayName => _basePlayer.displayName;
		public ulong UserId => _basePlayer.userID;

		public bool IsDead()
		{
			return _basePlayer.IsDead();
		}

		public override string ToString()
		{
			return _basePlayer.ToString();
		}
	}
}