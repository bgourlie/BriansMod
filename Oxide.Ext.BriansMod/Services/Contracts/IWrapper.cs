namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using Model.Rust.Contracts;
	using UnityEngine;

	public interface IWrapper
	{
		bool TryWrap(MonoBehaviour unwrapped, out IMonoBehavior wrapped);
	}
}