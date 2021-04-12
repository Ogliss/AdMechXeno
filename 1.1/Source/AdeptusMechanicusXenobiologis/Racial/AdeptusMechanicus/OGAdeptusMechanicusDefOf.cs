using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGAdeptusMechanicusDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGAdeptusMechanicusDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGAdeptusMechanicusDefOf));
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
