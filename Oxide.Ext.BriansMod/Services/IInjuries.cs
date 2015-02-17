namespace Oxide.Ext.BriansMod.Services
{
	using Oxide.Ext.BriansMod.Model;

	public interface IInjuries
	{
		void UpdateInjuryStatus(IHitInfo hitInfo);

		bool TryGetLastRelevantInjury(IBasePlayer player, out Injury injury);

		Injury ResolveInjury(IHitInfo hitInfo);
	}
}