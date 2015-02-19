namespace Oxide.Ext.BriansMod.Services
{
	using Model;
	using UnityEngine;

	public interface IWrapper
	{
		bool TryWrap(MonoBehaviour unwrapped, out IMonoBehavior wrapped);
	}
}