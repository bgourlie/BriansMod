// ReSharper disable InconsistentNaming

namespace Oxide.Ext.BriansMod.Tests
{
	using System;
	using System.Linq;
	using Moq;
	using Services;
	using Xunit;

	public class DataTests
	{
		[Fact]
		private void save_weapon_death_smoke_test()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "pistol", 1f, DateTime.UtcNow);
		}

		[Fact]
		private void save_trap_death_smoke_test()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveTrapDeath(1, 1, 1f, 1f, 1, DateTime.UtcNow);
		}

		[Fact]
		private void save_trap_smoke_test()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveTrap(1, 1);
		}

		[Fact]
		private void get_weapon_stats_smoke_test()
		{
			var factory = new Factory();
			var data = factory.GetDataService();
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "pistol", 12f, DateTime.UtcNow);
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "pistol", 6f, DateTime.UtcNow);
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "pistol", 13f, DateTime.UtcNow);
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "shotgun", 3f, DateTime.UtcNow);
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "shotgun", 1f, DateTime.UtcNow);
			data.SaveWeaponDeath(1, 1, 1f, 1f, 2f, 2f, "shotgun", 6f, DateTime.UtcNow);
			data.SaveTrapDeath(1, 1, 1f, 1f, 1, DateTime.UtcNow);
			data.SaveTrapDeath(1, 1, 1f, 1f, 1, DateTime.UtcNow);
			data.SaveTrapDeath(1, 1, 1f, 1f, 1, DateTime.UtcNow);
			var stats = data.GetWeaponStats().ToList();
			Assert.Equal(3, stats.Count);
		}

		public class Factory
		{
			public readonly Mock<ILogger> LoggerMock;

			public Factory()
			{
				LoggerMock = new Mock<ILogger>();
			}

			public Data GetDataService()
			{
				var data = new Data(LoggerMock.Object);
				data.InitializeStore("Data Source=:memory:;Version=3;");
				return data;
			}
		}
	}
}