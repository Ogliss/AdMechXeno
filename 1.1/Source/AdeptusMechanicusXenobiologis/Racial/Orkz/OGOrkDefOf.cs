using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGOrkDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGOrkDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkDefOf));
        }

        // Ork ThingDefs
        public static ThingDef OG_Alien_Ork;
        public static ThingDef OG_Alien_Grot;
        
        public static FactionDef OG_Ork_Tek_Faction;
        public static FactionDef OG_Ork_Feral_Faction;
        public static FactionDef OG_Ork_Hulk;
        public static FactionDef OG_Ork_Rok;

        // Ork IncidentDefs
        public static IncidentDef OG_Ork_Rok_Crash;

        // Ork DamageDefs
        //    public static DamageDef ;

        // Ork HediffDefs
        //    public static HediffDef ;

        // Ork MentalStateDefs
        //    public static MentalStateDef ;

        // Ork GameConditionDefs
        //    public static GameConditionDef ;

        // Ork PawnKindDefs
        //    public static PawnKindDef ;
    }
}
