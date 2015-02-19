namespace Oxide.Ext.BriansMod.Services
{
	using System;

	public interface IData
	{
		void InitializeStore(string connectionString);

		void SaveDeath(ulong victimId, ulong killerId, DateTime time);

		void SaveTrap(ulong trapId, ulong ownerId);

		ulong GetTrapOwnerId(ulong trapId);

		void SetTrapDestroyed(ulong trapId);
	}
}