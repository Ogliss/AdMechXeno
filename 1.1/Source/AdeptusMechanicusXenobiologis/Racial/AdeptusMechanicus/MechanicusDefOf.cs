using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    [DefOf]
    public static class MechanicusDefOf
    {
        static MechanicusDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MechanicusDefOf));
        }

        // AdeptusMechanicus ThingDefs
        public static ThingDef OG_Human_Mechanicus;

        // AdeptusMechanicus FactionDefs
        public static FactionDef OG_Mechanicus_Faction;

        // AdeptusMechanicus IncidentDefs
        //    public static IncidentDef ;

        // AdeptusMechanicus DamageDefs
        //    public static DamageDef ;

        // AdeptusMechanicus HediffDefs
        //    public static HediffDef ;

        // AdeptusMechanicus MentalStateDefs
        //    public static MentalStateDef ;

        // AdeptusMechanicus GameConditionDefs
        //    public static GameConditionDef ;

        // AdeptusMechanicus PawnKindDefs
        //    public static PawnKindDef ;
    }
}
