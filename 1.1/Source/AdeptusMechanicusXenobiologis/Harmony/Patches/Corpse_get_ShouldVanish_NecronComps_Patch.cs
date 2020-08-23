using Verse;
using HarmonyLib;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Corpse), "get_ShouldVanish")]
    public static class Corpse_get_ShouldVanish_NecronComps_Patch
    {
        [HarmonyPrefix]
        public static bool Corpse_get_ShouldVanish_NecronComps_Postfix(Corpse __instance, ref bool __result)
        {

            if (__instance == null)
            {
                __result = false;
                return false;
            }
            if (!__instance.Spawned)
            {
                __result = false;
                return false;
            }
            if (__instance.TryGetComp<Comp_NecronOG>() != null)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
    
}
