using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
    // Token: 0x0200025A RID: 602
    public class CompProperties_SpawnerNecronsOnDamaged : CompProperties
    {
        // Token: 0x06000AC8 RID: 2760 RVA: 0x000562D4 File Offset: 0x000546D4
        public CompProperties_SpawnerNecronsOnDamaged()
        {
            this.compClass = typeof(CompSpawnerNecronsOnDamaged);
        }
    }

    // Token: 0x02000769 RID: 1897
    public class CompSpawnerNecronsOnDamaged : ThingComp
    {
        public Faction OfNecrons
        {
            get
            {
                return Find.FactionManager.FirstFactionOfDef(OGNecronDefOf.NecronFaction);
            }
        }
        // Token: 0x060029EB RID: 10731 RVA: 0x0013D92F File Offset: 0x0013BD2F
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
            Scribe_Values.Look<float>(ref this.pointsLeft, "NecronPointsLeft", 0f, false);
        }

        // Token: 0x060029EC RID: 10732 RVA: 0x0013D960 File Offset: 0x0013BD60
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PostPreApplyDamage(dinfo, out absorbed);
            if (absorbed)
            {
                return;
            }
            if (dinfo.Def.harmsHealth)
            {
                if (this.lord != null)
                {
                    this.lord.ReceiveMemo(CompSpawnerNecronsOnDamaged.MemoDamaged);
                }
                float num = (float)this.parent.HitPoints - dinfo.Amount;
                if ((num < (float)this.parent.MaxHitPoints * 0.98f && dinfo.Instigator != null && dinfo.Instigator.Faction != null) || num < (float)this.parent.MaxHitPoints * 0.9f)
                {
                    this.TrySpawnNecrons();
                }
            }
            absorbed = false;
        }

        // Token: 0x060029ED RID: 10733 RVA: 0x0013DA14 File Offset: 0x0013BE14
        public void Notify_BlueprintReplacedWithSolidThingNearby(Pawn by)
        {
            if (by.Faction != OfNecrons)
            {
                this.TrySpawnNecrons();
            }
        }

        // Token: 0x060029EE RID: 10734 RVA: 0x0013DA2C File Offset: 0x0013BE2C
        private void TrySpawnNecrons()
        {
            if (this.pointsLeft <= 0f)
            {
                return;
            }
            if (!this.parent.Spawned)
            {
                return;
            }
            if (this.lord == null)
            {
                IntVec3 invalid;
                if (!CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 5, (IntVec3 c) => c.Standable(this.parent.Map) && this.parent.Map.reachability.CanReach(c, this.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)), out invalid, -1))
                {
                    Log.Error("Found no place for Necrons to defend " + this, false);
                    invalid = IntVec3.Invalid;
                }
                LordJob_NecronsDefendShip lordJob = new LordJob_NecronsDefendShip(this.parent, this.parent.Faction, 21f, invalid);
                this.lord = LordMaker.MakeNewLord(OfNecrons, lordJob, this.parent.Map, null);
            }
            try
            {
                while (this.pointsLeft > 0f)
                {
                    PawnKindDef kind;
                    if (!(from def in DefDatabase<PawnKindDef>.AllDefs
                          where def.defaultFactionType==OGNecronDefOf.NecronFaction && def.isFighter && def.combatPower <= this.pointsLeft
                          select def).TryRandomElement(out kind))
                    {
                        break;
                    }
                    IntVec3 center;
                    if (!(from cell in GenAdj.CellsAdjacent8Way(this.parent)
                          where this.CanSpawnNecronAt(cell)
                          select cell).TryRandomElement(out center))
                    {
                        break;
                    }
                    PawnGenerationRequest request = new PawnGenerationRequest(kind, OfNecrons, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
                    Pawn pawn = PawnGenerator.GeneratePawn(request);
                    if (!GenPlace.TryPlaceThing(pawn, center, this.parent.Map, ThingPlaceMode.Near, null, null))
                    {
                        Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                        break;
                    }
                    this.lord.AddPawn(pawn);
                    this.pointsLeft -= pawn.kindDef.combatPower;
                }
            }
            finally
            {
                this.pointsLeft = 0f;
            }
            SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
        }

        // Token: 0x060029EF RID: 10735 RVA: 0x0013DC44 File Offset: 0x0013C044
        private bool CanSpawnNecronAt(IntVec3 c)
        {
            return c.Walkable(this.parent.Map);
        }

        // Token: 0x04001746 RID: 5958
        public float pointsLeft;

        // Token: 0x04001747 RID: 5959
        private Lord lord;

        // Token: 0x04001748 RID: 5960
        private const float NecronsDefendRadius = 21f;

        // Token: 0x04001749 RID: 5961
        public static readonly string MemoDamaged = "ShipPartDamaged";

        // Token: 0x04000FB7 RID: 4023
        private List<Faction> allFactions = new List<Faction>();
    }
}
