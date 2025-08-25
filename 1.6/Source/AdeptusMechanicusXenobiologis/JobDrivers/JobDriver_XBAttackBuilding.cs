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
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.numWPBuildingAttacksMade, "numWPBuildingAttacksMade", 0, false);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override IEnumerable<Toil> MakeNewToils()
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

        private int numWPBuildingAttacksMade;
    }
}
