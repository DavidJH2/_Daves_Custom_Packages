using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

public class DHTConsole : EditorWindow
{
	private       Vector2        logScrollPosition;
	private       Vector2        stackTraceScrollPosition;
	private       float          dividerPosition;
	private       bool           isResizing;
	private       List<LogEntry> logEntries        = new();
	private const float          DividerHeight     = 2f; // Height of the divider
	private const float          ClearButtonHeight = 25;


	[MenuItem("Window/DHT Console")]
	public static void ShowWindow()
	{
		GetWindow<DHTConsole>("DHT Console");
	}

	
	private void OnEnable()
	{
		// Application.logMessageReceived += HandleLog;
		DHTLogHandler.LogEvent += HandleLog;
		
		var defaultLogHandler = Debug.unityLogger.logHandler;
		Debug.unityLogger.logHandler = new DHTLogHandler(defaultLogHandler);

		dividerPosition = position.height / 2; // Initialize divider position to half of window height
	}

	private void OnDisable()
	{
		DHTLogHandler.LogEvent -= HandleLog;
		// Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string logString, string stackTrace, LogType type, Object context)
	{
		if (stackTrace == "") stackTrace = "{No Stack Trace}";
		logEntries.Add(new LogEntry { Log = logString, StackTrace = stackTrace, Type = type});
		Repaint();
	}

	private void OnGUI()
	{
		DrawClearButton();
		DrawLogs();
		DrawDivider();
		DrawStackTrace();
		ProcessEvents(Event.current);
	}

	private void DrawLogs()
	{
		List<string> metaLogs = new();
		logScrollPosition = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(dividerPosition - DividerHeight / 2));

		foreach (var entry in logEntries)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(entry.Log, EditorStyles.largeLabel); // Using LargeLabel for log entries
			HandleEntryClick(entry, metaLogs);
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();
		if(metaLogs.Count>0)
			Debug.Log("-------------------------");
		foreach (var log in metaLogs)
		{
			Debug.Log(log);
		}
	}

	private void DrawDivider()
	{
		GUILayout.Box("", GUILayout.Height(DividerHeight), GUILayout.ExpandWidth(true));

		Rect dividerRect = GUILayoutUtility.GetLastRect();
		EditorGUI.DrawRect(dividerRect, new Color(0.1f, 0.1f, 0.1f)); // Adjust the RGB values as needed to get your desired shade of dark
		EditorGUIUtility.AddCursorRect(dividerRect, MouseCursor.ResizeVertical);

		if (Event.current.type == EventType.MouseDown && dividerRect.Contains(Event.current.mousePosition))
		{
			isResizing = true;
		}
	}

	private string stackTrace = "";

	private void DrawStackTrace()
	{
		stackTraceScrollPosition = EditorGUILayout.BeginScrollView(stackTraceScrollPosition, GUILayout.Height(position.height - dividerPosition - DividerHeight / 2-ClearButtonHeight));
		GUILayout.Label(logEntries.Count > 0 ? stackTrace : "", EditorStyles.largeLabel); // Using LargeLabel for stack trace
		EditorGUILayout.EndScrollView();
	}

	private void DrawClearButton()
	{
		Rect buttonRect = GUILayoutUtility.GetRect(100, 100, ClearButtonHeight, ClearButtonHeight);
		buttonRect.width = Mathf.Min(buttonRect.width, 200); // Constrain the width to a maximum of 200
		if (GUI.Button(buttonRect, "Clear")) {
			logEntries.Clear();
			stackTrace = "";
		}
	}

	private void ProcessEvents(Event e)
	{
		switch (e.type)
		{
			case EventType.MouseDrag:
				if (isResizing)
				{
					dividerPosition += e.delta.y;
					Repaint();
				}
				break;
			case EventType.MouseUp:
				isResizing = false;
				break;
		}
	}

	private void HandleEntryClick(LogEntry entry, List<string> metaLogs)
	{
		if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
		{
			if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 1)
			{
				stackTrace = entry.StackTrace;

				
				foreach (var line in stackTrace.Split('\n'))
				{
					// Check if the line includes file path but not "No Trace"
					if (line.Contains("at ") && !line.Contains("No Trace"))
					{
						var match = Regex.Match(line, @"(?<class>[^ ]+):[^ ]+\s*\(.*\) \(at ");
						if (match.Success)
						{
							metaLogs.Add($"Found class: {match.Groups["class"].Value}");
							//break; // Stop after finding the first match
						}
					}
				}
				
				
				Repaint();
			}
			else if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
			{
				ExtractFilePathAndLineNumber(stackTrace, out var filePath, out var lineNumber);

				DHTMetaLogService.MetaLogEvent.Invoke($"-----------  File: {filePath}, Line: {lineNumber}  -----------");
				if (filePath != "")
				{
					InternalEditorUtility.OpenFileAtLineExternal(filePath, lineNumber);
				}
			}
		}
	}

	
	private void ExtractFilePathAndLineNumber(string stackTrace, out string filePath, out int lineNumber)
	{
		filePath   = string.Empty;
		lineNumber = 0;

		
		string pattern = @"in (.*?):(\d+)";

		// Find matches
		MatchCollection matches = Regex.Matches(stackTrace, pattern);

		// Iterate over matches and print file paths and line numbers
		foreach (Match match in matches)
		{
			if (match.Groups.Count == 3) // Ensure we have both file path and line number groups
			{
				string filePath2   = match.Groups[1].Value;
				int lineNumber2 = int.Parse( match.Groups[2].Value);
				DHTMetaLogService.MetaLogEvent.Invoke($"File: {filePath2}, Line: {lineNumber2}");

				if (filePath == "")
				{
					if (lineNumber2!=0 && !filePath2.Contains("No Trace", StringComparison.InvariantCultureIgnoreCase))
					{
						filePath   = filePath2;
						lineNumber = lineNumber2;
					}
				}
			}
		}
		DHTMetaLogService.MetaLogEvent.Invoke($"------------------------------------------------------------------------------------------------");
	}

	private string NormalizeFilePath(string filePath)
	{
		// Replace backslashes with forward slashes for consistency
		string normalizedPath = filePath.Replace("\\", "/");

		// If the path starts with a drive letter, it's already absolute
		if (Regex.IsMatch(normalizedPath, @"^[a-zA-Z]:/"))
		{
			return normalizedPath;
		}

		// If not, prepend the project's base path to make it absolute
		return Application.dataPath.Replace("/Assets", "") + "/" + normalizedPath;
	}
	private class LogEntry
	{
		public string  Log;
		public string  StackTrace;
		public LogType Type;
	}
}
