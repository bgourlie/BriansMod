namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using System;
	using JetBrains.Annotations;
	using Model;
	using Model.Rust.Contracts;

	public interface IInjuries
	{
		void UpdateInjuryStatus([NotNull] IHitInfo hitInfo);
		bool TryGetLastRelevantInjury([NotNull] IBasePlayer player, TimeSpan timeframe, out Injury injury);
		Injury ResolveInjury([NotNull] IHitInfo hitInfo);
	}
}