using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QoL;

internal static class OnSceneLoadPatch
{
    internal static void OnSceneLoad(Scene scene, LoadSceneMode lsm)
    {
        SkipWeakness(scene.name);
        SkipChapelMaidIntro(scene.name);
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

    private static void SkipChapelMaidIntro(string sceneName)
    {
        if (sceneName == "Bonetown" && !PlayerData.instance.churchKeeperIntro)
        {
            PlayerData.instance.churchKeeperIntro = true;

            StartCoroutine(
                () =>
                {
                    GameObject
                        .Find("Churchkeeper Intro Scene")
                        .LocateMyFSM("Control")
                        .SetState("Set End");
                },
                0.3f
            );
        }
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
