using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(TraitSet), "GetTrait")]
    public static class TraitSet_GetTrait_Mechanicus_Patch
    {
        private static Trait transhumanist = new Trait(TraitDefOf.Transhumanist);
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isMechanicus())
                {
                    if (tDef == TraitDefOf.Transhumanist)
                    {
                        __result = transhumanist;
                    }
                }
            }
        }
    }
}
