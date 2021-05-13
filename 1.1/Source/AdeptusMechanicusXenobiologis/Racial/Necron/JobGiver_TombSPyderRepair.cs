using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // Token: 0x020000D6 RID: 214
    public class JobGiver_TombSpyderRepair : ThinkNode_JobGiver
    {
        // Token: 0x0600041A RID: 1050 RVA: 0x0002C918 File Offset: 0x0002AD18
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_TombSpyderRepair jobGiver_TombSpyderRepair = (JobGiver_TombSpyderRepair)base.DeepCopy(resolve);
            jobGiver_TombSpyderRepair.SearchRadius = this.SearchRadius;
            return jobGiver_TombSpyderRepair;
        }
        
        private float SearchRadius = 9999f;

        // Token: 0x060004D1 RID: 1233 RVA: 0x0003100C File Offset: 0x0002F40C
        protected override Job TryGiveJob(Pawn pawn)
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
        
        // Token: 0x040002AB RID: 683
        public const float VictimSearchRadiusInitial = 8f;

        // Token: 0x040002AC RID: 684
        private const float VictimSearchRadiusOngoing = 18f;
    }
}
