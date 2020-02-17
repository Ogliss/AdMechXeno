using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGChaosDeamonDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGChaosDeamonDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGChaosDeamonDefOf));
        }

        // Deamon ThingDefs
        public static ThingDef OG_Chaos_Deamon_WarpTunnel;

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

        // Deamons of Nurgle
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Nurgling;
        public static PawnKindDef OG_Chaos_Deamon_Plague_Bearer;
        //   public static PawnKindDef OG_Chaos_Deamon_Great_Unclean_One;

        // Deamons of Slaanesh
        public static PawnKindDef OG_Chaos_Deamon_Lessar_Deamonette;
        //   public static PawnKindDef OG_Chaos_Deamon_Mounted_Deamonette;
        //   public static PawnKindDef OG_Chaos_Deamon_Keeper_Of_Secrets;

        // Deamons of Khrone
        //   public static PawnKindDef OG_Chaos_Deamon_Hound_of_Khorne;
        //   public static PawnKindDef OG_Chaos_Deamon_Bloodletter;
        //   public static PawnKindDef OG_Chaos_Deamon_Bloodthirster;
        //   public static PawnKindDef OG_Chaos_Deamon_Juggernaught_of_Khorne;

    }
}
