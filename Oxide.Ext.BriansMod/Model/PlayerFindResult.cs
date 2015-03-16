namespace Oxide.Ext.BriansMod.Model
{
	public class PlayerFindResult
	{
		public readonly bool Found;
		public readonly IBasePlayer FoundPlayer;
		public readonly bool IsAmbiguous;
		public readonly IBasePlayer[] MatchedPlayers;

		private PlayerFindResult(bool found, bool isAmbiguous, IBasePlayer foundPlayer, IBasePlayer[] matchedPlayers)
		{
			Found = found;
			IsAmbiguous = isAmbiguous;
			FoundPlayer = foundPlayer;
			MatchedPlayers = matchedPlayers;
		}

		public static PlayerFindResult NewFoundResult(IBasePlayer player)
		{
			return new PlayerFindResult(true, false, player, null);
		}

		public static PlayerFindResult NewAmbiguousResult(IBasePlayer[] matchedPlayers)
		{
			return new PlayerFindResult(true, true, null, matchedPlayers);
		}

		public static PlayerFindResult NewNotFoundResult()
		{
			return new PlayerFindResult(true, false, null, null);
		}
	}
}