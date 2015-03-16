namespace Oxide.Ext.BriansMod.Services
{
	using System;
	using JetBrains.Annotations;
	using Model;

	public interface IInjuries
	{
		void UpdateInjuryStatus([NotNull] IHitInfo hitInfo);
		bool TryGetLastRelevantInjury([NotNull] IBasePlayer player, TimeSpan timeframe, out Injury injury);
		Injury ResolveInjury([NotNull] IHitInfo hitInfo);
	}
}