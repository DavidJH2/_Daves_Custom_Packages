using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using UnityEditorInternal;

public class DHTConsole : EditorWindow
{
    private Vector2 scrollPosition;
    private List<LogEntry> logEntries = new List<LogEntry>();
    private double lastClickTime = 0;
    private const double doubleClickTime = 0.3;

    private class LogEntry
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public LogType Type { get; set; }
    }

    [MenuItem("Tools/DHT Console")] //
    public static void ShowWindow()
    {
        GetWindow<DHTConsole>("DHT Console");
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog; //
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logEntries.Add(new LogEntry { Message = logString, StackTrace = stackTrace, Type = type });
        Repaint();
    }

    private string stackTrace = "";
    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < logEntries.Count; i++)
        {
            if (GUILayout.Button(logEntries[i].Message, EditorStyles.label))
            {
                if ((EditorApplication.timeSinceStartup - lastClickTime) < doubleClickTime)
                {
                    var logEntry = logEntries[i];
                    stackTrace = logEntry.StackTrace;
                    OpenLogInEditor(logEntry);
                }
                lastClickTime = EditorApplication.timeSinceStartup; //
            }
        }
        
        GUILayout.Label(stackTrace);

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Clear Logs"))
        {
            logEntries.Clear();
            stackTrace = "";
        }
    }

    private void OpenLogInEditor(LogEntry logEntry)
    {
        // Parse the stack trace to find the first file reference
        string pattern = @"\bat\s(.*):(\d+)\)";
        Match  match   = Regex.Match(logEntry.StackTrace, pattern);

        if (match.Success && match.Groups.Count > 2)
        {
            string filePath   = match.Groups[1].Value;
            int    lineNumber = int.Parse(match.Groups[2].Value);

            // Replace the Unity project path with the full path
            filePath = filePath.Replace("Assets", Application.dataPath);

            // Open the file at the specific line in the default editor configured for Unity
            InternalEditorUtility.OpenFileAtLineExternal(filePath, lineNumber);
        }
    }}
