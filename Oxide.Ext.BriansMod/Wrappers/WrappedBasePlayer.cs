namespace Oxide.Ext.BriansMod.Wrappers
{
	using Oxide.Ext.BriansMod.Model;

	public class WrappedBasePlayer : WrappedBaseCombatEntity, IBasePlayer
	{
		private readonly BasePlayer basePlayer;

		public WrappedBasePlayer(BasePlayer basePlayer)
			: base(basePlayer)
		{
			this.basePlayer = basePlayer;
		}

		public string DisplayName => this.basePlayer.displayName;

		public ulong UserId => this.basePlayer.userID;

		public bool IsDead()
		{
			return this.basePlayer.IsDead();
		}

		public override string ToString()
		{
			return this.basePlayer.ToString();
		}
	}
}