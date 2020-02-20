using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Harmony;
using Verse.Sound;

namespace AdeptusMechanicus.Harmony
{
    [HarmonyPatch(typeof(Corpse), "get_ShouldVanish")]
    public static class AMXB_Corpse_get_ShouldVanish_NecronComps_Patch
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
