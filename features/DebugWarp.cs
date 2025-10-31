namespace RacingMod.Features;

[HarmonyPatch(typeof(GameManager), "OnWillActivateFirstLevel")]
public class GameManager_OnWillActivateFirstLevel_DebugWarp_Patch
{
    static string WarpScene = "Mosstown_02";

    static void Postfix(GameManager __instance)
    {
        if (!Config.DebugWarp.Value)
        {
            return;
        }

        // Start a coroutine to warp after the initial scene load is complete.
        __instance.StartCoroutine(WarpAfterDelay(__instance));
    }

    private static IEnumerator WarpAfterDelay(GameManager gm)
    {
        // Wait for the initial scene to be fully loaded.
        yield return new WaitForSeconds(1f);

        Log.Info($"Warping to: {WarpScene}");

        // Use BeginSceneTransition for proper scene loading.
        gm.BeginSceneTransition(
            new GameManager.SceneLoadInfo
            {
                SceneName = WarpScene,
                EntryGateName = "",
                EntryDelay = 0f,
                PreventCameraFadeOut = false,
                WaitForSceneTransitionCameraFade = true,
                Visualization = GameManager.SceneLoadVisualizations.Default,
                AlwaysUnloadUnusedAssets = true,
            }
        );
    }
}
