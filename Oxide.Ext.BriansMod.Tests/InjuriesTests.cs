namespace Oxide.Ext.BriansMod.Tests
{
	using Moq;
	using Services;
	using Services.Contracts;
	using Xunit;

	public class InjuriesTests
	{
		[Fact]
		public void should_work()
		{
		}

		private class Factory
		{
			public readonly Mock<ILogger> LoggerMock;
			public readonly Mock<IPlayers> PlayersMock;
			public readonly Mock<ITraps> TrapsMock;

			public Factory()
			{
				LoggerMock = new Mock<ILogger>();
				TrapsMock = new Mock<ITraps>();
				PlayersMock = new Mock<IPlayers>();
			}

			public Injuries GetInjuriesService()
			{
				return new Injuries(LoggerMock.Object, TrapsMock.Object, PlayersMock.Object);
			}
		}
	}
}