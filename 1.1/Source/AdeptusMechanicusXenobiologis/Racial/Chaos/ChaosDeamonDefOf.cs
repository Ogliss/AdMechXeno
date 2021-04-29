using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    [DefOf]
    public static class ChaosDeamonDefOf
    {
        static ChaosDeamonDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ChaosDeamonDefOf));
        }

        // Deamon ThingDefs
        public static ThingDef OG_Chaos_Deamon_WarpTunnel;

        // Deamon FleshTypeDefs
        public static FleshTypeDef OG_Flesh_Chaos_Deamon;

        // Deamon FactionDefs
        public static FactionDef OG_Chaos_Deamon_Faction;

        public static IncidentDef OG_Chaos_Deamon_Warpstorm_Deamonic;
        public static IncidentDef OG_Chaos_Deamon_Deamonic_Incursion;
        public static IncidentDef OG_Chaos_Deamon_Daemonic_Infestation;

        // Deamon DamageDefs
        public static DamageDef OG_Chaos_Deamon_Warpfire;
        public static DamageDef OG_Chaos_Deamon_WarpfireDeath;

        // Deamon HediffDefs
        //    public static HediffDef ;

        // Deamon MentalStateDefs
        public static MentalStateDef OG_MentalState_ChaosDeamon;

        // Deamon PawnKindDefs
        // Deamons of Tzeentch
        public static PawnKindDef OG_Chaos_Deamon_Flamer_Exalted; 
        public static PawnKindDef OG_Chaos_Deamon_Flamer;
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Horror_Pink;
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Horror_Blue;
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Horror_Brimstone;
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Screamer;
        public static PawnKindDef OG_Chaos_Deamon_Lord_of_Change; 

        // Deamons of Nurgle
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Nurgling;
        public static PawnKindDef OG_Chaos_Deamon_Plague_Bearer;
        public static PawnKindDef OG_Chaos_Deamon_Great_Unclean_One;

        // Deamons of Slaanesh
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Deamonette;
        //   public static PawnKindDef OG_Chaos_Deamon_Mounted_Deamonette;
        public static PawnKindDef OG_Chaos_Deamon_Keeper_of_Secrets;

        // Deamons of Khrone
        //   public static PawnKindDef OG_Chaos_Deamon_Hound_of_Khorne;
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Bloodletter;
        //   public static PawnKindDef OG_Chaos_Deamon_Bloodthirster;
        //   public static PawnKindDef OG_Chaos_Deamon_Juggernaught_of_Khorne;

    }
}
