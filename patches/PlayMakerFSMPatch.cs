using QoL.FSMEdits;

namespace QoL.Patches;

[HarmonyPatch(typeof(PlayMakerFSM), nameof(PlayMakerFSM.Start))]
internal static class PlayMakerFSMPatch
{
    // Note: put all FSM edit methods here.
    // Feel free to ignore nullity checks as they will be caught here.
    private static readonly Action<PlayMakerFSM>[] edits =
    [
        // Add your FSM edit methods here, for example:
        // ExampleFsm.EditExample,
    ];

    [HarmonyPostfix]
    private static void Postfix(PlayMakerFSM __instance)
    {
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
}
