using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QoL;

internal static class OnSceneLoadPatch
{
    internal static void OnSceneLoad(Scene scene, LoadSceneMode lsm)
    {
        LogSceneChange(scene, lsm);
        SkipWeakness(scene.name);
        ResetHornetPosition(scene.name);
    }

    public static void LogSceneChange(Scene scene, LoadSceneMode mode)
    {
        Log.Info($"Scene loaded: {scene.name} (Mode: {mode})");
    }

    private static void SkipWeakness(string sceneName)
    {
        if (!Config.SkipWeakness.Value)
        {
            return;
        }

        StartCoroutine(
            () =>
            {
                GameObject weaknessScene = GameObject.Find("Weakness Scene");

                /*
                if (sceneName == "Cog_09_Destroyed")
                {
                    weaknessScene = GameObject.Find("Weakness Cog Drop Scene");
                }
                */

                weaknessScene?.SetActive(false);
            },
            0.3f
        );
    }

    private static void ResetHornetPosition(string sceneName)
    {
        if (!Config.HornetQuickStart.Value)
        {
            return;
        }

        /*
        if (sceneName == "Tut_01" && !PlayerData.instance.scenesVisited.Contains(sceneName))
        {
            // After setting `bindCutscenePlayed` to true, Hornet will enter the scene as if she had
            // jumped down from Bone Bottom. Thus, we manually set her back to where she is supposed
            // to be.
            HeroController.instance.gameObject.transform.position = new Vector3(
                50.40f,
                4.55f,
                0.00f
            );

            // Play the getting up animation that would occur if we saved and quit after the bind cutscene was completed.
            // Because Hornet was mid-air when we moved her, she will still appear like she is
            // falling. Manually set her to a grounded state, which should hopefully reset her
            // sprite.
            HeroController.instance.StartCoroutine(PlayWakeUpAnimation());
        }
        */
    }

    private static IEnumerator PlayWakeUpAnimation()
    {
        HeroController hc = HeroController.instance;

        // Stop animation control and prevent player input during the wake-up sequence.
        hc.StopAnimationControl();
        hc.controlReqlinquished = true;

        // Get the duration of the wake-up animation.
        float clipLength = hc.AnimCtrl.GetClipDuration("Wake Up Ground");

        // Play the wake-up animation.
        hc.AnimCtrl.PlayClipForced("Wake Up Ground");

        // Wait for the animation to complete.
        tk2dSpriteAnimationClip clip = hc.AnimCtrl.animator.CurrentClip;
        if (clip != null)
        {
            while (hc.AnimCtrl.animator.IsPlaying(clip) && clipLength > 0f)
            {
                yield return null;
                clipLength -= Time.deltaTime;
            }
        }
        else
        {
            yield return new WaitForSeconds(clipLength);
        }

        // Restore animation control and player input.
        hc.StartAnimationControl();
        hc.controlReqlinquished = false;
    }

    private static IEnumerator Delay(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    private static void StartCoroutine(Action action, float seconds)
    {
        if (HeroController.UnsafeInstance == null)
        {
            return;
        }

        HeroController.instance.StartCoroutine(Delay(seconds, action));
    }
}
