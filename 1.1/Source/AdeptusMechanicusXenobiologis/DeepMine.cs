using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
    // Token: 0x02000268 RID: 616
    public class CompProperties_DeepMine : CompProperties
    {
        // Token: 0x06000AD8 RID: 2776 RVA: 0x000566F8 File Offset: 0x00054AF8
        public CompProperties_DeepMine()
        {
            this.compClass = typeof(CompDeepMine);
        }
    }
    // Token: 0x02000733 RID: 1843
    public class CompDeepMine : ThingComp
    {
        // Token: 0x17000615 RID: 1557
        // (get) Token: 0x06002876 RID: 10358 RVA: 0x00134288 File Offset: 0x00132688
        public static float WorkPerPortionCurrentDifficulty
        {
            get
            {
                return 12000f / Find.Storyteller.difficulty.mineYieldFactor;
            }
        }

        // Token: 0x17000616 RID: 1558
        // (get) Token: 0x06002877 RID: 10359 RVA: 0x0013429F File Offset: 0x0013269F
        public float ProgressToNextPortionPercent
        {
            get
            {
                return this.portionProgress / CompDeepMine.WorkPerPortionCurrentDifficulty;
            }
        }

        // Token: 0x06002878 RID: 10360 RVA: 0x001342AE File Offset: 0x001326AE
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            this.powerComp = this.parent.TryGetComp<CompRefuelable>();
        }

        // Token: 0x06002879 RID: 10361 RVA: 0x001342C1 File Offset: 0x001326C1
        public override void PostExposeData()
        {
            Scribe_Values.Look<float>(ref this.portionProgress, "portionProgress", 0f, false);
            Scribe_Values.Look<float>(ref this.portionYieldPct, "portionYieldPct", 0f, false);
            Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
        }

        // Token: 0x0600287A RID: 10362 RVA: 0x00134304 File Offset: 0x00132704
        public void DrillWorkDone(Pawn driller)
        {
            float statValue = driller.GetStatValue(StatDefOf.MiningSpeed, true);
            this.portionProgress += statValue;
            this.portionYieldPct += statValue * driller.GetStatValue(StatDefOf.MiningYield, true) / CompDeepMine.WorkPerPortionCurrentDifficulty;
            this.lastUsedTick = Find.TickManager.TicksGame;
            if (this.portionProgress > CompDeepMine.WorkPerPortionCurrentDifficulty)
            {
                this.TryProducePortion(this.portionYieldPct);
                this.portionProgress = 0f;
                this.portionYieldPct = 0f;
            }
        }

        // Token: 0x0600287B RID: 10363 RVA: 0x0013438F File Offset: 0x0013278F
        public override void PostDeSpawn(Map map)
        {
            this.portionProgress = 0f;
            this.portionYieldPct = 0f;
            this.lastUsedTick = -99999;
        }

        // Token: 0x0600287C RID: 10364 RVA: 0x001343B4 File Offset: 0x001327B4
        private void TryProducePortion(float yieldPct)
        {
            ThingDef thingDef;
            int num;
            IntVec3 intVec;
            bool nextResource = this.GetNextResource(out thingDef, out num, out intVec);
            if (thingDef == null)
            {
                return;
            }
            int num2 = Mathf.Min(num, thingDef.deepCountPerPortion);
            if (nextResource)
            {
                this.parent.Map.deepResourceGrid.SetAt(intVec, thingDef, num - num2);
            }
            int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
            Thing thing = ThingMaker.MakeThing(thingDef, null);
            thing.stackCount = stackCount;
            GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, null);
            if (nextResource && !this.ValuableResourcesPresent())
            {
                if (DeepDrillUtility.GetBaseResource(this.parent.Map) == null)
                {
                    Messages.Message("DeepDrillExhaustedNoFallback".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
                }
                else
                {
                    Messages.Message("DeepDrillExhausted".Translate(Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(this.parent.Map).label, -1)), this.parent, MessageTypeDefOf.TaskCompletion, true);
                    for (int i = 0; i < 9; i++)
                    {
                        IntVec3 c = intVec + GenRadial.RadialPattern[i];
                        if (c.InBounds(this.parent.Map))
                        {
                            ThingWithComps firstThingWithComp = c.GetFirstThingWithComp<CompDeepMine>(map: parent.Map);
                            if (firstThingWithComp != null && !firstThingWithComp.GetComp<CompDeepMine>().ValuableResourcesPresent())
                            {
                                firstThingWithComp.SetForbidden(true, true);
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x0600287D RID: 10365 RVA: 0x0013455A File Offset: 0x0013295A
        private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
        {
            return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
        }

        // Token: 0x0600287E RID: 10366 RVA: 0x0013457A File Offset: 0x0013297A
        public bool CanDrillNow()
        {
            return (DeepDrillUtility.GetBaseResource(this.parent.Map) != null || this.ValuableResourcesPresent());
        }

        // Token: 0x0600287F RID: 10367 RVA: 0x001345B8 File Offset: 0x001329B8
        public bool ValuableResourcesPresent()
        {
            ThingDef thingDef;
            int num;
            IntVec3 intVec;
            return this.GetNextResource(out thingDef, out num, out intVec);
        }

        // Token: 0x06002880 RID: 10368 RVA: 0x001345D1 File Offset: 0x001329D1
        public bool UsedLastTick()
        {
            return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
        }

        // Token: 0x06002881 RID: 10369 RVA: 0x001345EC File Offset: 0x001329EC
        public override string CompInspectStringExtra()
        {
            if (!this.parent.Spawned)
            {
                return null;
            }
            ThingDef thingDef;
            int num;
            IntVec3 intVec;
            this.GetNextResource(out thingDef, out num, out intVec);
            if (thingDef == null)
            {
                return "DeepDrillNoResources".Translate();
            }
            return string.Concat(new string[]
            {
                "ResourceBelow".Translate(),
                ": ",
                thingDef.LabelCap,
                "\n",
                "ProgressToNextPortion".Translate(),
                ": ",
                this.ProgressToNextPortionPercent.ToStringPercent("F0")
            });
        }

        // Token: 0x040016A4 RID: 5796
        private CompRefuelable powerComp;

        // Token: 0x040016A5 RID: 5797
        private float portionProgress;

        // Token: 0x040016A6 RID: 5798
        private float portionYieldPct;

        // Token: 0x040016A7 RID: 5799
        private int lastUsedTick = -99999;

        // Token: 0x040016A8 RID: 5800
        private const float WorkPerPortionBase = 12000f;
    }
    // Token: 0x02000140 RID: 320
    public class WorkGiver_DeepMine : WorkGiver_Scanner
    {
        // Token: 0x170000FD RID: 253
        // (get) Token: 0x06000695 RID: 1685 RVA: 0x0003D36B File Offset: 0x0003B76B
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(DeepMinDefOf.DeepMine);
            }
        }

        // Token: 0x170000FE RID: 254
        // (get) Token: 0x06000696 RID: 1686 RVA: 0x0003D377 File Offset: 0x0003B777
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.InteractionCell;
            }
        }

        // Token: 0x06000697 RID: 1687 RVA: 0x0003D37A File Offset: 0x0003B77A
        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        // Token: 0x06000698 RID: 1688 RVA: 0x0003D37D File Offset: 0x0003B77D
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.listerBuildings.AllBuildingsColonistOfDef(DeepMinDefOf.DeepMine).Cast<Thing>();
        }

        // Token: 0x06000699 RID: 1689 RVA: 0x0003D39C File Offset: 0x0003B79C
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
            for (int i = 0; i < allBuildingsColonist.Count; i++)
            {
                Building building = allBuildingsColonist[i];
                if (building.def == DeepMinDefOf.DeepMine)
                {
                    CompPowerTrader comp = building.GetComp<CompPowerTrader>();
                    if ((comp == null || comp.PowerOn) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Token: 0x0600069A RID: 1690 RVA: 0x0003D420 File Offset: 0x0003B820
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t.Faction != pawn.Faction)
            {
                return false;
            }
            Building building = t as Building;
            if (building == null)
            {
                return false;
            }
            if (building.IsForbidden(pawn))
            {
                return false;
            }
            LocalTargetInfo target = building;
            if (!pawn.CanReserve(target, 1, -1, null, forced))
            {
                return false;
            }
            CompDeepMine compDeepDrill = building.TryGetComp<CompDeepMine>();
            return compDeepDrill.CanDrillNow() && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null && !building.IsBurning();
        }

        // Token: 0x0600069B RID: 1691 RVA: 0x0003D4BB File Offset: 0x0003B8BB
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(DeepMinDefOf.OperateDeepMine, t, 1500, true);
        }
    }
    [DefOf]
    public static class DeepMinDefOf
    {
        public static ThingDef DeepMine;
        public static JobDef OperateDeepMine;
    }

}
