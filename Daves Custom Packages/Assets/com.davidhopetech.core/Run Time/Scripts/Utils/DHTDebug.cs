using System;
using System.Diagnostics;
using UnityEngine;
using Object = UnityEngine.Object;


namespace com.davidhopetech.core.Run_Time.Utils
{
	public static partial class DHTDebug
	{
		public static Func<Object, GameObject> toGameObject = (obj) => (obj is GameObject) ? ((GameObject)obj) : ((obj is Component) ? (((Component)obj).gameObject) : null);
		public static Func<Object, string>     scene        = (obj) => (toGameObject(obj)) ? (toGameObject(obj).scene.name) : "";

		
		public static bool ShowPostionResetDebug = false;

		
		public static string MethodeInfo(Object context = null, int stackFramOffset = 0)
		{
			StackTrace   stackTrace  = new StackTrace();
			StackFrame[] stackFrames = stackTrace.GetFrames();
			string       message     = "";

			if (stackFrames != null && stackFrames.Length > 1)
			{

				var gameObject = toGameObject(context);

				string sceneGoMessage;
				if (gameObject == null)
				{
					sceneGoMessage = "{Unknown GameObject}";
				}
				else
				{
					var sceneName = ((gameObject) ? (gameObject.scene.name) : "");
					sceneGoMessage = ((sceneName != "") ? $"  ( {sceneName}, {gameObject.name} ) - " : "");
				}

				string methodeDiscription;
				if (1 + stackFramOffset >= stackFrames.Length)
				{
					methodeDiscription ="{Unknown Method}";
				}
				else
				{
					StackFrame callingFrame = stackFrames[1 + stackFramOffset];
					var method       = callingFrame.GetMethod();
					
					methodeDiscription = $"{method.DeclaringType.Name}.{method.Name}";
				}

				message = $"{sceneGoMessage}{methodeDiscription}()";
			}

			return message;
		}
	}
}