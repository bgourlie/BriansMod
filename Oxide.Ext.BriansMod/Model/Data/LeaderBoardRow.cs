namespace Oxide.Ext.BriansMod.Model.Data
{
	public class LeaderBoardRow
	{
		public readonly int Deaths;
		public readonly float KillDeathRatio;
		public readonly int Kills;
		public readonly ulong UserId;

		public LeaderBoardRow(ulong userId, int kills, int deaths, float killDeathRatio)
		{
			UserId = userId;
			Kills = kills;
			Deaths = deaths;
			KillDeathRatio = killDeathRatio;
		}
	}
}