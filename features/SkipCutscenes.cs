using System.Collections;
using HarmonyLib;
using UnityEngine;

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
/// Patch to skip the cutscene where you meet the Church Maid.
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

        PlayerData.instance.churchKeeperIntro = true;
    }
}
