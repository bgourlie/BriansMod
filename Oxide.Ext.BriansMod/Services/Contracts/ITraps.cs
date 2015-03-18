namespace Oxide.Ext.BriansMod.Services.Contracts
{
	using JetBrains.Annotations;
	using Model.Rust.Contracts;

	public interface ITraps
	{
		void RecordTrap([NotNull] IBasePlayer player, [NotNull] ITrap trap);
		void DestroyTrap([NotNull] ITrap trap);
		ulong GetOwnerId([NotNull] ITrap trap);
		ulong GetTrapId([NotNull] ITrap trap);
	}
}