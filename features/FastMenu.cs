using HarmonyLib;

namespace QoL.Features;

[HarmonyPatch(typeof(UIManager), "Start")]
public class UIManagerStartPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(UIManager __instance)
    {
        if (!QoL.FastMenu.Value)
        {
            return;
        }

        __instance.MENU_FADE_SPEED = 100f; // The normal value is 3.2f.
    }
}

[HarmonyPatch(typeof(UIManager), "FadeScreenIn")]
public class UIManagerFadeScreenInPatch
{
    [HarmonyWrapSafe, HarmonyPrefix]
    private static bool Prefix(ref float __result)
    {
        if (!QoL.FastMenu.Value)
        {
            return true; // Run the original method
        }

        __result = 0f; // Return 0 to skip the delay
        return false; // Skip the original method
    }
}
