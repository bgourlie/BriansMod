namespace Oxide.Ext.BriansMod.Model.Data
{
	public class PvpDeathRow
	{
		public readonly float Distance;
		public readonly ulong KillerId;
		public readonly ulong RowId;
		public readonly int Time;
		public readonly ulong? TrapId;
		public readonly ulong VictimId;
		public readonly string Weapon;

		public PvpDeathRow(ulong rowId, ulong victimId, ulong killerId, string weapon, ulong? trapId, float distance, int time)
		{
			RowId = rowId;
			VictimId = victimId;
			KillerId = killerId;
			Weapon = weapon;
			TrapId = trapId;
			Distance = distance;
			Time = time;
		}
	}
}