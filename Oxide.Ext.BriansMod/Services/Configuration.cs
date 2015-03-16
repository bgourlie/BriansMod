namespace Oxide.Ext.BriansMod.Services
{
	using System.IO;
	using Core;

	public class Configuration : IConfiguration
	{
		private static readonly string DbLocation = Path.Combine(Interface.GetMod().DataDirectory, "stats.db");
		private static Configuration _instance;
		public static Configuration Instance => _instance ?? (_instance = new Configuration());
		public string DatabaseLocation => DbLocation;
	}
}