using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobDriver_XBAttackBuilding
    public class JobDriver_XBAttackBuilding : JobDriver
    {
        //// Token: 0x06003B7D RID: 15229 RVA: 0x001BFF59 File Offset: 0x001BE359
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.numWPBuildingAttacksMade, "numWPBuildingAttacksMade", 0, false);
        }

        // Token: 0x06003B7E RID: 15230 RVA: 0x001BFF85 File Offset: 0x001BE385
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x06003B7F RID: 15231 RVA: 0x001BFF88 File Offset: 0x001BE388
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            Toil killthebuilding = new Toil
            {
                initAction = delegate
                {
                    Building pawn = this.TargetThingA as Building;
                },
                tickAction = delegate
                {
                    if (!this.TargetA.IsValid)
                    {
                        this.EndJobWith(JobCondition.Succeeded);
                        return;
                    }
                    if (this.TargetA.HasThing)
                    {
                        Building pawn = this.TargetA.Thing as Building;
                        if (this.TargetA.Thing.Destroyed)
                        {
                            this.EndJobWith(JobCondition.Succeeded);
                            return;
                        }
                    }
                    if (this.pawn.TryStartAttack(this.TargetA))
                    {
                        this.numWPBuildingAttacksMade++;
                    }
                    if (this.TargetA.Thing.Destroyed)
                    {
                        this.EndJobWith(JobCondition.Succeeded);
                        return;
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            yield return killthebuilding;
            yield break;
        }

        // Token: 0x04002639 RID: 9785
        private int numWPBuildingAttacksMade;
    }
}
