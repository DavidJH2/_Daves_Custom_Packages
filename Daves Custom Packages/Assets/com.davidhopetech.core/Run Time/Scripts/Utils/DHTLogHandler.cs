using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DHTLogHandler : ILogHandler
{
	private static DHTLogHandler instance;
	private        ILogHandler   defaultLogHandler;

	
	public static Action<string, string, LogType, Object> LogEvent;
	
	
	// Private constructor to prevent instantiation outside
	public DHTLogHandler(ILogHandler defaultLogHandler)
	{
		this.defaultLogHandler = defaultLogHandler;
	}

	public static DHTLogHandler Instance
	{
		get
		{
			if (instance == null)
			{
				instance                     = new DHTLogHandler(Debug.unityLogger.logHandler);
				Debug.unityLogger.logHandler = instance;
			}
			return instance;
		}
	}

	public void LogFormat(LogType logType, Object context, string format, params object[] args)
	{
		
		StackTrace stackTrace       = new StackTrace(true); // 'true' to capture the file name, line number, and column number
		string     stackTraceString = stackTrace.ToString();


		LogEvent.Invoke(args[0].ToString(), stackTraceString, logType, context);
		defaultLogHandler.LogFormat(logType, context, format, args);
	}

	public void LogException(Exception exception, Object context)
	{
		// Forward exceptions to the default console log handler
		defaultLogHandler.LogException(exception, context);
	}
}