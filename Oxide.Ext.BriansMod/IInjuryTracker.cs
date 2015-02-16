namespace Oxide.Ext.BriansMod
{
	using Oxide.Ext.BriansMod.Model;

	public interface IInjuryTracker
	{
		void UpdateInjuryStatus(BasePlayer player, HitInfo hitInfo);

		bool TryGetLastInjury(BasePlayer player, out Injury injury);
	}
}