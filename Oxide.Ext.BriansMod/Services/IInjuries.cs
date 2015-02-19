namespace Oxide.Ext.BriansMod.Services
{
	using JetBrains.Annotations;
	using Model;

	public interface IInjuries
	{
		void UpdateInjuryStatus([NotNull] IHitInfo hitInfo);
		bool TryGetLastRelevantInjury([NotNull] IBasePlayer player, out Injury injury);
		Injury ResolveInjury([NotNull] IHitInfo hitInfo);
	}
}