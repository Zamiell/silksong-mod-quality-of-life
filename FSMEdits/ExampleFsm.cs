namespace QoL.FSMEdits;

internal static class ExampleFsm
{
    // This is an example FSM edit.
    // To create your own FSM edits:
    // 1. Create a new static class in the FSMEdits directory.
    // 2. Create a static method that takes a PlayMakerFSM parameter.
    // 3. Check if the FSM matches what you want to edit (by name, game object, scene, etc.).
    // 4. Use the FsmUtil extension methods to modify the FSM.
    // 5. Add the method to the edits array in PlayMakerFSMPatch.cs.
    internal static void EditExample(PlayMakerFSM fsm)
    {
        // Example: Check if this is the FSM we want to edit.
        if (fsm is not { FsmName: "Example FSM Name", gameObject.name: "Example Object Name" })
            return;

        // Example: Check for a config setting.
        // if (!Config.ExampleFeature.Value)
        //     return;

        Log.Info("Modifying Example FSM");

        // Example FsmUtil operations:

        // Disable an action at a specific index in a state.
        // fsm.DisableAction("State Name", 0);

        // Get and modify an action.
        // var wait = fsm.GetAction<Wait>("State Name", 0);
        // wait.time = 0f;

        // Change a transition.
        // fsm.ChangeTransition("State Name", "Event Name", "New Target State");

        // Add a method to be called when entering a state.
        // fsm.AddMethod("State Name", (_) => {
        //     Log.Info("Entered state!");
        // });

        // Replace an action.
        // fsm.ReplaceAction("State Name", 0, new Wait() { time = 1f });

        // Insert a method at a specific index.
        // fsm.InsertMethod("State Name", 0, (_) => {
        //     Log.Info("Before first action!");
        // });

        // Disable multiple actions.
        // fsm.DisableActions("State Name", 0, 1, 2);
    }
}
