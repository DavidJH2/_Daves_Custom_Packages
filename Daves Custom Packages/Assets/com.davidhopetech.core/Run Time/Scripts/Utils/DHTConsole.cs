using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DHTConsole : EditorWindow
{
    private Vector2 logScrollPosition;
    private Vector2 stackTraceScrollPosition;
    private float dividerPosition;
    private bool isResizing;
    private System.Collections.Generic.List<LogEntry> logEntries = new System.Collections.Generic.List<LogEntry>();
    private const float DividerHeight = 2f; // Height of the divider

    [MenuItem("Window/DHT Console")]
    public static void ShowWindow()
    {
        GetWindow<DHTConsole>("DHT Console");
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        dividerPosition = position.height / 2; // Initialize divider position to half of window height
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logEntries.Add(new LogEntry { Log = logString, StackTrace = stackTrace, Type = type });
        Repaint();
    }

    private void OnGUI()
    {
        DrawLogs();
        DrawDivider();
        DrawStackTrace();
        ProcessEvents(Event.current);
    }

    private void DrawLogs()
    {
        logScrollPosition = EditorGUILayout.BeginScrollView(logScrollPosition, GUILayout.Height(dividerPosition - DividerHeight / 2));

        foreach (var entry in logEntries)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(entry.Log, EditorStyles.largeLabel); // Using LargeLabel for log entries
            HandleEntryClick(entry);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
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
        stackTraceScrollPosition = EditorGUILayout.BeginScrollView(stackTraceScrollPosition, GUILayout.Height(position.height - dividerPosition - DividerHeight / 2));
        GUILayout.Label(logEntries.Count > 0 ? stackTrace : "", EditorStyles.largeLabel); // Using LargeLabel for stack trace
        EditorGUILayout.EndScrollView();
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

    private void HandleEntryClick(LogEntry entry)
    {
        if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 1)
            {
                stackTrace = entry.StackTrace;
                Repaint();
            }
            else if (Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
            {
                OpenLogInDefaultEditor(entry.StackTrace);
            }
        }
    }

    private void OpenLogInDefaultEditor(string stackTrace)
    {
        string filePath;
        int    lineNumber;
        ExtractFilePathAndLineNumber(stackTrace, out filePath, out lineNumber);
        InternalEditorUtility.OpenFileAtLineExternal(filePath, lineNumber);
    }

    private void ExtractFilePathAndLineNumber(string stackTrace, out string filePath, out int lineNumber)
    {
#if true
    filePath = string.Empty;
    lineNumber = 0;

    // Pattern to extract the file path and line number
    string pattern = @"\(at\s+(.*?):(\d+)\)";

    // Find all matches in the stack trace
    MatchCollection matches = Regex.Matches(stackTrace, pattern);

    foreach (Match match in matches)
    {
        if (match.Success && match.Groups.Count > 2)
        {
            string tempFilePath = match.Groups[1].Value;

            // Check if the file path contains the "No Trace" folder
            if (!tempFilePath.Contains("/No Trace/") && !tempFilePath.Contains("\\No Trace\\"))
            {
                filePath = NormalizeFilePath(tempFilePath);
                lineNumber = int.Parse(match.Groups[2].Value);

                // Since a valid file path and line number have been found, break out of the loop
                break;
            }
        }
    }
#else
        
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
#endif
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
        public string Log;
        public string StackTrace;
        public LogType Type;
    }
}
