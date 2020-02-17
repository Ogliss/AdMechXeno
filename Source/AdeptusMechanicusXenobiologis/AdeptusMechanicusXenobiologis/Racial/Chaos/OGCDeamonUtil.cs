using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    static class OGCDeamonUtil
    {
        public static PawnKindDef LesserDeamon(out PawnKindDef pawn)
        {
            List<PawnKindDef> list = new List<PawnKindDef>();
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Deamonette);
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink);
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue);
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone);
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Screamer);
            list.Add(OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Nurgling);
            pawn = list.RandomElement();
            return pawn;
        }


        public static Faction OfDeamon()
        {
            return Find.FactionManager.FirstFactionOfDef(OGChaosDeamonDefOf.OG_Chaos_Deamon_Faction);
        }

        public static Lord lord(Pawn p)
        {
            return LordUtility.GetLord(p);
        }
    }
}