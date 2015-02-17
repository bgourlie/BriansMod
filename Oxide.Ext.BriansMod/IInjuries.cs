namespace Oxide.Ext.BriansMod
{
	using Model;

	public interface IInjuries
	{
		void UpdateInjuryStatus(HitInfo hitInfo);

		bool TryGetLastRelevantInjury(BasePlayer player, out Injury injury);

		Injury ResolveInjury(HitInfo hitInfo);
	}
}