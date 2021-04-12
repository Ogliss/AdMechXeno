using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobGiver_OGChaosDeamon_Ranged
    public class JobGiver_OGChaosDeamon_Ranged : ThinkNode_JobGiver
    {
        private const float WaitChance = 0.0f;

        private const int WaitTicks = 30;

        private const int MinMeleeChaseTicks = 420;

        private const int MaxMeleeChaseTicks = 900;

        private const int WanderOutsideDoorRegions = 9;

        protected override Job TryGiveJob(Pawn pawn)
        {
            //  pawn.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon, null, false, false, null, false);
            //  pawn.mindState.duty.def = DutyDefOf.AssaultColony;
            Rand.PushState();
            float wait = Rand.Value;
            int expiryInterval = Rand.Range(42, 200);
            Rand.PopState();
            if (wait < WaitChance)
            {
                return new Job(JobDefOf.Wait_Combat)
                {
                    expiryInterval = 500
                };
            }
            if (pawn.TryGetAttackVerb(null, false) == null)
            {
                return null;
            }
            Pawn pawn2 = this.FindPawnTarget(pawn);
            if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
            {

            //    log.message(string.Format("{0}", pawn.equipment.AllEquipmentListForReading));
                if (pawn.equipment.PrimaryEq!=null)
                {
                    if (!pawn.equipment.PrimaryEq.PrimaryVerb.IsMeleeAttack)
                    {
                        return new Job(JobDefOf.UseVerbOnThing, pawn2)
                        {
                            verbToUse = pawn.equipment.PrimaryEq.PrimaryVerb,
                            expiryInterval = expiryInterval,
                            attackDoorIfTargetLost = true
                        };
                    }
                    else
                    {
                        return new Job(JobDefOf.AttackMelee, pawn2)
                        {
                            maxNumMeleeAttacks = 1,
                            expiryInterval = expiryInterval,
                            canBash = true
                        };
                    }
                }
                else
                {
                    return new Job(JobDefOf.AttackMelee, pawn2)
                    {
                        maxNumMeleeAttacks = 1,
                        expiryInterval = expiryInterval,
                        canBash = true
                    };
                }
            }
            Building building = this.FindTurretTarget(pawn);
            if (building != null)
            {
                return new Job(JobDefOf.AttackMelee, pawn2)
                {
                    maxNumMeleeAttacks = 1,
                    expiryInterval = Rand.Range(420, 900),
                    canBash = true
                };
            }
            if (pawn2 != null)
            {
                using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
                {
                    Job result;
                    if (!pawnPath.Found)
                    {
                        result = null;
                        return result;
                    }
                    IntVec3 loc;
                    if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
                    {
                        Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
                        result = null;
                        return result;
                    }
                    IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
                    if (randomCell == pawn.Position)
                    {
                        result = new Job(JobDefOf.Wait, 30, false);
                        return result;
                    }
                    result = new Job(JobDefOf.Goto, randomCell);
                    return result;
                }
            }
            return null;
        }

        //private Job MeleeAttackJob(Pawn pawn, Thing target)
        //{
        //    return new Job(JobDefOf.PredatorHunt, target)
        //    {
        //        maxNumMeleeAttacks = 1,
        //        expiryInterval = Rand.Range(420, 900),
        //        attackDoorIfTargetLost = true
        //    };
        //}


        private Pawn FindPawnTarget(Pawn pawn)
        {
            return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.Faction.HostileTo(Faction.OfInsects), 0f, 9999f, default(IntVec3), 3.40282347E+38f, true);
        }


        private Building FindTurretTarget(Pawn pawn)
        {
            return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Thing t) => t is Building, 0f, 70f, default(IntVec3), 3.40282347E+38f, false);
        }
    }
}
