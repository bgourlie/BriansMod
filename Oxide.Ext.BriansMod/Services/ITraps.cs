namespace Oxide.Ext.BriansMod.Services
{
	using JetBrains.Annotations;
	using Model;

	public interface ITraps
	{
		void RecordTrap([NotNull] IBasePlayer player, [NotNull] ITrap trap);
		void DestroyTrap([NotNull] ITrap trap);
		ulong GetOwnerId([NotNull] ITrap trap);
		ulong GetTrapId([NotNull] ITrap trap);
	}
}