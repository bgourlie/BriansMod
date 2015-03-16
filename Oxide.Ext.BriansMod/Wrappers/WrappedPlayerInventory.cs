namespace Oxide.Ext.BriansMod.Wrappers
{
	using Model;

	public class WrappedPlayerInventory : IPlayerInventory
	{
		private readonly PlayerInventory _inventory;

		public WrappedPlayerInventory(PlayerInventory inventory)
		{
			_inventory = inventory;
		}

		public void GiveItem(int itemId, int amount = 1)
		{
			_inventory.GiveItem(itemId, amount);
		}
	}
}