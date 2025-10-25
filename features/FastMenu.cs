using HarmonyLib;

namespace QoL.Features;

[HarmonyPatch(typeof(UIManager), "Start")]
public class UIManagerPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(UIManager __instance)
    {
        if (!QoL.FastMenu.Value)
        {
            return;
        }

        __instance.MENU_FADE_SPEED = 15;
    }
}
