namespace Oxide.Ext.BriansMod
{
	using System.Collections.Generic;
	using Core.Plugins;

	public class BriansModPluginLoader : PluginLoader
	{
		public override Plugin Load(string directory, string name)
		{
			switch (name)
			{
				case "briansmod":
					return new BriansModPluginGlue();
				default:
					return null;
			}
		}

		public override IEnumerable<string> ScanDirectory(string directory)
		{
			return new[] {"briansmod"};
		}
	}
}