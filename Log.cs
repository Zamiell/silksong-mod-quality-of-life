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
        Debug.Log($"{ModName}: {message}");
    }

    public static void Error(string message)
    {
        Debug.LogError($"{ModName}: {message}");
    }
}
