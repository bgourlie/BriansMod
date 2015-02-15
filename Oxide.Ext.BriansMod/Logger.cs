namespace Oxide.Ext.BriansMod
{
	using Core;
	using Core.Logging;

	public class Logger : ILogger
	{
		// ReSharper disable once InconsistentNaming
		private static readonly Core.Logging.Logger _Logger = Interface.GetMod().RootLogger;

		private static Logger instance;

		public static Logger Instance => instance ?? (instance = new Logger());

		private Logger()
		{
		}

		public void Debug(string module, string message, params object[] args)
		{
			this.Log(LogType.Debug, module, message, args);
		}

		public void Error(string module, string message, params object[] args)
		{
			this.Log(LogType.Error, module, message, args);
		}

		public void Info(string module, string message, params object[] args)
		{
			this.Log(LogType.Info, module, message, args);
		}

		public void Warn(string module, string message, params object[] args)
		{
			this.Log(LogType.Warning, module, message, args);
		}

		private void Log(LogType logType, string module, string message, params object[] args)
		{
			_Logger.Write(logType, "Brian's Mod [" + module + "]: " + message, args);
		}
	}
}