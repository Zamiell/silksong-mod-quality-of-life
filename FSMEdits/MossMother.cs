using HutongGames.PlayMaker.Actions;

namespace QoL.FSMEdits;

internal static class MossMother
{
    private const float SpeedMultiplier = 2f;

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
        if (fsm is { gameObject.name: "Mossbone Mother", FsmName: "Control" })
        {
            fsm.ChangeTransition("Burst Out", "FINISHED", "Roar End");
        }

        if (fsm is { gameObject.name: "Mossbone Mother Corpse(Clone)", FsmName: "Death" })
        {
            fsm.ChangeTransition("Stagger", "FINISHED", "Blow");
        }
    }
}

/*

This is the normal sequence for Moss Mother:

```
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Dormant
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Start Battle
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Short Pause
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Rumble
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Burst Out
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Roar
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Reset Bind Prompt?
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Roar End
[Info   : Unity Log] QoL: [FSM State] Mossbone Mother :: Control -> Idle
```

This is the normal sequence for the corpse:

```
[Info   : Unity Log] [2025-10-30 17:05:37.033] QoL: [FSM State] Mossbone Mother Corpse(Clone) :: Death -> Stagger
[Info   : Unity Log] [2025-10-30 17:05:38.578] QoL: [FSM State] Mossbone Mother Corpse(Clone) :: Death -> Steam
[Info   : Unity Log] [2025-10-30 17:05:42.125] QoL: [FSM State] Mossbone Mother Corpse(Clone) :: Death -> Blow
```

This is the normal sequence for the green vines that block the boss arena exit:

```
[Info   : Unity Log] [2025-10-30 17:05:32.531] QoL: [FSM State] Battle Gate Mossbone :: BG Control -> Close
[Info   : Unity Log] [2025-10-30 17:05:32.533] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Close
[Info   : Unity Log] [2025-10-30 17:05:32.843] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Closed
[Info   : Unity Log] [2025-10-30 17:05:32.844] QoL: [FSM State] Battle Gate Mossbone :: BG Control -> Closed
[Info   : Unity Log] [2025-10-30 17:05:39.727] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Tinked
[Info   : Unity Log] [2025-10-30 17:05:39.728] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Closed
[Info   : Unity Log] [2025-10-30 17:05:40.181] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Tinked
[Info   : Unity Log] [2025-10-30 17:05:40.182] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Closed
[Info   : Unity Log] [2025-10-30 17:05:40.581] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Tinked
[Info   : Unity Log] [2025-10-30 17:05:40.582] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Closed
[Info   : Unity Log] [2025-10-30 17:05:48.486] QoL: [FSM State] Battle Gate Mossbone :: BG Control -> Open
[Info   : Unity Log] [2025-10-30 17:05:48.488] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Open
[Info   : Unity Log] [2025-10-30 17:05:48.747] QoL: [FSM State] Battle Gate Mossbone :: BG Control -> Opened
[Info   : Unity Log] [2025-10-30 17:05:48.749] QoL: [FSM State] Battle Gate Mossbone (1) :: BG Control -> Opened
```

*/
