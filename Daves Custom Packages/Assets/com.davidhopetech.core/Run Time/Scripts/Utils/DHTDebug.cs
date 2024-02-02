using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;


namespace com.davidhopetech.core.Run_Time.Utils
{
	public static partial class DhtDebug
	{
		public static Func<Object, GameObject> toGameObject = (obj) => (obj is GameObject) ? ((GameObject)obj) : ((obj is Component) ? (((Component)obj).gameObject) : null);
		public static Func<Object, string>     scene        = (obj) => (toGameObject(obj)) ? (toGameObject(obj).scene.name) : "";

		
		public static bool ShowPostionResetDebug = false;

		
		public static string MethodeInfo(Object context, int stackFramOffset = 0)
		{
			StackTrace   stackTrace  = new StackTrace();
			StackFrame[] stackFrames = stackTrace.GetFrames();
			string       message     = "";

			if (stackFrames != null && stackFrames.Length > 1)
			{
				StackFrame callingFrame = stackFrames[1 + stackFramOffset];
				var        method       = callingFrame.GetMethod();

				var gameObject     = toGameObject(context);
				var sceneName      = ((gameObject) ? (gameObject.scene.name) : "");
				var sceneGoMessage = ((sceneName != "") ? $"  ( {sceneName}, {gameObject.name} ) - " : "");

				message = $"{sceneGoMessage}{method.DeclaringType.Name}.{method.Name}()";
			}

			return message;
		}
	}
}