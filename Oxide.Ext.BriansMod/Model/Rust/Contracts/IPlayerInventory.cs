namespace Oxide.Ext.BriansMod.Model.Rust.Contracts
{
	public interface IPlayerInventory
	{
		void GiveItem(int itemId, int amount = 1);
	}
}