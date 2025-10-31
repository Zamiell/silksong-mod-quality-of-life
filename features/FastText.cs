namespace RacingMod.Features;

[HarmonyPatch(typeof(DialogueBox), "Start")]
public class DialogueBoxPatch
{
    [HarmonyWrapSafe, HarmonyPostfix]
    private static void Postfix(DialogueBox __instance)
    {
        if (!Config.FastText.Value)
        {
            return;
        }

        // Create a Traverse object for the instance
        var traverse = Traverse.Create(__instance);

        // Get the value from the private field 'fastRevealSpeed'
        float originalSpeed = traverse.Field<float>("fastRevealSpeed").Value;

        // Calculate the new speed
        float newSpeed = originalSpeed * 50;

        // Set the values for all the private speed fields
        traverse.Field("fastRevealSpeed").SetValue(newSpeed);
        traverse.Field("regularRevealSpeed").SetValue(newSpeed);
        traverse.Field("currentRevealSpeed").SetValue(newSpeed);
        traverse.Field("animator").Property("speed").SetValue(10f);
    }
}
