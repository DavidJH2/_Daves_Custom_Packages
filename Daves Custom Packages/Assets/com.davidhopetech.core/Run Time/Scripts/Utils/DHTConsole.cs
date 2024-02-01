using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using UnityEditorInternal;

public class DHTConsole : EditorWindow
{
    private       Vector2        scrollPosition1;
    private       Vector2        scrollPosition2;
    private       List<LogEntry> logEntries      = new List<LogEntry>();

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
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true, // Enable word wrapping
            fontSize = 6, // Adjust font size as needed
            // Set fixed height and padding for each label
            fixedHeight = 60, // Adjust fixed height as needed
            padding     = new RectOffset(5, 5, 5, 5) // Adjust padding as needed (left, right, top, bottom)
        };
        
        Event          currentEvent = Event.current;
        Rect           labelRect;
        
        scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1);

        for (int i = 0; i < logEntries.Count; i++)
        {
            EditorGUILayout.LabelField(logEntries[i].Message, EditorStyles.largeLabel);
            labelRect = GUILayoutUtility.GetLastRect(); // Get the rect of the label we just drew

            // Check for mouse click within the last drawn label rect
            if (labelRect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    if (currentEvent.clickCount == 1)
                    {
                        stackTrace = logEntries[i].StackTrace;
                        Repaint();
                    }
                    else if (currentEvent.clickCount == 2)
                    {
                        OpenLogInEditor(logEntries[i]);
                    }
                }
            }
        }


        EditorGUILayout.EndScrollView();

        // -----------------------------------------------------
        
        scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2);
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
        ExtractFilePathAndLineNumber(logEntry.StackTrace, out var filePath, out var lineNumber);
        InternalEditorUtility.OpenFileAtLineExternal(filePath, lineNumber);
    }
    
    private void ExtractFilePathAndLineNumber(string stackTrace, out string filePath, out int lineNumber)
    {
        filePath   = string.Empty;
        lineNumber = 0;

        // Pattern to extract the file path and line number
        string pattern = @"\(at\s+(.*?):(\d+)\)";

        Match match = Regex.Match(stackTrace, pattern);

        if (match.Success && match.Groups.Count > 2)
        {
            filePath   = match.Groups[1].Value;
            lineNumber = int.Parse(match.Groups[2].Value);

            // Normalize the file path
            filePath = NormalizeFilePath(filePath);
        }
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
    }}
