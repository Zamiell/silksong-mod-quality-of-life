using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace QoL
{
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

    [BepInPlugin("io.github.zamiel.qol", "QoL", "0.0.1")]
    public class QoL : BaseUnityPlugin
    {
        public static ConfigEntry<bool> SkipIntro { get; private set; } = null!;

        private void Awake()
        {
            SkipIntro = Config.Bind(
                "General",
                "SkipIntro",
                true,
                "Skip the intro sequence and load the main menu directly when launching the game."
            );

            var harmony = new Harmony("io.github.zamiel.qol");
            harmony.PatchAll();

            Log.Info("Loaded.");
        }
    }
}
