namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public interface IDeathResolver
	{
		bool TryResolvePvpDeath(MonoBehaviour entity, HitInfo hitinfo, out PvpDeath pvpDeath);
	}
}