using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QoL
{
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

            Logger.LogInfo("QoL: Loaded.");
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
            if (!QoL.SkipIntro.Value)
            {
                return true;
            }

            // address
            var addressFieldMaybe = typeof(AddressablesLoadScene).GetField(
                "address",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            if (!(addressFieldMaybe is System.Reflection.FieldInfo addressField))
            {
                Debug.LogError("QoL: Failed to get field: address");
                return true;
            }
            var addressMaybe = addressField.GetValue(__instance);
            if (!(addressMaybe is string address))
            {
                Debug.LogError("QoL: The \"address\" field was not a string.");
                return true;
            }

            // loadScene
            var loadSceneFieldMaybe = typeof(AddressablesLoadScene).GetField(
                "loadScene",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            if (!(loadSceneFieldMaybe is System.Reflection.FieldInfo loadSceneField))
            {
                Debug.LogError("QoL: Failed to get field: loadScene");
                return true;
            }
            var loadSceneMaybe = loadSceneField.GetValue(__instance);
            if (!(loadSceneMaybe is AssetReference loadScene))
            {
                Debug.LogError("QoL: The \"address\" field was not an object.");
                return true;
            }

            // Skip the intro.
            var hasSceneRef = !string.IsNullOrEmpty(loadScene.AssetGUID);
            if (!hasSceneRef && address == "Scenes/Pre_Menu_Intro")
            {
                Addressables.LoadSceneAsync("Scenes/Menu_Title");
                return false;
            }

            return true;
        }
    }
}
