using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    [DefOf]
    public static class XenobiologisDefOf
    {
        static XenobiologisDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(XenobiologisDefOf));
        }
        

        // ScenPartDefs
         public static ScenPartDef OG_Rule_EnforceFactionRelations;

        // HediffDefs
        // public static HediffDef OG_;

        // MentalStateDefs
        // public static MentalStateDef OG_MentalState_;
        

        //  PawnKindDefs
        // public static PawnKindDef OG_; 
    }

    [DefOf]
    public static class XenobiologisDutyDefOf
    {
        static XenobiologisDutyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(XenobiologisDutyDefOf));
        }

        //    public static DutyDef OGAMXB_DefendAndExpandHiveLike;
        //    public static DutyDef OGAMXB_DefendHiveLikeAggressively;
        //    public static DutyDef OGAMXB_AssaultColony;
    }

    [DefOf]
    public static class XenobiologisHediffDefOf
    {
        static XenobiologisHediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(XenobiologisHediffDefOf));
        }
        public static HediffDef MissingBodyPart;
        public static HediffDef OG_Regenerating;
        public static HediffDef OG_Regenerated;
    }

    [DefOf]
    public static class XenobiologisJobDefOf
    {
        static XenobiologisJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(XenobiologisJobDefOf));
        }
        public static JobDef OGAMXBDeconstruct;
        public static JobDef OGAMXBWaitBuilding;
        public static JobDef OGAMXBWaitCombatBuilding;
        public static JobDef OGAMXBAttackBuilding;
    }
}
