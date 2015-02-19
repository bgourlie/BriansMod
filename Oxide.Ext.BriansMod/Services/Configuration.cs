namespace Oxide.Ext.BriansMod.Services
{
	using Core;

	public class Configuration : IConfiguration
	{
		private static readonly string DataDir = Interface.GetMod().DataDirectory;
		private static Configuration _instance;
		public static Configuration Instance => _instance ?? (_instance = new Configuration());
		public string DataDirectory => DataDir;
	}
}