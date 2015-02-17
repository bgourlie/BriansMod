namespace Oxide.Ext.BriansMod.Model
{
	using UnityEngine;

	public interface IBaseEntity : IMonoBehavior
	{
		Transform Transform { get; }
	}
}