namespace Oxide.Ext.BriansMod.Model
{
	public interface IBasePlayer : IBaseCombatEntity
	{
		string DisplayName { get; }
		ulong UserId { get; }
		IPlayerInventory Inventory { get; }
		bool IsDead();
		void SendConsoleCommand(string command, params object[] options);
		void ForcePosition(float x, float y, float z);
	}
}