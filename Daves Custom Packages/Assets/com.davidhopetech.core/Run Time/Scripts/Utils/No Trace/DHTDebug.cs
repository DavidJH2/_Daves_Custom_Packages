using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Utils
{
	public static partial class DHTDebug
	{
		public static void Tag(Object context, string caps = "----")
		{
			var tag = MethodeInfo(context,1);
			UnityEngine.Debug.Log($"{caps}  {tag}  {caps}", context);
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
				UnityEngine.Debug.Log($"{prefix}{message}", context);
			else
				UnityEngine.Debug.Log($"{prefix}{message}");
		}

		public static void Log(object message, string decoration, Object context = null, bool tag = false, int frameOffset = 1)
		{
			var prefix = (tag) ? MethodeInfo(context, frameOffset) + "  " : "";
			
			if (context)
				UnityEngine.Debug.Log($"{prefix}{decoration}  {message}  {decoration}", context);
			else
				UnityEngine.Debug.Log($"{prefix}{decoration}  {message}  {decoration}");
		}

		public static void Log(object message, string decoration1, string decoration2, Object context = null, bool tag = false, int frameOffset = 1)
		{
			var prefix = (tag) ? MethodeInfo(context, frameOffset) + "  " : "";
			
			if (context)
				UnityEngine.Debug.Log($"{prefix}{decoration1}  {message}  {decoration2}", context);
			else
				UnityEngine.Debug.Log($"{prefix}{decoration1}  {message}  {decoration2}");
		}
	}
}