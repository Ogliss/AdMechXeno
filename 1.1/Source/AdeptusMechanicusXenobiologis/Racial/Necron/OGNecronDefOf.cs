using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGNecronDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGNecronDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGNecronDefOf));
        }
        
        public static FactionDef OG_Necron_Faction;

        public static ThingDef MonolithIncoming;
        public static ThingDef OG_FilthBlood_Necron;

        public static FleshTypeDef OG_Flesh_Construct_Necron;

        public static IncidentDef OGN_MonolithAppears;

        public static HediffDef OG_Necron_Upgrade_RessurectionOrb;
        public static HediffDef OG_Necron_Upgrade_Phylactery;
        public static HediffDef OG_Necron_Upgrade_VeilOfDarkness;
        public static HediffDef OG_Necron_Upgrade_PhaseShifter;

        public static JobDef OG_XB_Job_Necron_TombSpyderRepair;

        public static PawnKindDef OG_Necron_Scarab_Swarm;
        public static PawnKindDef OG_Necron_Flayed_One;
        public static PawnKindDef OG_Necron_Warrior;
        public static PawnKindDef OG_Necron_Wraith;
        public static PawnKindDef OG_Necron_Immortal;
        public static PawnKindDef OG_Necron_Tomb_Spyder;
        public static PawnKindDef OG_Necron_Destroyer;
        public static PawnKindDef OG_Necron_Destroyer_Heavy;
        public static PawnKindDef OG_Necron_Lord;

        public static ThingDef Necron_ScarabSwarm;
        public static ThingDef Necron_FlayedOne;
        public static ThingDef Necron_Warrior;
        public static ThingDef Necron_Wraith;
        public static ThingDef Necron_Immortal;
        public static ThingDef Necron_TombSpyder;
        public static ThingDef Necron_Destroyer;
        public static ThingDef Necron_HeavyDestroyer;
        public static ThingDef Necron_Lord;

        public static BodyPartDef OG_Necron_PhasicCapacitor;
        public static BodyPartDef OG_Necron_ReanimationMatrix;
        public static BodyPartDef OG_Necron_NecrodermisRegulator;
    }
}
