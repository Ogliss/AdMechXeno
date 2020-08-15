using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGTyranidDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGTyranidDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGTyranidDefOf));
        }

        // Tyranid ThingDefs
        public static ThingDef Alien_Tau;
        public static ThingDef OG_FilthBlood_Tyranid;

        // Tyranid FactionDefs
        public static FactionDef OG_Tyranid_Faction;

        // Tyranid IncidentDefs
        //    public static IncidentDef ;

        // Tyranid DamageDefs
        //    public static DamageDef ;

        // Tyranid HediffDefs
        //    public static HediffDef ;

        // Tyranid MentalStateDefs
        //    public static MentalStateDef ;

        // Tyranid GameConditionDefs
        //    public static GameConditionDef ;

        // Tyranid PawnKindDefs
        public static PawnKindDef OG_Tyranid_Ripper_Swarm;
        public static PawnKindDef OG_Tyranid_Hormagaunt;
        public static PawnKindDef OG_Tyranid_Termagant;
        public static PawnKindDef OG_Tyranid_Genestealer;
        public static PawnKindDef OG_Tyranid_Warrior;
        public static PawnKindDef OG_Tyranid_Gargoyle;
        public static PawnKindDef OG_Tyranid_Lictor;
        public static PawnKindDef OG_Tyranid_Ravener;
        public static PawnKindDef OG_Tyranid_Hive_Tyrant;
        public static PawnKindDef OG_Tyranid_Carnifex;
        public static PawnKindDef OG_Tyranid_Zoanthrope;

        // Tyranid FleshTypeDefs
        public static FleshTypeDef OG_Flesh_Tyranid;
    }
}
