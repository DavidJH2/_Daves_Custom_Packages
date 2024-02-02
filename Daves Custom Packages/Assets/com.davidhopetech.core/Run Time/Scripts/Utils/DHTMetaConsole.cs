using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DHTMetaConsole : EditorWindow
{
	private       Vector2        logScrollPosition;
	private       List<LogEntry> logEntries        = new();
	private const float          ClearButtonHeight = 25;


	private void OnEnable()
	{
		DHTMetaLogService.MetaLogEvent += MetaLog;
	}

	private void OnDisable()
	{
		DHTMetaLogService.MetaLogEvent -= MetaLog;
	}

	[MenuItem("Window/DHT Meta Console")]
	public static void ShowWindow()
	{
		GetWindow<DHTConsole>("DHT Meta Console");
	}

	public void MetaLog(string message)
	{
		logEntries.Add(new LogEntry { Message = message});
		Repaint();
	}

	private void OnGUI()
	{
		DrawClearButton();
		DrawLogs();
	}

	private void DrawLogs()
	{
		logScrollPosition = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(position.height));

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
