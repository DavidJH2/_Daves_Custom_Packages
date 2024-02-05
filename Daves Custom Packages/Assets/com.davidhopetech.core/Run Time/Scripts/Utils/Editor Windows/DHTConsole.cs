#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Codice.Client.Common.WebApi;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;
using UnityEditorInternal;



public class DHTConsole : EditorWindow
{
	private       Vector2        logScrollPosition;
	private       Vector2        stackTraceScrollPosition;
	private       float          dividerPosition;
	private       bool           isResizing;
	private       List<LogEntry> logEntries         = new();
	private const float          DividerHeight      = 4f; // Height of the divider
	private const float          dragAreaHeight     = 20;
	private const float          ClearButtonHeight  = 25;
	private       bool           LogEntryAdded      = false;
	private       bool           isScrolledToBottom = false;
	private       Rect           DragRect;
	private       Rect           dividerRect;



	[MenuItem("David's Tools/DHT Console")]
	public static void ShowWindow()
	{
		GetWindow<DHTConsole>("DHT Console");
	}

	
	private void OnEnable()
	{
		DHTLogHandler.LogEvent += HandleLog;
		
		var defaultLogHandler = Debug.unityLogger.logHandler;
		Debug.unityLogger.logHandler = new DHTLogHandler(defaultLogHandler);

		dividerPosition = position.height / 2; // Initialize divider position to half of window height
	}

	private void OnDisable()
	{
		DHTLogHandler.LogEvent -= HandleLog;
	}

	private void HandleLog(string logString, string stackTrace, LogType type, Object context)
	{
		var        path       = "";
		
		GameObject gameObject = DHTDebug.toGameObject(context);

		if (context is GameObject)
		{
			gameObject = ((GameObject) context);
		}

		path = gameObject.GetPath();
		/*
		*/
		
		if (stackTrace == "") stackTrace = "{No Stack Trace}";
		logEntries.Add(new LogEntry { Log = logString, StackTrace = stackTrace, Type = type, Context = context, GameObjectPath = path});

		LogEntryAdded = true;
	}

	private void OnGUI()
	{
		if(Event.current.type == EventType.MouseDown)
			DHTMetaLogService.MetaLog($"Event: {Event.current.type}");
		
		DrawClearButton();
		
		if(Event.current.type == EventType.MouseDown)
			DHTMetaLogService.MetaLog($"*************************");
		
		DrawLogs();
		
		if(Event.current.type == EventType.MouseDown)
			DHTMetaLogService.MetaLog($"*************************");
		
		DrawDivider();
		
		if(Event.current.type == EventType.MouseDown)
			DHTMetaLogService.MetaLog($"*************************");
		
		DrawStackTrace();
		
		if(Event.current.type == EventType.MouseDown)
			DHTMetaLogService.MetaLog($"*************************");
		
		if (Event.current.type != EventType.Repaint)
		{
			ProcessEvents(Event.current);
			// Repaint();
		}

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

	private void DrawLogs()
	{
		if (LogEntryAdded && isScrolledToBottom)
		{
			ScrollLogWindowToBottom();
			LogEntryAdded = false;
		}

		var          error    = "";
		List<string> metaLogs = new();

		var logScrollViewHeight = dividerPosition - DividerHeight / 2 - ClearButtonHeight;
		logScrollPosition = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(logScrollViewHeight));
		
		
		foreach (var entry in logEntries)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(entry.Log, EditorStyles.largeLabel); // Using LargeLabel for log entries

			if (Event.current.type == EventType.Repaint)
			{
				entry.rect = GUILayoutUtility.GetLastRect();
			}

			if (Event.current.type == EventType.MouseDown)
			{
				var newError = HandleEntryClick(entry, metaLogs);
				if (newError != "") error = newError;
			}

			EditorGUILayout.EndHorizontal();
		}

		
		if (Event.current.type == EventType.Repaint)
		{
			if (logEntries.Count == 0)
			{
				isScrolledToBottom = true;
			}
			else
			{
				var lastRect = GUILayoutUtility.GetLastRect();
				var totalContentHeight= lastRect.y + lastRect.height;
				isScrolledToBottom = (logScrollPosition.y + logScrollViewHeight) >= (totalContentHeight);
			}
		}

