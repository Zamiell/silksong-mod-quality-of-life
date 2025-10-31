using RacingMod.FSMEdits;

namespace RacingMod.Patches;

[HarmonyPatch(typeof(PlayMakerFSM), nameof(PlayMakerFSM.Start))]
internal static class PlayMakerFSMPatch
{
    private static readonly Action<PlayMakerFSM>[] edits =
    [
        // Bosses
        MossMother.SpeedUp,
        // Other
        ShrineWeaverAbility.Skip,
    ];

    [HarmonyPostfix]
    private static void Postfix(PlayMakerFSM __instance)
    {
        if (Config.LogFSMDetails.Value)
        {
            // LogFSMInfo(__instance);
            AddStateTransitionLogging(__instance);
        }

        foreach (Action<PlayMakerFSM> edit in edits)
        {
            try
            {
                edit.Invoke(__instance);
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Exception thrown when editing FSM {__instance.FsmName} on {__instance.name}"
                );
                Log.Error(e.ToString());
            }
        }
    }

    private static void LogFSMInfo(PlayMakerFSM fsm)
    {
        try
        {
            string sceneName = fsm.gameObject.scene.name;
            string gameObjectName = fsm.gameObject.name;
            string fsmName = fsm.FsmName;

            Log.Info($"=== FSM Details ===");
            Log.Info($"Scene: {sceneName}");
            Log.Info($"GameObject: {gameObjectName}");
            Log.Info($"FSM Name: {fsmName}");

            if (fsm.Fsm != null && fsm.Fsm.States != null)
            {
                Log.Info($"States ({fsm.Fsm.States.Length}):");
                foreach (var state in fsm.Fsm.States)
                {
                    int actionCount = state.Actions?.Length ?? 0;
                    int transitionCount = state.Transitions?.Length ?? 0;
                    Log.Info(
                        $"  - {state.Name} ({actionCount} actions, {transitionCount} transitions)"
                    );

                    // Log transitions for each state.
                    if (transitionCount > 0 && state.Transitions != null)
                    {
                        foreach (var transition in state.Transitions)
                        {
                            string eventName = transition.EventName ?? "(null)";
                            string toState = transition.ToState ?? "(null)";
                            Log.Info($"    -> {eventName} => {toState}");
                        }
                    }
                }
            }

            Log.Info($"===================");
        }
        catch (Exception e)
        {
            Log.Error($"Error logging FSM info: {e}");
        }
    }

    private static void AddStateTransitionLogging(PlayMakerFSM fsm)
    {
        try
        {
            string gameObjectName = fsm.gameObject.name;
            string fsmName = fsm.FsmName;

            // Add logging at the start of each state.
            if (fsm.Fsm != null && fsm.Fsm.States != null)
            {
                foreach (var state in fsm.Fsm.States)
                {
                    string stateName = state.Name;
                    fsm.InsertMethod(
                        stateName,
                        0,
                        (_) =>
                        {
                            Log.Info($"[FSM State] {gameObjectName} :: {fsmName} -> {stateName}");
                        }
                    );
                }
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error adding state transition logging: {e}");
        }
    }
}
