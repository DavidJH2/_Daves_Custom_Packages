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

		DrawClearButton();
		DrawLogs();
		
		if (Event.current.type != EventType.Repaint)
		{
			Repaint();
		}
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

	private void DrawClearButton()
	{
		Rect buttonRect = GUILayoutUtility.GetRect(100, 100, ClearButtonHeight, ClearButtonHeight);
		buttonRect.width = Mathf.Min(buttonRect.width, 200); // Constrain the width to a maximum of 200
		if (GUI.Button(buttonRect, "Clear")) {
			logEntries.Clear();
		}
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