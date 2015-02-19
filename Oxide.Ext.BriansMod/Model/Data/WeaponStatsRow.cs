namespace Oxide.Ext.BriansMod.Model.Data
{
	public class WeaponStatsRow
	{
		public readonly float BestDistance;
		public readonly int NumKills;
		public readonly string Weapon;

		public WeaponStatsRow(string weapon, int numKills, float bestDistance)
		{
			Weapon = weapon;
			NumKills = numKills;
			BestDistance = bestDistance;
		}
	}
}