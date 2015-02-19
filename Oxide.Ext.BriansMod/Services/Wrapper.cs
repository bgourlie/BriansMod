namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Wrappers;

	using UnityEngine;

	public class Wrapper : IWrapper
	{
		private const string Module = "Wrapper";

		private static Wrapper instance;

		private readonly ILogger logger;

		public Wrapper()
			: this(Logger.Instance)
		{
		}

		public Wrapper(ILogger logger)
		{
			this.logger = logger;
		}

		public static Wrapper Instance => instance ?? (instance = new Wrapper());

		public bool TryWrap(MonoBehaviour unwrapped, out IMonoBehavior wrapped)
		{
			var baseNpc = unwrapped as BaseNPC;
			if (baseNpc != null)
			{
				wrapped = new WrappedBaseNpc(baseNpc);
				return true;
			}

			var baseCorpse = unwrapped as BaseCorpse;
			if (baseCorpse != null)
			{
				wrapped = new WrappedBaseCorpse(baseCorpse);
				return true;
			}

			var basePlayer = unwrapped as BasePlayer;
			if (basePlayer != null)
			{
				wrapped = new WrappedBasePlayer(basePlayer);
				return true;
			}

			var trap = unwrapped as BearTrap;
			if (trap != null)
			{
				wrapped = new WrappedTrap(trap);
				return true;
			}

			this.logger.Warn(Module, "Unable to wrap object {0}", unwrapped);
			wrapped = null;
			return false;
		}
	}
}