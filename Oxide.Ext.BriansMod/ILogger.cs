namespace Oxide.Ext.BriansMod
{
	public interface ILogger
	{
		void Info(string module, string message, params object[] args);

		void Debug(string module, string message, params object[] args);

		void Warn(string module, string message, params object[] args);

		void Error(string module, string message, params object[] args);
	}
}