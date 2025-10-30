namespace QoL.Features;

[HarmonyPatch(typeof(StartManager), "Start")]
public class StartManager_Start_Patch
{
    static void Prefix(StartManager __instance)
    {
        if (!Config.SkipIntro.Value)
        {
            return;
        }

        if (__instance.startManagerAnimator != null)
        {
            __instance.startManagerAnimator.speed = 1000f;
        }
    }
}
