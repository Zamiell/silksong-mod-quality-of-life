using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace QoL;

[BepInPlugin("io.github.zamiel.qol", "QoL", "0.0.1")]
public class QoL : BaseUnityPlugin
{
    public static ConfigEntry<bool> SkipIntro { get; private set; } = null!;
    public static ConfigEntry<bool> FastMainMenu { get; private set; } = null!;

    private void Awake()
    {
        SkipIntro = Config.Bind(
            "General",
            "SkipIntro",
            true,
            "Skip the intro sequence and load the main menu directly when launching the game."
        );

        FastMainMenu = Config.Bind(
            "General",
            "FastMenu",
            true,
            "Remove the fading animations on the main menu."
        );

        var harmony = new Harmony("io.github.zamiel.qol");
        harmony.PatchAll();

        Log.Info("Loaded.");
    }
}
