namespace QoL.Features;

// Mark all main areas as visited to skip area intro screens.
[HarmonyPatch(typeof(GameManager), "StartNewGame")]
public class SkipAreaIntro_StartNewGame_Patch
{
    static void Postfix()
    {
        if (!Config.SkipAreaIntro.Value)
        {
            return;
        }

        // Moss Grotto
        PlayerData.instance.visitedMossCave = true;

        // Bone Bottom
        PlayerData.instance.visitedBoneBottom = true;

        // Only add things in here that need to be set to avoid the text. Otherwise, we can get odd
        // side effects like the Lace 1 boss fight already being cleared just by having visited an
        // area.
    }
}
