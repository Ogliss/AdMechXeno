using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse.AI;
using RimWorld;
using AdeptusMechanicus;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobGiver_XBAttackBuilding
    public class JobGiver_XBAttackBuilding : ThinkNode_JobGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            Region region = pawn.GetRegion();
            if (region == null)
            {
                return null;
            }
            float num = float.MaxValue;
            IntVec3 c = pawn.Position;
            Building building = null;
            if (building != null)
            {
                building = null;
            }
            List<Building> list1 = pawn.Map.listerBuildings.allBuildingsColonist;
            for (int i = 0; i < list1.Count; i++)
            {
                Building deconstruct = list1[i];
                int num2 = deconstruct.Position.DistanceToSquared(pawn.Position);
                if ((float)num2 < num && deconstruct.def.useHitPoints && deconstruct != null && !deconstruct.Destroyed && pawn.CurrentEffectiveVerb.CanHitTargetFrom(pawn.Position, deconstruct))   /* && pawn.CanReach(deconstruct, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)*/
                {
                    num = (float)num2;
                    building = deconstruct;
                }
            }
            if (building != null && !building.Destroyed && building.def.useHitPoints && pawn.Position.DistanceTo(building.Position) <= pawn.CurrentEffectiveVerb.verbProps.range)
            {
                return new Job(AdeptusJobDefOf.OG_AMXB_AttackBuilding, building);
            }
            return null;

        }
    }
}
