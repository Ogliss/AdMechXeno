﻿using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobGiver_ChaosDeamon
    public class JobGiver_ChaosDeamon : ThinkNode_JobGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            //pawn.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon, null, false, false, null, false);
            if (pawn.TryGetAttackVerb(null, false) == null)
            {
                return null;
            }
            Pawn pawn2 = this.FindPawnTarget(pawn);
            if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
            {
                return this.MeleeAttackJob(pawn2);
            }
            Building building = this.FindTurretTarget(pawn);
            if (building != null)
            {
                return this.MeleeAttackJob(building);
            }
            if (pawn2 != null)
            {
                using PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell);
                if (!pawnPath.Found)
                {
                    return null;
                }
                if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out IntVec3 loc))
                {
                    Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
                    return null;
                }
                IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), WanderOutsideDoorRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
                if (randomCell == pawn.Position)
                {
                    return new Job(JobDefOf.Wait, WaitTicks, false);
                }
                return new Job(JobDefOf.Goto, randomCell);
            }
            return null;
        }

        private Job MeleeAttackJob(Thing target)
        {
            Rand.PushState();
            int expiryInterval = Rand.Range(MinMeleeChaseTicks, MaxMeleeChaseTicks);
            Rand.PopState();
            return new Job(JobDefOf.AttackMelee, target)
            {
                maxNumMeleeAttacks = 1,
                expiryInterval = expiryInterval,
                attackDoorIfTargetLost = true
            };
        }

        private Pawn FindPawnTarget(Pawn pawn)
        {
            //return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.Faction.HostileTo(Faction.OfInsects), 0f, 9999f, default(IntVec3), float.MaxValue, true, true);
            return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.Faction != Faction.OfInsects && x.def.race.canBePredatorPrey, 0f, 9999f, default, float.MaxValue, true, true);
        }

        private Building FindTurretTarget(Pawn pawn)
        {
            return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Thing t) => t is Building, 0f, 70f, default, float.MaxValue, false, true);
        }

        private const float WaitChance = 0.75f;
        private const int WaitTicks = 90;
        private const int MinMeleeChaseTicks = 420;
        private const int MaxMeleeChaseTicks = 900;
        private const int WanderOutsideDoorRegions = 9;
    }
}
