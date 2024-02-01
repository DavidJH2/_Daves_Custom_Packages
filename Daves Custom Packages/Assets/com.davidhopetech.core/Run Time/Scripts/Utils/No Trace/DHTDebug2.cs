using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace com.davidhopetech.core.Run_Time.Utils
{
	 public static partial class DHTDebug2
	{
		public static void TestLog(string msg)
		{
			Debug.Log(msg);
		}
	}
}