namespace Oxide.Ext.BriansMod.Model
{
	public interface IBasePlayer : IBaseCombatEntity
	{
		string DisplayName { get; }
		ulong UserId { get; }
		bool IsDead();
	}
}