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
        if (!Config.SkipCutscenes.Value)
        {
            // If the feature is disabled, just run the original coroutine.
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
