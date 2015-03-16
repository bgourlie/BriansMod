namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Model.Data;

	public interface IData
	{
		void InitializeStore([NotNull] string connectionString);

		void SaveWeaponDeath(ulong victimId, ulong killerId, float victimLocationX, float victimLocationY,
			float killerLocationX, float killerLocationY, string weapon, float distance, DateTime time);

		void SaveTrapDeath(ulong victimId, ulong killerId, float victimLocationX, float victimLocationY, ulong trapId,
			DateTime time);

		IEnumerable<WeaponStatsRow> GetWeaponStatsByUser(ulong userId);
		void SaveTrap(ulong trapId, ulong ownerId);
		ulong GetTrapOwnerId(ulong trapId);
		void SetTrapDestroyed(ulong trapId);
		IEnumerable<WeaponStatsRow> GetWeaponStats();
	}
}