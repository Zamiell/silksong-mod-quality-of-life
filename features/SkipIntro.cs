using HarmonyLib;
using UnityEngine.AddressableAssets;

namespace QoL.Features;

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
        if (addressFieldMaybe is not System.Reflection.FieldInfo addressField)
        {
            Log.Error("Failed to get field: address");
            return true;
        }
        var addressMaybe = addressField.GetValue(__instance);
        if (addressMaybe is not string address)
        {
            Log.Error("The \"address\" field was not a string.");
            return true;
        }

        // loadScene
        var loadSceneFieldMaybe = typeof(AddressablesLoadScene).GetField(
            "loadScene",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        if (loadSceneFieldMaybe is not System.Reflection.FieldInfo loadSceneField)
        {
            Log.Error("Failed to get field: loadScene");
            return true;
        }
        var loadSceneMaybe = loadSceneField.GetValue(__instance);
        if (loadSceneMaybe is not AssetReference loadScene)
        {
            Log.Error("The \"address\" field was not an AssetReference.");
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
