namespace Oxide.Ext.BriansMod
{
	using System.Data.SQLite;

	public interface IData
	{
		void InitializeStore();

		SQLiteConnection Connection { get; }
	}
}