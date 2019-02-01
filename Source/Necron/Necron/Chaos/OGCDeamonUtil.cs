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
            list.Add(OGChaosDeamonDefOf.ChaosDeamonDeamonette);
            list.Add(OGChaosDeamonDefOf.ChaosDeamonHorrorPink);
            list.Add(OGChaosDeamonDefOf.ChaosDeamonHorrorBlue);
            list.Add(OGChaosDeamonDefOf.ChaosDeamonHorrorBrimstone);
            list.Add(OGChaosDeamonDefOf.ChaosDeamonScreamer);
            list.Add(OGChaosDeamonDefOf.ChaosDeamonNurgling);
            pawn = list.RandomElement();
            return pawn;
        }


        public static Faction OfDeamon()
        {
            return Find.FactionManager.FirstFactionOfDef(OGChaosDeamonDefOf.OGChaosDeamonFaction);
        }

        public static Lord lord(Pawn p)
        {
            return LordUtility.GetLord(p);
        }
    }
}