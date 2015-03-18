namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using Model;
	using Model.Rust.Contracts;

	public interface IDeaths
	{
		bool TryResolvePvpDeath(IMonoBehavior entity, IHitInfo hitInfo, out PvpDeath pvpDeath);
		void Record(PvpDeath pvpDeath);
		string GetDeathMessage(PvpDeath death);
	}
}