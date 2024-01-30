using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace com.davidhopetech.core.Run_Time.Utils
{
	public static class DTH
	{
		public static bool ShowPostionResetDebug = false;

		public static string DecoratedMethodeInfo(GameObject go)
		{
			return $">>>>>>>>>>>>>>>>>>>>  {MethodeInfo(go, 1)}  <<<<<<<<<<<<<<<<<<<";
		}


		public static string MethodeInfo(GameObject go, int stackFramOffset = 0)
		{
			StackTrace   stackTrace  = new StackTrace();
			StackFrame[] stackFrames = stackTrace.GetFrames();
			string       message     = "";

			// Check for null and length before accessing to avoid exceptions
			if (stackFrames != null && stackFrames.Length > 1)
			{
				StackFrame callingFrame = stackFrames[1+stackFramOffset];
				var        method       = callingFrame.GetMethod();

				message = $"{go.scene.name} - {method.DeclaringType.Name}.{method.Name}()";
			}

			return message;
		}
	}
}