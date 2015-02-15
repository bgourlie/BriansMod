namespace Oxide.Ext.BriansMod
{
	using Model;

	public interface IData
	{
		void InitializeStore();

		void RecordPlayer(BasePlayer player);

		void RecordDeath(PvpDeath pvpDeath);
	}
}