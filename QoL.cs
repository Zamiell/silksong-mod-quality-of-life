using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace QoL;

[BepInPlugin("io.github.zamiel.qol", "QoL", "0.0.1")]
public class QoL : BaseUnityPlugin
{
    private void Awake()
    {
        global::QoL.Config.Initialize(Config);

        var harmony = new Harmony("io.github.zamiel.qol");
        harmony.PatchAll();

        Log.Info("Loaded.");
    }
}
