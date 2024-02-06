using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
public class DHTMetaConsole : EditorWindow
{
	private       Vector2        logScrollPosition;
	private       List<LogEntry> logEntries         = new();
	private const float          ClearButtonHeight  = 25;
	private       bool           isScrolledToBottom = false;
	public static string         Message1;
	public static string         Message2;
	public static string         Message3;

	private bool entryAdded = false;

	private void OnEnable()
	{
		DHTMetaLogService.MetaLogEvent += MetaLog;
	}

	private void OnDisable()
	{
		DHTMetaLogService.MetaLogEvent -= MetaLog;
	}

	[MenuItem("David's Tools/DHT Meta Console")]
	public static void ShowWindow()
	{
		GetWindow<DHTMetaConsole>("DHT Meta Console");
	}

	public void MetaLog(string message)
	{
		logEntries.Add(new LogEntry { Message = message});
		entryAdded          = true;
	}

	

	
	private void OnGUI()
	{
		if (entryAdded && isScrolledToBottom)
		{
			entryAdded = false;
			ScrollLogWindowToBottom();
		}

		DrawTopLine();
		DrawLogs();
		
		if (Event.current.type != EventType.Repaint)
		{
			Repaint();
		}
	}

	private void DrawTopLine()
	{
		GUILayout.BeginHorizontal();

		GUILayout.Space(10); 
		if (GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(ClearButtonHeight))) {
			logEntries.Clear();
		}
		GUILayout.Space(10); 
		GUILayout.TextArea(Message1, EditorStyles.largeLabel, GUILayout.Width(300)); // Using LargeLabel for stack trace
		GUILayout.TextArea(Message2, EditorStyles.largeLabel, GUILayout.Width(300)); // Using LargeLabel for stack trace
		GUILayout.TextArea(Message3, EditorStyles.largeLabel, GUILayout.Width(300)); // Using LargeLabel for stack trace
		
		GUILayout.EndHorizontal();
	} 

	private void DrawLogs()
	{
		float startY = 0;
		
		if (Event.current.type == EventType.Repaint)
		{
			var lastRect = GUILayoutUtility.GetLastRect();
			startY   = lastRect.y + lastRect.height;
		}
		
		
		var logScrollViewHeight = position.height - ClearButtonHeight;
		logScrollPosition = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(logScrollViewHeight));

		foreach (var entry in logEntries)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(entry.Message, EditorStyles.largeLabel); // Using LargeLabel for log entries
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
				var lastRect           = GUILayoutUtility.GetLastRect();
				var totalContentHeight = lastRect.y + lastRect.height;
				isScrolledToBottom = (logScrollPosition.y + logScrollViewHeight) >= (totalContentHeight);
				// DHTMetaLogService.MetaLog($"isToBottom: {isScrolledToBottom}    Scroll Y: {logScrollPosition.y}     ScrollView Height:{logScrollViewHeight}    Content Height: {totalContentHeight}");
			}
		}

		EditorGUILayout.EndScrollView();
	}
	
	
	private void ScrollLogWindowToBottom()
	{
		logScrollPosition.y = float.MaxValue;
	}

	
	private class LogEntry
	{
		public string  Message;
	}
}

#endif