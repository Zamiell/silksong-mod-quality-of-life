using HarmonyLib;

namespace QoL.Features;

[HarmonyPatch(typeof(StartManager), "Start")]
public class StartManager_Start_Patch
{
    static void Prefix(StartManager __instance)
    {
        if (!QoL.SkipIntro.Value)
        {
            return;
        }

        __instance.startManagerAnimator?.speed = 1000f;
    }
}
