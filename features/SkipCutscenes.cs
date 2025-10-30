namespace QoL.Features;

/// <summary>
/// Patch to automatically skip the opening sequence cutscene.
/// </summary>
[HarmonyPatch(typeof(OpeningSequence), "Start")]
public class OpeningSequence_Start_Patch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static IEnumerator Postfix(IEnumerator result, OpeningSequence __instance)
    {
        // If the feature is disabled, just run the original coroutine.
        if (!Config.SkipCutscenes.Value)
        {
            while (result.MoveNext())
            {
                yield return result.Current;
            }
            yield break;
        }

        // Start a coroutine to continuously skip the chain sequence.
        __instance.StartCoroutine(ContinuouslySkip(__instance));

        // Run the original coroutine to handle all the loading and scene activation.
        while (result.MoveNext())
        {
            yield return result.Current;
        }
    }

    private static IEnumerator ContinuouslySkip(OpeningSequence instance)
    {
        // Wait a frame for initialization.
        yield return null;

        // Keep skipping until the opening sequence is done.
        for (int i = 0; i < 100; i++)
        {
            // Try to skip.
            yield return instance.Skip();

            // Wait a bit before trying to skip again.
            yield return new WaitForSeconds(0.1f);
        }
    }
}

/// <summary>
/// Patch to monitor scene loads and disable Act Card display in Opening_Sequence.
/// </summary>
[HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager), "Internal_ActiveSceneChanged")]
public class SceneManager_ActiveSceneChanged_Patch
{
    [HarmonyPostfix]
    private static void Postfix(in UnityEngine.SceneManagement.Scene newActiveScene)
    {
        if (!Config.SkipCutscenes.Value)
        {
            return;
        }

        if (newActiveScene.name == "Opening_Sequence")
        {
            // Start a coroutine to search for and disable the "Act Card" GameObject.
            GameManager.instance.StartCoroutine(DisableActCard());
        }
    }

    private static System.Collections.IEnumerator DisableActCard()
    {
        // Search for the "Act Card" GameObject for up to 15 seconds.
        float elapsed = 0f;

        while (elapsed < 15f)
        {
            // Use GameObject.Find to search for the "Act Card" GameObject by name.
            var actCard = GameObject.Find("Act Card");
            if (actCard != null)
            {
                actCard.SetActive(false);
                yield break;
            }

            yield return new UnityEngine.WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
    }
}

/// <summary>
/// Patch to skip certain cutscenes by setting save file flags.
/// </summary>
[HarmonyPatch(typeof(GameManager), "StartNewGame")]
public class GameManager_StartNewGame_Patch_2
{
    static void Postfix()
    {
        if (!Config.HornetQuickStart.Value)
        {
            return;
        }

        // Skips the Chapel Maid "weakness" cutscene.
        PlayerData.instance.churchKeeperIntro = true;

        // Skips Shakra talking to you for the first time in The Marrow.
        PlayerData.instance.metMapper = true; //
    }
}

/// <summary>
/// Patch to automatically skip certain cutscene scenes when they are loaded.
/// </summary>
[HarmonyPatch(typeof(CutsceneHelper), "Start")]
public class CutsceneHelper_Start_Patch
{
    private static readonly string[] CutscenesToSkip =
    {
        // "Bone_East_Umbrella",
        // "Belltown",
        // "Room_Pinstress",
        // "Belltown_Room_pinsmith",
        // "Belltown_Room_doctor",
        // "End_Credits_Scroll",
        // "End_Credits",
        // "Menu_Credits",
        // "End_Game_Completion",
        // "PermaDeath",
        // "Bellway_City",
        // "City_Lace_cutscene",
    };

    [HarmonyWrapSafe, HarmonyPostfix]
    private static IEnumerator Postfix(IEnumerator result, CutsceneHelper __instance)
    {
        if (!Config.SkipCutscenes.Value)
        {
            // If the feature is disabled, just run the original coroutine.
            while (result.MoveNext())
            {
                yield return result.Current;
            }
            yield break;
        }

        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        bool shouldSkip = false;

        foreach (string sceneName in CutscenesToSkip)
        {
            if (currentScene == sceneName)
            {
                shouldSkip = true;
                break;
            }
        }

        if (shouldSkip)
        {
            // Wait for one frame to let the scene initialize.
            yield return null;

            // Skip the cutscene immediately.
            yield return __instance.Skip();
        }
        else
        {
            // If not a scene we want to skip, run the original coroutine.
            while (result.MoveNext())
            {
                yield return result.Current;
            }
        }
    }
}
