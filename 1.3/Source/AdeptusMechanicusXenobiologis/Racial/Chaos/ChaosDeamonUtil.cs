using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    internal static class ChaosDeamonUtil
    {

        static List<Pair<PawnKindDef, ChaosPatron>> ChaosDeamons = new List<Pair<PawnKindDef, ChaosPatron>>(){
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_ChaosHound, ChaosPatron.Undivided),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Fury, ChaosPatron.Undivided),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Deamonette, ChaosPatron.Slannesh),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Keeper_of_Secrets, ChaosPatron.Slannesh),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Screamer, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lord_of_Change, ChaosPatron.Tzeentch),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_KhorneHound, ChaosPatron.Khorne),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Bloodletter, ChaosPatron.Khorne),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Bloodthirster, ChaosPatron.Khorne),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Nurgling, ChaosPatron.Nurgle),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Plague_Bearer, ChaosPatron.Nurgle),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Great_Unclean_One, ChaosPatron.Nurgle),
        };
        
        static List<Pair<PawnKindDef, ChaosPatron>> GreaterDeamons = new List<Pair<PawnKindDef, ChaosPatron>>(){
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Keeper_of_Secrets, ChaosPatron.Slannesh),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lord_of_Change, ChaosPatron.Tzeentch),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Bloodthirster, ChaosPatron.Khorne),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Great_Unclean_One, ChaosPatron.Nurgle),
        };

        static List<Pair<PawnKindDef, ChaosPatron>> LesserDeamons = new List<Pair<PawnKindDef, ChaosPatron>>(){
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_ChaosHound, ChaosPatron.Undivided),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Fury, ChaosPatron.Undivided),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Deamonette, ChaosPatron.Slannesh),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone, ChaosPatron.Tzeentch),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Screamer, ChaosPatron.Tzeentch),
        //    new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_KhorneHound, ChaosPatron.Khorne),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Bloodletter, ChaosPatron.Khorne),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Nurgling, ChaosPatron.Nurgle),
            new Pair<PawnKindDef, ChaosPatron>(AdeptusPawnKindDefOf.OG_Chaos_Deamon_Plague_Bearer, ChaosPatron.Nurgle),
        };

        public static PawnKindDef LesserDeamon(out PawnKindDef pawn, ChaosPatron patron = ChaosPatron.Undivided)
        {
            pawn = LesserDeamons.FindAll(x=> x.Second == patron || patron == ChaosPatron.Undivided).RandomElement().First;
            return pawn;
        }
        public static PawnKindDef GreaterDeamon(out PawnKindDef pawn, ChaosPatron patron = ChaosPatron.Undivided)
        {
            pawn = GreaterDeamons.FindAll(x=> x.Second == patron || patron == ChaosPatron.Undivided).RandomElement().First;
            return pawn;
        }

        public static Faction OfDeamon()
        {
            return Find.FactionManager.FirstFactionOfDef(AdeptusFactionDefOf.OG_Chaos_Deamon_Faction);
        }

        public static Lord lord(Pawn p)
        {
            return LordUtility.GetLord(p);
        }
    }

    public enum ChaosPatron
    {
        Undivided,
        Khorne,
        Tzeentch,
        Slannesh,
        Nurgle
    }
}