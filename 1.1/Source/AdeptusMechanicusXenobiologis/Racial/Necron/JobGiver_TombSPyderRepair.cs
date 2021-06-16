using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    public class JobGiver_TombSpyderRepair : ThinkNode_JobGiver
    {
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_TombSpyderRepair jobGiver_TombSpyderRepair = (JobGiver_TombSpyderRepair)base.DeepCopy(resolve);
            jobGiver_TombSpyderRepair.SearchRadius = this.SearchRadius;
            return jobGiver_TombSpyderRepair;
        }
        
        private float SearchRadius = 9999f;

        public override Job TryGiveJob(Pawn pawn)
        {
            float Searchradius = SearchRadius;
            IntVec3 c = IntVec3.Invalid;
            if (pawn.kindDef== AdeptusPawnKindDefOf.OG_Necron_Tomb_Spyder)
            {
                if (TombSpyderUtility.TryFindGoodRepairTarget(pawn, Searchradius, out Pawn pt, null) && !GenAI.InDangerousCombat(pawn))
                {
                    return new Job(AdeptusJobDefOf.OG_XB_Job_Necron_TombSpyderRepair)
                    {
                        targetA = pt,
                        count = 1
                    };
                }
                else
                {
                    if (TombSpyderUtility.TryFindGoodRessTarget(pawn, Searchradius, out Corpse ct, null) && !GenAI.InDangerousCombat(pawn))
                    {
                        return new Job(AdeptusJobDefOf.OG_XB_Job_Necron_TombSpyderRepair)
                        {
                            targetA = ct,
                            count = 1
                        };
                    }
                }
            }
            return null;
        }
        
        public const float VictimSearchRadiusInitial = 8f;
        private const float VictimSearchRadiusOngoing = 18f;
    }
}
