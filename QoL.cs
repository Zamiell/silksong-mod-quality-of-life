using BepInEx;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace QoL;

[BepInPlugin("io.github.zamiel.qol", "QoL", "0.0.1")]
[BepInDependency("org.silksong-modding.fsmutil", BepInDependency.DependencyFlags.HardDependency)]
public class QoL : BaseUnityPlugin
{
    private void Awake()
    {
        global::QoL.Config.Initialize(Config);

        SceneManager.sceneLoaded += OnSceneLoadPatch.OnSceneLoad;

        var harmony = new Harmony("io.github.zamiel.qol");
        harmony.PatchAll();

        Log.Info("Loaded.");
    }
}
