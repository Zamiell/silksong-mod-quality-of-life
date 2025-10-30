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
                var weaknessScene = GameObject.Find("Weakness Scene");
                weaknessScene?.SetActive(false);
            },
            0
        );
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
