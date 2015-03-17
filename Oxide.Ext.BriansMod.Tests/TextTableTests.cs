namespace Oxide.Ext.BriansMod.Tests
{
	using Xunit;

	public class TextTableTests
	{
		[Fact]
		public void text_table_smoke_test()
		{
			var textTable = new TextTable("Weapon", "Num Kills", "Best Distance (meters)", "Distance Record Holder");
			textTable.AddRow("AR", 5, 6.32431, "bgzee");
			textTable.AddRow("Bow", 15, 50.2343, "brady");
			textTable.AddRow("Bolt", 4, 90.2343, "tbs");
			var output = textTable.ToString();
		}
	}
}
