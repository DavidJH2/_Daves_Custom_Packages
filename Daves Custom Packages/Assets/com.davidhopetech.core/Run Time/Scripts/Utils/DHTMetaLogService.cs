using System;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;



public class DHTMetaLogService : Object
{
	private static DHTMetaLogService _instance;
	public static  Action<string>    MetaLogEvent = (a) => { };
	
	
	// Private constructor to prevent instantiation outside
	public DHTMetaLogService(ILogHandler defaultLogHandler)
	{
	}

	public static DHTMetaLogService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance                     = new DHTMetaLogService(Debug.unityLogger.logHandler);
			}
			return _instance;
		}
	}
	
	

	public static void MetaLog(string message)
	{
		MetaLogEvent.Invoke(message);
	}
}