using BepInEx;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace RacingMod;

[BepInPlugin("io.github.zamiel.racingmod", "RacingMod", "0.0.1")]
[BepInDependency("org.silksong-modding.fsmutil", BepInDependency.DependencyFlags.HardDependency)]
public class RacingMod : BaseUnityPlugin
{
    private void Awake()
    {
        global::RacingMod.Config.Initialize(Config);

        SceneManager.sceneLoaded += OnSceneLoadPatch.OnSceneLoad;

        var harmony = new Harmony("io.github.zamiel.racingmod");
        harmony.PatchAll();

        Log.Info("Loaded.");
    }
}
