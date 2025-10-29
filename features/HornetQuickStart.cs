using HarmonyLib;

namespace QoL.Features;

[HarmonyPatch(typeof(PlayMakerFSM), "SendEvent", typeof(string))]
public class PlayMakerFSM_SendEvent_Patch { }
