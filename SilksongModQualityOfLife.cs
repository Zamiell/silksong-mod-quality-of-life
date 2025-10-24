using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QoL
{
    [BepInPlugin("io.github.zamiel.qol", "QoL", "0.0.1")]
    public class QoL : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("QoL: Loaded.");

            var harmony = new Harmony("io.github.zamiel.qol");
            harmony.PatchAll();

            Logger.LogInfo("Harmony patches applied successfully");
        }
    }

    /// <summary>
    /// Patch for AddressablesLoadScene to skip the intro and go directly to the main menu.
    /// </summary>
    [HarmonyPatch(typeof(AddressablesLoadScene), "Start")]
    public class AddressablesLoadScene_Start_Patch
    {
        static bool Prefix(AddressablesLoadScene __instance)
        {
            // Get the address field value using reflection
            var addressField = typeof(AddressablesLoadScene).GetField("address",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var loadSceneField = typeof(AddressablesLoadScene).GetField("loadScene",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (addressField == null || loadSceneField == null)
            {
                Debug.LogWarning("QoL Mod: Could not find required fields in AddressablesLoadScene");
                return true;
            }

            var loadScene = loadSceneField.GetValue(__instance) as AssetReference;
            var address = addressField.GetValue(__instance) as string;

            // Check if we have a scene reference or an address
            bool hasSceneRef = loadScene != null && !string.IsNullOrEmpty(loadScene.AssetGUID);

            if (!hasSceneRef && address == "Scenes/Pre_Menu_Intro")
            {
                // Skip the intro - load the main menu directly
                Debug.Log("QoL Mod: Skipping intro, loading main menu directly");
                Addressables.LoadSceneAsync("Scenes/Menu_Title");
                return false; // Skip the original Start method
            }

            return true;
        }
    }
}
