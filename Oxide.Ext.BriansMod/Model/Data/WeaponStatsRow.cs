namespace Oxide.Ext.BriansMod.Model.Data
{
	public class WeaponStatsRow
	{
		public readonly float BestDistance;
		public readonly ulong BestDistanceUser;
		public readonly int NumKills;
		public readonly string Weapon;

		public WeaponStatsRow(string weapon, int numKills, float bestDistance, ulong bestDistanceUser)
		{
			Weapon = weapon;
			NumKills = numKills;
			BestDistance = bestDistance;
			BestDistanceUser = bestDistanceUser;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1} kills, longest {2} ({3})", Weapon, NumKills, BestDistance, BestDistanceUser);
		}
	}
}