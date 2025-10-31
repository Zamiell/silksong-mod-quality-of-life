using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RacingMod.Features;

// Make the menu text fade in instantly.
[HarmonyPatch(typeof(UIManager), "Start")]
public class UIManagerStartPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(UIManager __instance)
    {
        if (!Config.FastMainMenu.Value)
        {
            return;
        }

        __instance.MENU_FADE_SPEED = 100f; // The normal value is 3.2f.
    }
}

// Make the background image fade in instantly.
[HarmonyPatch(typeof(UIManager), "FadeScreenBlankerTo")]
public class UIManagerFadeScreenBlankerToPatch
{
    [HarmonyWrapSafe, HarmonyPrefix]
    private static void Prefix(ref float duration)
    {
        if (!Config.FastMainMenu.Value)
        {
            return;
        }

        duration = 0f;
    }
}

// Make the menu text appear instantly. (Normally, it only starts fading in after a second.)
[HarmonyPatch(typeof(UIManager), "FadeScreenIn")]
public class UIManagerFadeScreenInPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(ref float __result)
    {
        if (!Config.FastMainMenu.Value)
        {
            return;
        }

        __result = 0f;
    }
}

// Make the save slot selection screen appear instantly instead of animating in one by one.
[HarmonyPatch]
public class UIManagerGoToProfileMenuPatch
{
    private static YieldInstruction ConditionalWait(float seconds)
    {
        if (Config.FastMainMenu.Value)
        {
            return null!;
        }

        return new WaitForSeconds(seconds);
    }

    [HarmonyTargetMethod]
    private static MethodBase TargetMethod()
    {
        var method = AccessTools.Method(typeof(UIManager), "GoToProfileMenu");
        var nestedTypes = typeof(UIManager).GetNestedTypes(
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        foreach (var nestedType in nestedTypes)
        {
            if (nestedType.Name.Contains("GoToProfileMenu"))
            {
                var moveNext = AccessTools.Method(nestedType, "MoveNext");
                if (moveNext != null)
                {
                    return moveNext;
                }
            }
        }

        Log.Error("Could not find MoveNext method for GoToProfileMenu coroutine");
        return method;
    }

    [HarmonyWrapSafe, HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        var codes = new List<CodeInstruction>(instructions);
        var conditionalWaitMethod = AccessTools.Method(
            typeof(UIManagerGoToProfileMenuPatch),
            nameof(ConditionalWait)
        );

        int replacementCount = 0;

        // Replace all "WaitForSeconds(0.165f)" calls with "ConditionalWait(0.165f)".
        for (int i = 0; i < codes.Count; i++)
        {
            // Look for the pattern: ldc.r4 0.165 followed by newobj WaitForSeconds
            if (i < codes.Count - 1 && codes[i].opcode == OpCodes.Ldc_R4)
            {
                if (codes[i].operand is float f && f == 0.165f)
                {
                    if (
                        codes[i + 1].opcode == OpCodes.Newobj
                        && codes[i + 1].operand is ConstructorInfo ctor
                        && ctor.DeclaringType?.Name == "WaitForSeconds"
                    )
                    {
                        // Replace newobj WaitForSeconds with call ConditionalWait.
                        codes[i + 1].opcode = OpCodes.Call;
                        codes[i + 1].operand = conditionalWaitMethod;
                        replacementCount++;
                    }
                }
            }
        }

        return codes.AsEnumerable();
    }
}

// Make the save slot selection screen hide instantly when going back.
[HarmonyPatch]
public class UIManagerHideSaveProfileMenuPatch
{
    // Takes `timeTool` as parameter to match the stack layout, but only uses seconds.
    private static IEnumerator ConditionalWaitTimeScaleIndependent(object timeTool, float seconds)
    {
        if (Config.FastMainMenu.Value)
        {
            yield break;
        }

        // Call the actual `TimeScaleIndependentWaitForSeconds` method on the provided `timeTool`.
        var timeToolType = timeTool.GetType();
        var method = timeToolType.GetMethod("TimeScaleIndependentWaitForSeconds");
        if (method != null)
        {
            var result = method.Invoke(timeTool, new object[] { seconds }) as IEnumerator;
            if (result != null)
            {
                yield return result;
            }
        }
    }

    // Target the compiler-generated MoveNext method of the coroutine state machine.
    [HarmonyTargetMethod]
    private static MethodBase TargetMethod()
    {
        var method = AccessTools.Method(typeof(UIManager), "HideSaveProfileMenu");

        // Find the nested compiler-generated class.
        var nestedTypes = typeof(UIManager).GetNestedTypes(
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        foreach (var nestedType in nestedTypes)
        {
            if (nestedType.Name.Contains("HideSaveProfileMenu"))
            {
                var moveNext = AccessTools.Method(nestedType, "MoveNext");
                if (moveNext != null)
                {
                    return moveNext;
                }
            }
        }

        Log.Error("Could not find MoveNext method for HideSaveProfileMenu coroutine");
        return method;
    }

    [HarmonyWrapSafe, HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(
        IEnumerable<CodeInstruction> instructions
    )
    {
        var codes = new List<CodeInstruction>(instructions);
        var conditionalWaitMethod = AccessTools.Method(
            typeof(UIManagerHideSaveProfileMenuPatch),
            nameof(ConditionalWaitTimeScaleIndependent)
        );

        int replacementCount = 0;

        // Replace all `TimeScaleIndependentWaitForSeconds` calls with our conditional version.
        for (int i = 0; i < codes.Count; i++)
        {
            // Look for calls to `TimeScaleIndependentWaitForSeconds`.
            if (
                codes[i].opcode == OpCodes.Callvirt
                && codes[i].operand is MethodInfo method
                && method.Name == "TimeScaleIndependentWaitForSeconds"
            )
            {
                // Replace `Callvirt` with `Call` since our method is static.
                // The stack layout matches [timeTool instance, float] for both methods.
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = conditionalWaitMethod;
                replacementCount++;
            }
        }

        return codes.AsEnumerable();
    }
}
