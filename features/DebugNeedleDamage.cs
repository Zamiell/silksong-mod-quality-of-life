namespace QoL.Features;

[HarmonyPatch(typeof(PlayerData), "nailDamage", MethodType.Getter)]
public class PlayerData_NailDamage_Patch
{
    static void Postfix(ref int __result)
    {
        if (!Config.DebugNeedleDamage.Value)
        {
            return;
        }

        __result = 999;
    }
}
