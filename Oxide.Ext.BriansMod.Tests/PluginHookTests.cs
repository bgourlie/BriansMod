// ReSharper disable InconsistentNaming

namespace Oxide.Ext.BriansMod.Tests
{
	using Moq;

	using Oxide.Ext.BriansMod.Model;
	using Oxide.Ext.BriansMod.Services;

	using Xunit;

	public class PluginHookTests
	{
		[Fact]
		public void init_should_initialize_data_store()
		{
			var factory = new Factory();
			var plugin = factory.GetPlugin();
			plugin.Init();
			factory.dataMock.Verify(d => d.InitializeStore(), Times.Once());
		}

		[Fact]
		public void on_entity_death_for_trap_should_delete_trap()
		{
			var factory = new Factory();
			var plugin = factory.GetPlugin();
			var trapMock = new Mock<ITrap>();
			var hitInfoMock = new Mock<IHitInfo>();
			plugin.OnEntityDeath(trapMock.Object, hitInfoMock.Object);
			factory.trapsMock.Verify(t => t.DeleteTrap(trapMock.Object));
		}

		private class Factory
		{
			public readonly Mock<IChat> chatMock;

			public readonly Mock<IConsole> consoleMock;

			public readonly Mock<IData> dataMock;

			public readonly Mock<IDeaths> deathsMock;

			public readonly Mock<IInjuries> injuriesMock;

			public readonly Mock<ILogger> loggerMock;

			public readonly Mock<ITraps> trapsMock;

			public Factory()
			{
				this.chatMock = new Mock<IChat>();
				this.consoleMock = new Mock<IConsole>();
				this.loggerMock = new Mock<ILogger>();
				this.deathsMock = new Mock<IDeaths>();
				this.injuriesMock = new Mock<IInjuries>();
				this.dataMock = new Mock<IData>();
				this.trapsMock = new Mock<ITraps>();
			}

			public BriansModPlugin GetPlugin()
			{
				return new BriansModPlugin(
					this.loggerMock.Object,
					this.dataMock.Object,
					this.chatMock.Object,
					this.consoleMock.Object,
					this.deathsMock.Object,
					this.injuriesMock.Object,
					this.trapsMock.Object);
			}
		}
	}
}