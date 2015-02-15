namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public class PlayerResolver : IPlayerResolver
	{
		private static PlayerResolver _instance;

		public static PlayerResolver Instance => _instance ?? (_instance = new PlayerResolver());

		private readonly ILogger logger;

		public PlayerResolver()
			: this(Logger.Instance)
		{
		}

		public PlayerResolver(ILogger logger)
		{
			this.logger = logger;
		}

		public bool TryResolvePlayer(MonoBehaviour entity, out BasePlayer player)
		{
			player = entity as BasePlayer;
			return player != null;
		}
	}
}