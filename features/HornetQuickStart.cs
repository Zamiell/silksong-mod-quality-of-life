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

        // Skip the opening credits animation when starting a new game.
        PlayerData.instance.openingCreditsPlayed = true;
    }
}
