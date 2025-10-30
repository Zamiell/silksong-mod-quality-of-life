using HarmonyLib;

namespace QoL.Features;

// Mark all main areas as visited to skip area intro screens.
[HarmonyPatch(typeof(GameManager), "StartNewGame")]
public class SkipAreaIntro_StartNewGame_Patch
{
    static void Postfix()
    {
        if (!Config.SkipAreaIntro.Value)
        {
            return;
        }

        PlayerData.instance.visitedMossCave = true;
        PlayerData.instance.visitedBoneBottom = true;
        PlayerData.instance.visitedBoneForest = true;
        PlayerData.instance.visitedMosstown = true;
        PlayerData.instance.visitedHuntersTrail = true;
        PlayerData.instance.visitedDeepDocks = true;
        PlayerData.instance.visitedWilds = true;
        PlayerData.instance.visitedGrove = true;
        PlayerData.instance.visitedGreymoor = true;
        PlayerData.instance.visitedWisp = true;
        PlayerData.instance.visitedBellhartHaunted = true;
        PlayerData.instance.visitedBellhart = true;
        PlayerData.instance.visitedBellhartSaved = true;
        PlayerData.instance.visitedShellwood = true;
        PlayerData.instance.visitedCrawl = true;
        PlayerData.instance.visitedDustpens = true;
        PlayerData.instance.visitedShadow = true;
        PlayerData.instance.visitedAqueducts = true;
        PlayerData.instance.visitedMistmaze = true;
        PlayerData.instance.visitedCoral = true;
        PlayerData.instance.visitedCoralRiver = true;
        PlayerData.instance.visitedCoralRiverInner = true;
        PlayerData.instance.visitedCoralTower = true;
        PlayerData.instance.visitedSlab = true;
        PlayerData.instance.visitedGrandGate = true;
        PlayerData.instance.visitedCitadel = true;
        PlayerData.instance.visitedUnderstore = true;
        PlayerData.instance.visitedWard = true;
        PlayerData.instance.visitedHalls = true;
        PlayerData.instance.visitedLibrary = true;
        PlayerData.instance.visitedStage = true;
        PlayerData.instance.visitedGloom = true;
        PlayerData.instance.visitedWeave = true;
        PlayerData.instance.visitedMountain = true;
        PlayerData.instance.visitedIceCore = true;
        PlayerData.instance.visitedHang = true;
        PlayerData.instance.visitedHangAtrium = true;
        PlayerData.instance.visitedEnclave = true;
        PlayerData.instance.visitedArborium = true;
        PlayerData.instance.visitedCogwork = true;
        PlayerData.instance.visitedCradle = true;
        PlayerData.instance.visitedRuinedCradle = true;
        PlayerData.instance.visitedFleatopia = true;
        PlayerData.instance.visitedFleaFestival = true;
        PlayerData.instance.visitedAbyss = true;
    }
}
