using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{

    public class JobDriver_OGAMXBWaitBuilding : JobDriver
    {
        private const int TargetSearchInterval = 4;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        public override string GetReport()
        {
            if (this.job.def != AMXenoBiologisJobDefOf.OGAMXBWaitCombatBuilding)
            {
                return base.GetReport();
            }
            if (this.pawn.RaceProps.Humanlike && this.pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Violent))
            {
                return "ReportStanding".Translate();
            }
            return base.GetReport();
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Toil wait = new Toil();
            wait.initAction = delegate
            {
                this.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
                this.pawn.pather.StopDead();
                this.CheckForAutoAttack();
            };
            wait.tickAction = delegate
            {
                if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
                {
                    Log.Error(this.pawn + " in eternal WaitCombat without being drafted.");
                    this.ReadyForNextToil();
                    return;
                }
                if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
                {
                    this.CheckForAutoAttack();
                }
            };
            this.DecorateWaitToil(wait);
            wait.defaultCompleteMode = ToilCompleteMode.Never;
            yield return wait;
        }

        public virtual void DecorateWaitToil(Toil wait)
        {
        }

        public override void Notify_StanceChanged()
        {
            if (this.pawn.stances.curStance is Stance_Mobile)
            {
                this.CheckForAutoAttack();
            }
        }

        private void CheckForAutoAttack()
        {
            if (this.pawn.Downed)
            {
                return;
            }
            if (this.pawn.stances.FullBodyBusy)
            {
                return;
            }
            bool flag = this.pawn.story == null || !this.pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Violent);
            bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Firefighting);
            if (flag || flag2)
            {
                Fire fire = null;
                for (int i = 0; i < 9; i++)
                {
                    IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
                    if (c.InBounds(this.pawn.Map))
                    {
                        List<Thing> thingList = c.GetThingList(base.Map);
                        for (int j = 0; j < thingList.Count; j++)
                        {
                            if (flag)
                            {
                                Building pawn2 = thingList[j] as Building;
                                if (pawn2 != null && pawn2.Faction.def.humanlikeFaction)
                                {
                                    //this.pawn.meleeVerbs.TryMeleeAttack(pawn2, null, false);

                                    new Job(AMXenoBiologisJobDefOf.OGAMXBAttackBuilding, pawn2);
                                    return;
                                }
                            }
                            if (flag2)
                            {
                                Fire fire2 = thingList[j] as Fire;
                                if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
                                {
                                    fire = fire2;
                                }
                            }
                        }
                    }
                }
                if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
                {
                    this.pawn.natives.TryBeatFire(fire);
                    return;
                }
                if (flag && this.pawn.Faction != null && this.pawn.jobs.curJob.def == AMXenoBiologisJobDefOf.OGAMXBWaitCombatBuilding)
                {
                    bool allowManualCastWeapons = !this.pawn.IsColonist;
                    Verb verb = this.pawn.TryGetAttackVerb(null, allowManualCastWeapons);
                    if (verb != null && !verb.verbProps.IsMeleeAttack)
                    {
                        TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns;
                        //TargetScanFlags targetScanFlags = TargetScanFlags.None;
                        //if (verb.verbProps.ai_IsIncendiary)
                        //{
                        //    targetScanFlags |= TargetScanFlags.NeedNonBurning;
                        //}

                        Pawn pawn2 = (Pawn)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, targetScanFlags, (Thing x) => x is Pawn, verb.verbProps.minRange, verb.verbProps.range);


                        if (pawn2 != null)
                        {
                            this.pawn.TryStartAttack(pawn2);

                            return;
                        }
                    }
                }
            }
        }
    }
}
