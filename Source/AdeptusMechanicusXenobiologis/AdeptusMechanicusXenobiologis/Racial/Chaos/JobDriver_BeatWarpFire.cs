using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
    // Token: 0x0200004B RID: 75
    public class JobDriver_BeatWarpFire : JobDriver
    {
        // Token: 0x17000086 RID: 134
        // (get) Token: 0x06000268 RID: 616 RVA: 0x00017097 File Offset: 0x00015497
        protected Warpfire TargetWarpfire
        {
            get
            {
                return (Warpfire)this.job.targetA.Thing;
            }
        }

        // Token: 0x06000269 RID: 617 RVA: 0x000170AE File Offset: 0x000154AE
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        // Token: 0x0600026A RID: 618 RVA: 0x000170B4 File Offset: 0x000154B4
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            Toil beat = new Toil();
            Toil approach = new Toil();
            approach.initAction = delegate ()
            {
                if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetWarpfire, 1, -1, null, false))
                {
                    this.pawn.Reserve(this.TargetWarpfire, this.job, 1, -1, null, true);
                }
                this.pawn.pather.StartPath(this.TargetWarpfire, PathEndMode.Touch);
            };
            approach.tickAction = delegate ()
            {
                if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetWarpfire.Position)
                {
                    this.StartBeatingWarpfireIfAnyAt(this.pawn.pather.nextCell, beat);
                }
                if (this.pawn.Position != this.TargetWarpfire.Position)
                {
                    this.StartBeatingWarpfireIfAnyAt(this.pawn.Position, beat);
                }
            };
            approach.FailOnDespawnedOrNull(TargetIndex.A);
            approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            approach.atomicWithPrevious = true;
            yield return approach;
            beat.tickAction = delegate ()
            {
                if (!this.pawn.CanReachImmediate(this.TargetWarpfire, PathEndMode.Touch))
                {
                    this.JumpToToil(approach);
                }
                else
                {
                    if (this.pawn.Position != this.TargetWarpfire.Position && this.StartBeatingWarpfireIfAnyAt(this.pawn.Position, beat))
                    {
                        return;
                    }
                    this.pawn.natives.TryBeatFire(this.TargetWarpfire);
                    if (this.TargetWarpfire.Destroyed)
                    {
                        this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
                        this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
                        return;
                    }
                }
            };
            beat.FailOnDespawnedOrNull(TargetIndex.A);
            beat.defaultCompleteMode = ToilCompleteMode.Never;
            yield return beat;
            yield break;
        }

        // Token: 0x0600026B RID: 619 RVA: 0x000170D8 File Offset: 0x000154D8
        private bool StartBeatingWarpfireIfAnyAt(IntVec3 cell, Toil nextToil)
        {
            List<Thing> thingList = cell.GetThingList(base.Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                Warpfire warpfire = thingList[i] as Warpfire;
                if (warpfire != null && warpfire.parent == null)
                {
                    this.job.targetA = warpfire;
                    this.pawn.pather.StopDead();
                    base.JumpToToil(nextToil);
                    return true;
                }
            }
            return false;
        }
    }
}
