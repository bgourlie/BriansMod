namespace Oxide.Ext.BriansMod.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using Contracts;
	using Model;
	using Model.Rust;
	using Model.Rust.Contracts;

	public class Players : IPlayers
	{
		private static Players _instance;
		public static Players Instance => _instance ?? (_instance = new Players());

		public bool TryFindPlayerById(ulong userId, out IBasePlayer player)
		{
			var basePlayer = BasePlayer.activePlayerList.FirstOrDefault(p => p.userID == userId)
			                 ?? BasePlayer.sleepingPlayerList.FirstOrDefault(p => p.userID == userId);

			if (basePlayer != null)
			{
				player = new WrappedBasePlayer(basePlayer);
				return true;
			}

			player = null;
			return false;
		}

		public PlayerFindResult FindPlayer(string startsWith)
		{
			var players =
				ActivePlayers.Where(p => p.DisplayName.ToLowerInvariant().StartsWith(startsWith.ToLowerInvariant())).ToArray();

			if (players.Length == 1)
			{
				return PlayerFindResult.NewFoundResult(players[0]);
			}

			if (players.Length > 1)
			{
				return PlayerFindResult.NewAmbiguousResult(players);
			}

			return PlayerFindResult.NewNotFoundResult();
		}

		public IEnumerable<IBasePlayer> ActivePlayers
			=> from player in BasePlayer.activePlayerList select (IBasePlayer) new WrappedBasePlayer(player);

		public IEnumerable<IBasePlayer> Sleepers
			=> from player in BasePlayer.activePlayerList select (IBasePlayer) new WrappedBasePlayer(player);
	}
}