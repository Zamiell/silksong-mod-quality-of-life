using HarmonyLib;

namespace QoL.Features;

// Make the text fade in instantly.
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

// Make the background image fade in instantly.
[HarmonyPatch(typeof(UIManager), "FadeScreenBlankerTo")]
public class UIManagerFadeScreenBlankerToPatch
{
    [HarmonyWrapSafe, HarmonyPrefix]
    private static void Prefix(ref float duration)
    {
        if (!QoL.FastMenu.Value)
        {
            return;
        }

        duration = 0f;
    }
}

// Make the main menu text appear instantly. (Normally, it only appears after a second.)
[HarmonyPatch(typeof(UIManager), "FadeScreenIn")]
public class UIManagerFadeScreenInPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(ref float __result)
    {
        if (!QoL.FastMenu.Value)
        {
            return;
        }

        __result = 0f;
    }
}
