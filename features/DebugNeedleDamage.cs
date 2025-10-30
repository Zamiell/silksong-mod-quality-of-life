using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace QoL.Features;

[HarmonyPatch(typeof(DamageEnemies), "DoDamage", typeof(GameObject), typeof(bool))]
public class DamageEnemies_DoDamage_Patch
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var roundToIntMethod = AccessTools.Method(typeof(Mathf), nameof(Mathf.RoundToInt));

        // Find where Mathf.RoundToInt is called (this calculates num5, the final damage).
        // We want to inject code right after it's stored to override it to 999.
        for (int i = 0; i < codes.Count; i++)
        {
            // Look for the call to Mathf.RoundToInt followed by stloc (store local variable).
            if (
                codes[i].opcode == OpCodes.Call
                && codes[i].operand as MethodInfo == roundToIntMethod
            )
            {
                // The next instruction should be storing to num5 (a local variable).
                if (i + 1 < codes.Count && codes[i + 1].opcode == OpCodes.Stloc_S)
                {
                    var num5Local = codes[i + 1].operand;

                    // Create a label for the skip target.
                    var skipLabel = new Label();

                    // Insert our code right after the stloc instruction.
                    var injectedCode = new List<CodeInstruction>
                    {
                        // Check if DebugNeedleDamage is enabled.
                        new CodeInstruction(
                            OpCodes.Call,
                            AccessTools.PropertyGetter(
                                typeof(Config),
                                nameof(Config.DebugNeedleDamage)
                            )
                        ),
                        new CodeInstruction(
                            OpCodes.Callvirt,
                            AccessTools.PropertyGetter(
                                typeof(BepInEx.Configuration.ConfigEntry<bool>),
                                nameof(BepInEx.Configuration.ConfigEntry<bool>.Value)
                            )
                        ),
                        // If false, skip our override.
                        new CodeInstruction(OpCodes.Brfalse, skipLabel),
                        // Load 999 and store it in num5.
                        new CodeInstruction(OpCodes.Ldc_I4, 999),
                        new CodeInstruction(OpCodes.Stloc_S, num5Local),
                    };

                    // Add the label to the next instruction (skip target).
                    codes[i + 2].labels.Add(skipLabel);

                    codes.InsertRange(i + 2, injectedCode);
                    break;
                }
            }
        }

        return codes;
    }
}
