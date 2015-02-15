namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public interface IPvpDeathResolver
	{
		bool TryResolve(MonoBehaviour entity, HitInfo hitinfo, out PvpDeath pvpDeath);
	}
}