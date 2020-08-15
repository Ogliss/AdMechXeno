using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGEldarDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGEldarDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGEldarDefOf));
        }

        // Eldar ThingDefs
        //    public static ThingDef ;

        // Eldar ThingDefs Races
        public static ThingDef OG_Eldar_Wraithguard_Race;
        public static ThingDef OG_Alien_Eldar;

        // Eldar FactionDefs
        public static FactionDef OG_Eldar_Craftworld_Faction;
        /*
        public static FactionDef OG_Eldar_Exodite_Faction;
        public static FactionDef OG_Eldar_Harlequinn_Faction;
        */

        // Eldar IncidentDefs
        //    public static IncidentDef ;

        // Eldar DamageDefs
        //    public static DamageDef ;

        // Eldar HediffDefs
        //    public static HediffDef ;

        // Eldar MentalStateDefs
        //    public static MentalStateDef ;

        // Eldar GameConditionDefs
        //    public static GameConditionDef ;

        // Eldar PawnKindDefs
        public static PawnKindDef OG_Eldar_Wraithguard;
        public static PawnKindDef OG_Eldar_Wraithblade;
        public static PawnKindDef OG_Eldar_Guardian;
        public static PawnKindDef OG_Eldar_GuardianStorm;
        public static PawnKindDef OG_Eldar_GuardianSupport;
        public static PawnKindDef OG_Eldar_Ranger;
        public static PawnKindDef OG_Eldar_DireAvenger;
        public static PawnKindDef OG_Eldar_HowlingBanshee;
        public static PawnKindDef OG_Eldar_Warlock;
        public static PawnKindDef OG_Eldar_Farseer;
        public static PawnKindDef OG_Eldar_Farseer_Faction;
    }
}
