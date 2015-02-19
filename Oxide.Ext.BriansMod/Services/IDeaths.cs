namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;

	public interface IDeaths
	{
		bool TryResolvePvpDeath(IMonoBehavior entity, IHitInfo hitInfo, out PvpDeath pvpDeath);

		void Record(PvpDeath pvpDeath);

		string GetDeathMessage(PvpDeath death);
	}
}