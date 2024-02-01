using System.Text.RegularExpressions;
using UnityEngine;

public class CustomLogHandler : ILogHandler
{
	private readonly ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;

	public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		string message         = string.Format(format, args);
		string modifiedMessage = ModifyStackTrace(message);
		defaultLogHandler.LogFormat(logType, context, modifiedMessage);
	}

	public void LogException(System.Exception exception, UnityEngine.Object context)
	{
		string stackTrace         = exception.StackTrace;
		string modifiedStackTrace = ModifyStackTrace(stackTrace);
		defaultLogHandler.LogException(new System.Exception(exception.Message, new System.Exception(modifiedStackTrace)), context);
	}

	private string ModifyStackTrace(string stackTrace)
	{
		// Use regex or any other method to remove or modify the path and line number
		string pattern     = @"\(at .*?\)";
		string replacement = "(at <removed>)";
		return Regex.Replace(stackTrace, pattern, replacement);
	}
}
