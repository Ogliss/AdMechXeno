using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobGiver_ChaosDeamon
    public class JobGiver_ChaosDeamon : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            //pawn.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon, null, false, false, null, false);
            if (pawn.TryGetAttackVerb(null, false) == null)
            {
                return null;
            }
            Pawn pawn2 = this.FindPawnTarget(pawn);
            if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
            {
                return this.MeleeAttackJob(pawn, pawn2);
            }
            Building building = this.FindTurretTarget(pawn);
            if (building != null)
            {
                return this.MeleeAttackJob(pawn, building);
            }
            if (pawn2 != null)
            {
                using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
                {
                    if (!pawnPath.Found)
                    {
                        return null;
                    }
                    IntVec3 loc;
                    if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
                    {
                        Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap, false);
                        return null;
                    }
                    IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
                    if (randomCell == pawn.Position)
                    {
                        return new Job(JobDefOf.Wait, 30, false);
                    }
                    return new Job(JobDefOf.Goto, randomCell);
                }
            }
            return null;
        }

        private Job MeleeAttackJob(Pawn pawn, Thing target)
        {
            Rand.PushState();
            int expiryInterval = Rand.Range(420, 900);
            Rand.PopState();
            return new Job(JobDefOf.AttackMelee, target)
            {
                maxNumMeleeAttacks = 1,
                expiryInterval = expiryInterval,
                attackDoorIfTargetLost = true
            };
        }

        // Token: 0x060005B9 RID: 1465 RVA: 0x00037BC0 File Offset: 0x00035FC0
        private Pawn FindPawnTarget(Pawn pawn)
        {
            //return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.Faction.HostileTo(Faction.OfInsects), 0f, 9999f, default(IntVec3), float.MaxValue, true, true);
            return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.Faction != Faction.OfInsects && x.def.race.canBePredatorPrey, 0f, 9999f, default(IntVec3), float.MaxValue, true, true);
        }

        // Token: 0x060005BA RID: 1466 RVA: 0x00037C14 File Offset: 0x00036014
        private Building FindTurretTarget(Pawn pawn)
        {
            return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false, true);
        }

        // Token: 0x040002FD RID: 765
        private const float WaitChance = 0.75f;

        // Token: 0x040002FE RID: 766
        private const int WaitTicks = 90;

        // Token: 0x040002FF RID: 767
        private const int MinMeleeChaseTicks = 420;

        // Token: 0x04000300 RID: 768
        private const int MaxMeleeChaseTicks = 900;

        // Token: 0x04000301 RID: 769
        private const int WanderOutsideDoorRegions = 9;
    }
}
