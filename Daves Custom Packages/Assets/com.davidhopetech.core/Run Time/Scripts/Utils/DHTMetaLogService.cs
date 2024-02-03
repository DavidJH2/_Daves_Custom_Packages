using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DHTMetaLogService : Object
{
	private static DHTMetaLogService instance;
	private        ILogHandler   defaultLogHandler;

	
	public static Action<string> MetaLogEvent;
	
	
	// Private constructor to prevent instantiation outside
	public DHTMetaLogService(ILogHandler defaultLogHandler)
	{
		this.defaultLogHandler = defaultLogHandler;
	}

	public static DHTMetaLogService Instance
	{
		get
		{
			if (instance == null)
			{
				instance                     = new DHTMetaLogService(Debug.unityLogger.logHandler);
			}
			return instance;
		}
	}
	
	

	public static void MetaLog(string message)
	{
		MetaLogEvent.Invoke(message);
	}

	public void LogException(Exception exception, Object context)
	{
		// Forward exceptions to the default console log handler
		defaultLogHandler.LogException(exception, context);
	}
}