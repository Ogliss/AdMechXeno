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
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef) })]
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef), typeof(int) })]
    public static class TraitSet_GetTrait_Mechanicus_Patch
    {
        private static Trait transhumanist = new Trait(TraitDefOf.Transhumanist);
        private static Trait tough = new Trait(TraitDefOf.Tough);
        private static Trait slowLearner = new Trait(AdeptusTraitDefOf.SlowLearner);
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.IsMechanicus())
                {
                    if (tDef == TraitDefOf.Transhumanist)
                    {
                        __result = transhumanist;
                    }
                }
                if (___pawn.isOgyrn())
                {
                    if (tDef == TraitDefOf.Tough)
                    {
                        __result = tough;
                    }
                    if (tDef == AdeptusTraitDefOf.SlowLearner)
                    {
                        __result = slowLearner;
                    }
                }
            }
        }
    }

}
