namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public interface IDeaths
	{
		bool TryResolvePvpDeath(MonoBehaviour entity, HitInfo hitInfo, out PvpDeath pvpDeath);

		void Record(PvpDeath pvpDeath);
	}
}