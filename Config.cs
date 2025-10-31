using BepInEx.Configuration;

namespace QoL;

public static class Config
{
    // Main Menu Features
    public static ConfigEntry<bool> SkipIntro { get; private set; } = null!;
    public static ConfigEntry<bool> FastMainMenu { get; private set; } = null!;
    public static ConfigEntry<bool> BypassMainMenu { get; private set; } = null!;

    // In-Game Menu Features
    public static ConfigEntry<bool> SkipSaveAndQuitConfirmation { get; private set; } = null!;
    public static ConfigEntry<bool> FastText { get; private set; } = null!;

    // In-Game Features
    public static ConfigEntry<bool> SkipCutscenes { get; private set; } = null!;
    public static ConfigEntry<bool> SkipAreaIntro { get; private set; } = null!;
    public static ConfigEntry<bool> SkipWeakness { get; private set; } = null!;

    // Debug Features
    public static ConfigEntry<bool> DebugNeedleDamage { get; private set; } = null!;
    public static ConfigEntry<bool> DebugWarp { get; private set; } = null!;
    public static ConfigEntry<bool> LogFSMDetails { get; private set; } = null!;

    public static void Initialize(ConfigFile config)
    {
        // Main Menu Features
        SkipIntro = config.Bind(
            "General",
            "SkipIntro",
            true,
            "Skip the intro sequence and load the main menu directly when launching the game."
        );
        FastMainMenu = config.Bind(
            "General",
            "FastMainMenu",
            true,
            "Remove the fading animations on the main menu."
        );
        BypassMainMenu = config.Bind(
            "General",
            "BypassMainMenu",
            true,
            "Automatically navigate to the save file selection screen when the main menu loads."
        );

        // In-Game Menu Features
        SkipSaveAndQuitConfirmation = config.Bind(
            "General",
            "SkipSaveAndQuitConfirmation",
            true,
            "Automatically confirm \"Yes\" when quitting to the main menu, bypassing the confirmation prompt."
        );
        FastText = config.Bind("General", "FastText", true, "Makes dialog text instantaneous.");

        // In-Game Features
        SkipCutscenes = config.Bind(
            "General",
            "SkipCutscenes",
            true,
            "Automatically skip certain cutscene sequences in the game."
        );
        SkipAreaIntro = config.Bind(
            "General",
            "SkipAreaIntro",
            true,
            "Mark all areas as visited to skip area intro screens."
        );
        SkipWeakness = config.Bind(
            "General",
            "SkipWeakness",
            true,
            "Skips the \"weakness\" segments where Hornet moves very slowly."
        );

        // Debug Features
        DebugNeedleDamage = config.Bind(
            "Debug",
            "DebugNeedleDamage",
            false,
            "Makes Hornet's needle deal 999 damage on each swing."
        );
        DebugWarp = config.Bind(
            "Debug",
            "DebugWarp",
            false,
            "Warp to the Tut_01 scene upon loading into the game."
        );
        LogFSMDetails = config.Bind(
            "Debug",
            "LogFSMDetails",
            false,
            "Log detailed information about all FSMs (Finite State Machines) that are initialized. Useful for finding FSM names and game object names for modding."
        );
    }
}
