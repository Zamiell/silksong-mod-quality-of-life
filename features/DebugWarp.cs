namespace RacingMod.Features;

[HarmonyPatch(typeof(GameManager), "OnWillActivateFirstLevel")]
public class GameManager_OnWillActivateFirstLevel_DebugWarp_Patch
{
    static void Postfix(GameManager __instance)
    {
        string warpScene = Config.DebugWarp.Value;
        if (string.IsNullOrEmpty(warpScene))
        {
            return;
        }

        // Start a coroutine to warp after the initial scene load is complete.
        __instance.StartCoroutine(WarpAfterDelay(__instance, warpScene));
    }

    private static IEnumerator WarpAfterDelay(GameManager gm, string warpScene)
    {
        // Wait for the initial scene to be fully loaded.
        yield return new WaitForSeconds(1f);

        Log.Info($"Warping to: {warpScene}");

        // Use BeginSceneTransition for proper scene loading.
        gm.BeginSceneTransition(
            new GameManager.SceneLoadInfo
            {
                SceneName = warpScene,
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
