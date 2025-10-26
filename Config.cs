using BepInEx.Configuration;

namespace QoL;

public static class Config
{
    public static ConfigEntry<bool> SkipIntro { get; private set; } = null!;
    public static ConfigEntry<bool> FastMainMenu { get; private set; } = null!;
    public static ConfigEntry<bool> BypassMainMenu { get; private set; } = null!;
    public static ConfigEntry<bool> SkipSaveAndQuitConfirmation { get; private set; } = null!;
    public static ConfigEntry<bool> SkipCutscenes { get; private set; } = null!;

    public static void Initialize(ConfigFile config)
    {
        SkipIntro = config.Bind(
            "General",
            "SkipIntro",
            true,
            "Skip the intro sequence and load the main menu directly when launching the game."
        );

        FastMainMenu = config.Bind(
            "General",
            "FastMenu",
            true,
            "Remove the fading animations on the main menu."
        );

        BypassMainMenu = config.Bind(
            "General",
            "BypassMainMenu",
            true,
            "Automatically navigate to the save file selection screen when the main menu loads."
        );

        SkipSaveAndQuitConfirmation = config.Bind(
            "General",
            "SkipSaveAndQuitConfirmation",
            true,
            "Automatically confirm \"Yes\" when quitting to the main menu, bypassing the confirmation prompt."
        );

        SkipCutscenes = config.Bind(
            "General",
            "SkipCutscenes",
            true,
            "Automatically skip certain cutscene sequences in the game."
        );
    }
}
