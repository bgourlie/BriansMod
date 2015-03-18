namespace Oxide.Ext.BriansMod.Model.Rust
{
	using Contracts;
	using JetBrains.Annotations;
	using UnityEngine;

	public class WrappedMonoBehavior : IMonoBehavior
	{
		private readonly MonoBehaviour _monoBehaviour;

		public WrappedMonoBehavior([NotNull] MonoBehaviour monoBehaviour)
		{
			_monoBehaviour = monoBehaviour;
		}

		private bool Equals(WrappedMonoBehavior other)
		{
			return _monoBehaviour.Equals(other._monoBehaviour);
		}

		public override sealed bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is WrappedMonoBehavior && Equals((WrappedMonoBehavior) obj);
		}

		public override sealed int GetHashCode()
		{
			return _monoBehaviour.GetHashCode();
		}

		public static bool operator ==(WrappedMonoBehavior left, WrappedMonoBehavior right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(WrappedMonoBehavior left, WrappedMonoBehavior right)
		{
			return !Equals(left, right);
		}
	}
}