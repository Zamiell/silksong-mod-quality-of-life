using GlobalEnums;
using HarmonyLib;

namespace QoL.Features;

[HarmonyPatch(typeof(GameManager), "StartNewGame")]
public class GameManager_StartNewGame_Patch
{
    static void Postfix()
    {
        if (!Config.HornetQuickStart.Value)
        {
            return;
        }

        // Skip the long cutscene at the beginning of the game that teaches you how to bind (heal).
        PlayerData.instance.bindCutscenePlayed = true;

        // We also have to set the respawn fields, or else Hornet will enter the area from the top
        // as if she had come from Bone Bottom.
        PlayerData.instance.respawnScene = "Tut_01";
        PlayerData.instance.respawnMarkerName = "Death Respawn Marker Init";

        // Skip the opening credits from appearing in the corner of the screen.
        PlayerData.instance.openingCreditsPlayed = true;
    }
}

[HarmonyPatch(typeof(GameManager), "OnWillActivateFirstLevel")]
public class GameManager_OnWillActivateFirstLevel_Patch
{
    static bool Prefix(GameManager __instance)
    {
        if (!Config.HornetQuickStart.Value)
        {
            return true; // Run the original method.
        }

        // Instead of using the entry gate system (which would spawn Hornet at "top1"),
        // use the respawn system to spawn her at the correct location at the bottom.
        HeroController.instance.isEnteringFirstLevel = true;
        __instance.RespawningHero = true;
        __instance.SetState(GameState.PLAYING);
        __instance.ui.ConfigureMenu();

        return false; // Skip the original method.
    }
}
