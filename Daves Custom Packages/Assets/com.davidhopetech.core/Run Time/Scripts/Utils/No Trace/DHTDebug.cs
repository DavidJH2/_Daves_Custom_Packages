using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Utils
{
	public static partial class DhtDebug
	{
		public static void Tag(Object context, string caps = "----")
		{
			var tag = MethodeInfo(context,1);
			UnityEngine.Debug.Log($"{caps}  {tag}  {caps}\n", context);
		}
		public static void LogTag(object message, Object context = null)
		{
			Log(message, context, true, 2);
		}

		public static void LogTag(object message, string decoration, Object context = null)
		{
			Log(message, decoration, context, true, 2);
		}
		
		public static void LogTag(object message, string decoration1, string decoration2, Object context = null)
		{
			Log(message, decoration1, decoration2, context, true, 2);
		}

		public static void Log(object message, Object context = null, bool tag = false, int frameOffset = 1)
		{
			var prefix = (tag) ? MethodeInfo(context, frameOffset) + "  " : "";

			if (context)
				UnityEngine.Debug.Log($"{prefix}{message}\n", context);
			else
				UnityEngine.Debug.Log($"{prefix}{message}\n");
		}

		public static void Log(object message, string decoration, Object context = null, bool tag = false, int frameOffset = 1)
		{
			var prefix = (tag) ? MethodeInfo(context, frameOffset) + "  " : "";
			
			if (context)
				UnityEngine.Debug.Log($"{prefix}{decoration}  {message}  {decoration}\n", context);
			else
				UnityEngine.Debug.Log($"{prefix}{decoration}  {message}  {decoration}\n");
		}

		public static void Log(object message, string decoration1, string decoration2, Object context = null, bool tag = false, int frameOffset = 1)
		{
			var prefix = (tag) ? MethodeInfo(context, frameOffset) + "  " : "";
			
			if (context)
				UnityEngine.Debug.Log($"{prefix}{decoration1}  {message}  {decoration2}\n", context);
			else
				UnityEngine.Debug.Log($"{prefix}{decoration1}  {message}  {decoration2}\n");
		}
	}
}