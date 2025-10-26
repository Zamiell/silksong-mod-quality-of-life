using HarmonyLib;

namespace QoL.Features;

/// <summary>
/// Patch to automatically select "Yes" when quitting to the main menu.
/// Bypasses the confirmation prompt that appears after selecting "Quit to Menu" from the pause menu.
/// </summary>
[HarmonyPatch(typeof(UIManager), "UIShowReturnMenuPrompt")]
public class UIManager_UIShowReturnMenuPrompt_Patch
{
    static bool Prefix(UIManager __instance)
    {
        if (!Config.SkipSaveAndQuitConfirmation.Value)
        {
            return true;
        }

        __instance.UIReturnToMainMenu();
        return false;
    }
}
