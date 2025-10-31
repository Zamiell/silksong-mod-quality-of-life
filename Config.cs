using BepInEx.Configuration;

namespace RacingMod;

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
            "Skips the \"Team Cherry\" splash screen and the screen that explains to you what the save indicator means."
        );
        FastMainMenu = config.Bind(
            "General",
            "FastMainMenu",
            true,
            "Makes the menu appear instantly (instead of slowly fading in) and makes the save slot screen appear instantly (instead of animating all 4 rectangles one by one)."
        );
        BypassMainMenu = config.Bind(
            "General",
            "BypassMainMenu",
            true,
            "Makes the main menu automatically transition to the save file selection screen."
        );

        // In-Game Menu Features
        SkipSaveAndQuitConfirmation = config.Bind(
            "General",
            "SkipSaveAndQuitConfirmation",
            true,
            "Skips the \"Yes\" or \"No\" confirmation menu that appears after selecting \"Save & Quit\"."
        );
        FastText = config.Bind(
            "General",
            "FastText",
            true,
            "Makes dialog text with NPCs instantaneous."
        );

        // In-Game Features
        SkipCutscenes = config.Bind(
            "General",
            "SkipCutscenes",
            true,
            "Automatically skip certain cutscenes in the game. See the README.md for more specific details."
        );
        SkipAreaIntro = config.Bind(
            "General",
            "SkipAreaIntro",
            true,
            "Marks all areas as visited to skip the splash text that appears when you first visit an area."
        );
        SkipWeakness = config.Bind(
            "General",
            "SkipWeakness",
            true,
            "Skips the \"weakness\" segments of the game where Hornet moves very slowly."
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
            "Warp to a specific scene upon loading into the game. (Edit \"DebugWarp.cs\" to specify the scene.)"
        );
        LogFSMDetails = config.Bind(
            "Debug",
            "LogFSMDetails",
            false,
            "Log detailed information about all FSMs (Finite State Machines) that are initialized. Useful for finding FSM names and game object names for modding."
        );
    }
}
