﻿namespace Oxide.Ext.BriansMod.Services
{
	using System.Linq;
	using Model;
	using Wrappers;

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
	}
}