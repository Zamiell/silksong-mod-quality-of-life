using GlobalEnums;
using HarmonyLib;

namespace QoL.Features;

/// <summary>
/// Patch to automatically navigate to the save file selection screen when the main menu is loaded.
/// Only bypasses on first boot and when returning from in-game (Save and Quit).
/// Does not bypass when pressing Back from the save profiles menu.
/// </summary>
[HarmonyPatch(typeof(UIManager), "SetMenuState")]
public class UIManager_SetMenuState_Patch
{
    private static bool hasSeenMainMenuBefore = false;
    private static MainMenuState previousState = MainMenuState.LOGO;

    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(UIManager __instance, MainMenuState newState)
    {
        if (!Config.BypassMainMenu.Value)
        {
            return;
        }

        if (newState == MainMenuState.MAIN_MENU)
        {
            bool isFirstMenu = !hasSeenMainMenuBefore;
            bool comingFromBackButton = previousState == MainMenuState.SAVE_PROFILES;

            if (isFirstMenu || !comingFromBackButton)
            {
                __instance.UIGoToProfileMenu();
            }

            hasSeenMainMenuBefore = true;
        }

        previousState = newState;
    }
}
