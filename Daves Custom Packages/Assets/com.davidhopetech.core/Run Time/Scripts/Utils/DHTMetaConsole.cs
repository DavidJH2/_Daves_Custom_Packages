using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
public class DHTMetaConsole : EditorWindow
{
	private       Vector2        logScrollPosition;
	private       List<LogEntry> logEntries        = new();
	private const float          ClearButtonHeight = 25;

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
		logScrollPosition.y = float.MaxValue;
		entryAdded          = true;
	}

	private void OnGUI()
	{
		if (entryAdded)
		{
			Repaint();
			entryAdded = false;
		}
		DrawClearButton();
		DrawLogs();
	}

	private void DrawLogs()
	{
		logScrollPosition   = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(position.height - ClearButtonHeight));

		foreach (var entry in logEntries)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(entry.Message, EditorStyles.largeLabel); // Using LargeLabel for log entries
			EditorGUILayout.EndHorizontal();
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

	private class LogEntry
	{
		public string  Message;
	}
}

#endif