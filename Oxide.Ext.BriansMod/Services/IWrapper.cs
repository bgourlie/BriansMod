namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;

	using UnityEngine;

	public interface IWrapper
	{
		bool TryWrap(MonoBehaviour unwrapped, out IMonoBehavior wrapped);
	}
}