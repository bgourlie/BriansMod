namespace Oxide.Ext.BriansMod.Services
{
	using Model;

	public interface IInjuries
	{
		void UpdateInjuryStatus(IHitInfo hitInfo);
		bool TryGetLastRelevantInjury(IBasePlayer player, out Injury injury);
		Injury ResolveInjury(IHitInfo hitInfo);
	}
}