		EditorGUILayout.EndScrollView();

		if(error!="")
			DHTDebug.Log(error);
		
		if(metaLogs.Count>0)
			Debug.Log("-------------------------");
		foreach (var log in metaLogs)
		{
			Debug.Log(log);
		}
	}

	
	private int resizeCount = 0;
	private void DrawDivider()
	{
		GUILayout.Box("", GUILayout.Height(DividerHeight), GUILayout.ExpandWidth(true));

		if (Event.current.type == EventType.Repaint)
		{
			dividerRect = GUILayoutUtility.GetLastRect();

			float delta = dragAreaHeight - DividerHeight;
			DragRect        =  dividerRect;
			DragRect.y      -= delta/2;
			DragRect.height =  dragAreaHeight;
		}
		EditorGUI.DrawRect(dividerRect, new Color(0.1f, 0.1f, 0.1f)); // Adjust the RGB values as needed to get your desired shade of dark

		EditorGUIUtility.AddCursorRect(DragRect, MouseCursor.ResizeVertical);
		//===================

		//DHTMetaConsole.Message2 = $"Is Resizing: {isResizing}";

		// DHTMetaLogService.MetaLog($"{Event.current.type.ToString()}      {dividerRect.ToString()}       Mouse Pos: {Event.current.mousePosition}     Resizing = {isResizing}");

		
	}

	private string stackTrace = "";
	
	private void DrawStackTrace()
	{
		stackTraceScrollPosition = EditorGUILayout.BeginScrollView(stackTraceScrollPosition, GUILayout.Height(position.height - dividerPosition - DividerHeight / 2-ClearButtonHeight));
		EditorGUILayout.TextArea(logEntries.Count > 0 ? stackTrace : "", EditorStyles.largeLabel);
		EditorGUILayout.EndScrollView();
	}

	private void ProcessEvents(Event e)
	{

		DHTMetaConsole.Message1 = $"DPos = {dividerPosition}    Is Resizing = {isResizing}";
		DHTMetaConsole.Message2 = $"Event: {Event.current.type}";

		switch (e.type)
		{
			case EventType.MouseDown:
				DHTMetaLogService.MetaLog($"Mouse: {Event.current.mousePosition} \t\t Drag Rect: {DragRect}");
				if (DragRect.Contains(Event.current.mousePosition))
				{
					isResizing = true;
					resizeCount++;
					DHTMetaLogService.MetaLog($"*****  Is Resizing  *****");
					Repaint();
				}
				break;
			case EventType.MouseDrag:
				if (isResizing)
				{
					dividerPosition         += e.delta.y;
					Repaint();
				}
				break;
			case EventType.MouseUp:
				if (isResizing)
				{
					DHTMetaLogService.MetaLog($"=====  Stop Resizing  =====");
					isResizing = false;
				}

				Repaint();
				break;
		}
	}

	private string HandleEntryClick(LogEntry entry, List<string> metaLogs)
	{
		var error = "";
		
		if ( entry.rect.Contains(Event.current.mousePosition))
		{
			if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 1)
			{
				if (entry.Context)
				{
					Selection.activeObject = entry.Context;//
				}
				else
				{
					if (!ReferenceEquals(entry.Context, null))
					{
						GameObject gameObject = GameObject.Find(entry.GameObjectPath);

						if (gameObject)
						{
							Selection.activeObject = gameObject; //
						}
						else
						{
							error = "Referenced Object no longer exist (Playmode mode references are no longer valid after Playmode is stopped.)";
						}
					}
				}

				stackTrace = $"{entry.Log}\n{entry.StackTrace}";
				
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

		return error;
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
				// DHTMetaLogService.MetaLogEvent.Invoke($"File: {filePath2}, Line: {lineNumber2}");

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
		// DHTMetaLogService.MetaLogEvent.Invoke($"------------------------------------------------------------------------------------------------");
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
	
	
	private void ScrollLogWindowToBottom()
	{
		logScrollPosition.y = float.MaxValue;
	}

	
	private class LogEntry
	{
		public string  Log;
		public string  StackTrace;
		public LogType Type;
		public Object  Context;
		public string  GameObjectPath;
		public Rect    rect;
	}
}

#endif
