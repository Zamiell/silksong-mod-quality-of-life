using BepInEx;
using UnityEngine;

namespace QoL;

/// <summary>
/// Logging utility that dynamically retrieves the mod name from the BepInPlugin attribute.
/// </summary>
public static class Log
{
    private static string? _modName = null;

    private static string ModName
    {
        get
        {
            if (_modName == null)
            {
                var attribute = (BepInPlugin)
                    System.Attribute.GetCustomAttribute(typeof(QoL), typeof(BepInPlugin));
                _modName = attribute?.Name ?? "Unknown";
            }
            return _modName;
        }
    }

    public static void Info(string message)
    {
        var msg = GetFormattedLogMessage(message);
        Debug.Log(msg);
    }

    public static void Error(string message)
    {
        var msg = GetFormattedLogMessage(message);
        Debug.LogError(msg);
    }

    private static string GetFormattedLogMessage(String message)
    {
        var timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        return $"[{timestamp}] {ModName}: {message}";
    }
}
