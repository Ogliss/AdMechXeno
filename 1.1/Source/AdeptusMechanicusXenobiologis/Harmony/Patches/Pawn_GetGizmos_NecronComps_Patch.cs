using System.Collections.Generic;
using Verse;
using HarmonyLib;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public static class Pawn_GetGizmos_NecronComps_Patch
    {
        [HarmonyPostfix]
        public static void Pawn_GizmosFrom_NecronComps_Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (__instance == null)
            {
                Log.Warning("Pawn_GizmosFromComps cannot access Apparel.");
                return;
            }
            if (__result == null)
            {
                Log.Warning("Pawn_GizmosFromComps creating new list.");
                return;
            }

            // Find all comps on the apparel. If any have gizmos, add them to the result returned from apparel already (typically empty set).
            List<Gizmo> l = new List<Gizmo>(__result);

            for (int o = 0; o < __instance.AllComps.Count; o++)
            {
                Comp_NecronOG _Necron;
                if ((_Necron = __instance.TryGetComp<Comp_NecronOG>()) != null)
                {
                    foreach (Gizmo gizmo in _Necron.CompGetGizmosExtra())
                    {
                        l.Add(gizmo);
                    }
                }
                /*
                HediffComp_VerbGiverExtra _VerbGiverExtra;
                if ((_VerbGiverExtra = __instance.health.hediffSet.hediffs[o].TryGetComp<HediffComp_VerbGiverExtra>()) != null)
                {
                    foreach (Gizmo gizmo in _VerbGiverExtra.GetVerbsCommands())
                    {
                        l.Add(gizmo);
                    }
                }
                */
            }
            __result = l;
        }
    }

}
