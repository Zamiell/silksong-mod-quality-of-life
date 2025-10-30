using HutongGames.PlayMaker.Actions;

namespace QoL.FSMEdits;

internal static class MossMother
{
    private const float SpeedMultiplier = 5f;

    // Intro sequence states that should be sped up.
    private static readonly string[] IntroStates =
    {
        "Return Pause",
        "Return Antic",
        "Return In",
        "Roar",
        "Reset Bind Prompt?",
        "Roar End",
    };

    internal static void SpeedUp(PlayMakerFSM fsm)
    {
        if (fsm is not { gameObject.name: "Mossbone Mother", FsmName: "Control" })
        {
            return;
        }

        Log.Info($"Speeding up Moss Mother intro sequence");

        var animator = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        float[]? originalFps = null;

        // Speed up the tk2dSpriteAnimator if available.
        if (animator?.Library?.clips != null)
        {
            // Store original FPS values so we can restore them later.
            originalFps = new float[animator.Library.clips.Length];
            for (int i = 0; i < animator.Library.clips.Length; i++)
            {
                var clip = animator.Library.clips[i];
                if (clip != null)
                {
                    originalFps[i] = clip.fps;
                    clip.fps *= SpeedMultiplier;
                }
            }

            Log.Info("Sped up Moss Mother animation");
        }

        // Iterate through intro states and speed up timing actions.
        if (fsm.Fsm?.States == null)
        {
            return;
        }

        foreach (var state in fsm.Fsm.States)
        {
            if (state?.Actions == null || state.Name == null)
            {
                continue;
            }

            // Only modify intro states.
            bool isIntroState = false;
            foreach (var introState in IntroStates)
            {
                if (state.Name == introState)
                {
                    isIntroState = true;
                    break;
                }
            }

            if (!isIntroState)
            {
                continue;
            }

            for (int i = 0; i < state.Actions.Length; i++)
            {
                var action = state.Actions[i];

                // Speed up Wait actions.
                if (action is Wait wait)
                {
                    wait.time.Value /= SpeedMultiplier;
                }
                // Speed up RandomWait actions.
                else if (action is RandomWait randomWait)
                {
                    randomWait.min.Value /= SpeedMultiplier;
                    randomWait.max.Value /= SpeedMultiplier;
                }
            }
        }

        // Add a method to restore animation speed when entering Idle state.
        if (animator?.Library?.clips != null && originalFps != null)
        {
            fsm.InsertMethod(
                "Idle",
                0,
                (_) =>
                {
                    // Restore original animation speeds.
                    for (int i = 0; i < animator.Library.clips.Length; i++)
                    {
                        var clip = animator.Library.clips[i];
                        if (clip != null && i < originalFps.Length)
                        {
                            clip.fps = originalFps[i];
                        }
                    }
                    Log.Info("Restored Moss Mother animation speeds");
                }
            );
        }

        Log.Info($"Finished speeding up Moss Mother intro sequence");
    }
}
