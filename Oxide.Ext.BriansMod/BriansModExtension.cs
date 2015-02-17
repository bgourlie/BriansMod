namespace Oxide.Ext.BriansMod
{
	using Oxide.Core;
	using Oxide.Core.Extensions;

	public class BriansModExtension : Extension
	{
		public BriansModExtension(ExtensionManager manager)
			: base(manager)
		{
		}

		public override string Name => "Brian's Mod";

		public override VersionNumber Version => new VersionNumber(0, 1, 0);

		public override string Author => "W. Brian Gourlie";

		public override void Load()
		{
			this.Manager.RegisterPluginLoader(new BriansModPluginLoader());
		}

		public override void LoadPluginWatchers(string plugindir)
		{
		}

		public override void OnModLoad()
		{
		}
	}
}