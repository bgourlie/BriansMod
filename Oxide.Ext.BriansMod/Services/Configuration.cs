namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Core;

	public class Configuration : IConfiguration
	{
		private static readonly string DataDir = Interface.GetMod().DataDirectory;

		private static Configuration instance;

		public static Configuration Instance => instance ?? (instance = new Configuration());

		public string DataDirectory => DataDir;
	}
}