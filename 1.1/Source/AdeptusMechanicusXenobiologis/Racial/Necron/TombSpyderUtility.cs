using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // Token: 0x020000D7 RID: 215
    public static class TombSpyderUtility
    {
        // Token: 0x060004D2 RID: 1234 RVA: 0x00031074 File Offset: 0x0002F474
        public static bool TryFindGoodRepairTarget(Pawn Spyder, float maxDist, out Pawn Target, List<Thing> disallowed = null)
        {
            if (!Spyder.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                Target = null;
                return false;
            }
            Predicate<Thing> validator = delegate (Thing t)
            {
                Pawn pawn = t as Pawn;
                return pawn.RaceProps.FleshType == OGNecronDefOf.OG_Flesh_Construct_Necron && pawn.TryGetCompFast<Comp_NecronOG>()!=null && (pawn.Downed) && pawn.Faction == Spyder.Faction && Spyder.CanReserve(pawn, 1, -1, null, false) && (disallowed == null || !disallowed.Contains(pawn));
            };
            Target = (Pawn)GenClosest.ClosestThingReachable(Spyder.Position, Spyder.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, null, 0, -1, false, RegionType.Set_Passable, false);
            return Target != null;
        }

        // Token: 0x060004D2 RID: 1234 RVA: 0x00031074 File Offset: 0x0002F474
        public static bool TryFindGoodRessTarget(Pawn Spyder, float maxDist, out Corpse Target, List<Thing> disallowed = null)
        {
            if (!Spyder.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                Target = null;
                return false;
            }
            Predicate<Thing> validator = delegate (Thing t)
            {
                Corpse pawn = t as Corpse;
                return pawn.InnerPawn.RaceProps.FleshType == OGNecronDefOf.OG_Flesh_Construct_Necron && (pawn.TryGetCompFast<Comp_NecronOG>() != null || pawn.InnerPawn.TryGetCompFast<Comp_NecronOG>() != null) && pawn.InnerPawn.Faction == Spyder.Faction && Spyder.CanReserve(pawn, 1, -1, null, false) && (disallowed == null || !disallowed.Contains(pawn));
            };
            Target = (Corpse)GenClosest.ClosestThingReachable(Spyder.Position, Spyder.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, null, 0, -1, false, RegionType.Set_Passable, false);
            return Target != null;
        }

    }
}
