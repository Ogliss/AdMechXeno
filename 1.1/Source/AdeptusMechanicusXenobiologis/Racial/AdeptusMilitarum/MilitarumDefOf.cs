using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    [DefOf]
    public static class MilitarumDefOf
    {
        static MilitarumDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MilitarumDefOf));
        }

        // AdeptusMilitarum ThingDefs
        //    public static ThingDef ;

        // AdeptusMilitarum FactionDefs
        public static FactionDef OG_Militarum_Cadian_Faction;
        public static FactionDef OG_Militarum_PlayerColony;

        // AdeptusMilitarum IncidentDefs
        //    public static IncidentDef ;

        // AdeptusMilitarum DamageDefs
        //    public static DamageDef ;

        // AdeptusMilitarum HediffDefs
        //    public static HediffDef ;

        // AdeptusMilitarum MentalStateDefs
        //    public static MentalStateDef ;

        // AdeptusMilitarum GameConditionDefs
        //    public static GameConditionDef ;

        // AdeptusMilitarum PawnKindDefs
        //    public static PawnKindDef ;
    }
}
