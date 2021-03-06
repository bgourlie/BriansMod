﻿namespace Oxide.Ext.BriansMod
{
	using System;

	internal static class Util
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static int ToUnixEpoch(this DateTime datetime)
		{
			if (datetime.Kind != DateTimeKind.Utc)
			{
				throw new ArgumentException("DateTime must be UTC");
			}

			if (datetime < Epoch)
			{
				throw new ArgumentException("DateTime cannot be before unix epoch.");
			}

			var t = datetime - Epoch;
			return (int) t.TotalSeconds;
		}
	}
}