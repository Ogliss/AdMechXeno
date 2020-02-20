using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGTauDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGTauDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGTauDefOf));
        }

        // Tau ThingDefs
        public static ThingDef Alien_Tau;

        // Tau FactionDefs
        public static FactionDef OG_Tau_Faction;

        // Tau IncidentDefs
        //    public static IncidentDef ;

        // Tau DamageDefs
        //    public static DamageDef ;

        // Tau HediffDefs
        //    public static HediffDef ;

        // Tau MentalStateDefs
        //    public static MentalStateDef ;

        // Tau GameConditionDefs
        //    public static GameConditionDef ;

        // Tau PawnKindDefs
        //    public static PawnKindDef ;
    }
}
