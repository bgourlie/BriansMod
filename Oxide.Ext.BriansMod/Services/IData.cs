namespace Oxide.Ext.BriansMod.Services
{
	using System.Data.SQLite;

	public interface IData
	{
		SQLiteConnection Connection { get; }

		void InitializeStore();
	}
}