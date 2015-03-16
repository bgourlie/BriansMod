// ReSharper disable InconsistentNaming

namespace Oxide.Ext.BriansMod.Tests
{
	using Model;
	using Moq;
	using Services;
	using Xunit;

	public class PluginHookTests
	{
		[Fact]
		public void init_should_initialize_data_store()
		{
			var factory = new Factory();
			factory.configMock.Setup(c => c.DataDirectory).Returns("./");
			var plugin = factory.GetPlugin();
			plugin.Init();
			factory.dataMock.Verify(d => d.InitializeStore(It.IsAny<string>()), Times.Once());
		}

		[Fact]
		public void on_entity_death_for_trap_should_destroy_trap()
		{
			var factory = new Factory();
			var plugin = factory.GetPlugin();
			var trapMock = new Mock<ITrap>();
			var hitInfoMock = new Mock<IHitInfo>();
			plugin.OnEntityDeath(trapMock.Object, hitInfoMock.Object);
			factory.trapsMock.Verify(t => t.DestroyTrap(trapMock.Object));
		}

		private class Factory
		{
			public readonly Mock<IChat> chatMock;
			public readonly Mock<IConfiguration> configMock;
			public readonly Mock<IConsole> consoleMock;
			public readonly Mock<IData> dataMock;
			public readonly Mock<IDeaths> deathsMock;
			public readonly Mock<IInjuries> injuriesMock;
			public readonly Mock<ILogger> loggerMock;
			public readonly Mock<IPlayers> playersMock;
			public readonly Mock<ITraps> trapsMock;

			public Factory()
			{
				chatMock = new Mock<IChat>();
				configMock = new Mock<IConfiguration>();
				loggerMock = new Mock<ILogger>();
				deathsMock = new Mock<IDeaths>();
				injuriesMock = new Mock<IInjuries>();
				dataMock = new Mock<IData>();
				trapsMock = new Mock<ITraps>();
				consoleMock = new Mock<IConsole>();
				playersMock = new Mock<IPlayers>();
			}

			public BriansModPlugin GetPlugin()
			{
				return new BriansModPlugin(
					loggerMock.Object,
					dataMock.Object,
					chatMock.Object,
					configMock.Object,
					deathsMock.Object,
					injuriesMock.Object,
					trapsMock.Object,
					consoleMock.Object,
					playersMock.Object);
			}
		}
	}
}