using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;
using RimWorld;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobDriver_XBDeconstruct
    public class JobDriver_XBDeconstruct : JobDriver
    {

        private float workLeft;

        private float totalNeededWork;

        private const int MaxDeconstructWork = 3000;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.Target, this.job, 1, -1, null);
        }

        protected Thing Target
        {
            get
            {
                return this.job.targetA.Thing;
            }
        }

        protected Building Building
        {
            get
            {
                return (Building)this.Target.GetInnerIfMinified();
            }
        }

        protected void TickAction()
        {
        }

        protected int TotalNeededWork
        {
            get
            {
                Building building = Building;
                int value = Mathf.RoundToInt(building.GetStatValue(StatDefOf.WorkToBuild, true));
                return Mathf.Clamp(value, 20, 3000);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
            Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
        }

        [DebuggerHidden]
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnForbidden(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil doWork = new Toil().FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            doWork.initAction = delegate
            {
                this.totalNeededWork = (float)this.TotalNeededWork;
                this.workLeft = this.totalNeededWork;
            };
            doWork.tickAction = delegate
            {
                this.workLeft -= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true);
                this.TickAction();
                if (this.workLeft <= 0f)
                {
                    this.CurToil.actor.jobs.curDriver.ReadyForNextToil();
                }
            };
            doWork.defaultCompleteMode = ToilCompleteMode.Never;
            doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / this.totalNeededWork, false, -0.5f);
            yield return doWork;
            yield return new Toil
            {
                initAction = delegate
                {
                    this.Target.Destroy(DestroyMode.Deconstruct);
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }


    }
}
