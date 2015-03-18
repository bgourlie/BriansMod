namespace Oxide.Ext.BriansMod.Model.Rust.Contracts
{
	using UnityEngine;

	public interface IBaseEntity : IMonoBehavior
	{
		Transform Transform { get; }
	}
}