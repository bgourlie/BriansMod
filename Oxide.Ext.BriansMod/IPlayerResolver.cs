namespace Oxide.Ext.BriansMod
{
	using Model;

	using UnityEngine;

	public interface IPlayerResolver
	{
		bool TryResolvePlayer(MonoBehaviour entity, out BasePlayer player);
	}
}