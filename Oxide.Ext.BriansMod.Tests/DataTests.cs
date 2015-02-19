// ReSharper disable InconsistentNaming

namespace Oxide.Ext.BriansMod.Tests
{
	using System;

	using Moq;

	using Oxide.Ext.BriansMod.Services;

	using Xunit;

	public class DataTests
	{
		[Fact]
		private void save_death_should_succeed()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveDeath(1, 1, DateTime.UtcNow);
		}

		[Fact]
		private void save_trap_should_succeed()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveTrap(1, 1);
		}

		public class Factory
		{
			public readonly Mock<ILogger> LoggerMock;

			public Factory()
			{
				this.LoggerMock = new Mock<ILogger>();
			}

			public Data GetDataService()
			{
				var data = new Data(this.LoggerMock.Object);
				data.InitializeStore("Data Source=:memory:;Version=3;");
				return data;
			}
		}
	}
}