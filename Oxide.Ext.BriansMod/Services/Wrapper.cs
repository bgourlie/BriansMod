namespace Oxide.Ext.BriansMod.Services
{
	using Contracts;
	using Model.Rust;
	using Model.Rust.Contracts;
	using UnityEngine;

	public class Wrapper : IWrapper
	{
		private const string Module = "Wrapper";
		private static Wrapper _instance;
		private readonly ILogger _logger;

		public Wrapper()
			: this(Logger.Instance)
		{
		}

		public Wrapper(ILogger logger)
		{
			_logger = logger;
		}

		public static Wrapper Instance => _instance ?? (_instance = new Wrapper());

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

			_logger.Warn(Module, "Unable to wrap object {0}", unwrapped);
			wrapped = null;
			return false;
		}
	}
}