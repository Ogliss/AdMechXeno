using RimWorld.Planet;
using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Necron
{
    public class CompSpawnerNecronsOnDamaged : ThingComp
    {
        public float pointsLeft;

        private Lord lord;

        private const float MechanoidsDefendRadius = 21f;

        public static readonly string MemoDamaged = "ShipPartDamaged";

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
            Scribe_Values.Look<float>(ref this.pointsLeft, "mechanoidPointsLeft", 0f, false);
        }

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
                float num = (float)(this.parent.HitPoints - dinfo.Amount);
                if ((num < (float)this.parent.MaxHitPoints * 0.98f && dinfo.Instigator != null && dinfo.Instigator.Faction != null) || num < (float)this.parent.MaxHitPoints * 0.9f)
                {
                    this.TrySpawnMechanoids();
                }
            }
            absorbed = false;
        }

        public void Notify_BlueprintReplacedWithSolidThingNearby(Pawn by)
        {
            if (by.Faction != Faction.OfNecrons)
            {
                this.TrySpawnMechanoids();
            }
        }

        private void TrySpawnMechanoids()
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
                if (!CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 5, (IntVec3 c) => c.Standable(this.parent.Map) && this.parent.Map.reachability.CanReach(c, this.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)), out invalid))
                {
                    Log.Error("Found no place for mechanoids to defend " + this);
                    invalid = IntVec3.Invalid;
                }
                LordJob_NecronsDefendShip lordJob = new LordJob_NecronsDefendShip(this.parent, this.parent.Faction, 21f, invalid);
                this.lord = LordMaker.MakeNewLord(Faction.OfNecrons, lordJob, this.parent.Map, null);
            }
            PawnKindDef kindDef;
            while ((from def in DefDatabase<PawnKindDef>.AllDefs
                    where def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft
                    select def).TryRandomElement(out kindDef))
            {
                IntVec3 center;
                if ((from cell in GenAdj.CellsAdjacent8Way(this.parent)
                     where this.CanSpawnMechanoidAt(cell)
                     select cell).TryRandomElement(out center))
                {
                    Pawn pawn = PawnGenerator.GeneratePawn(kindDef, Faction.OfNecrons);
                    if (GenPlace.TryPlaceThing(pawn, center, this.parent.Map, ThingPlaceMode.Near, null))
                    {
                        this.lord.AddPawn(pawn);
                        this.pointsLeft -= pawn.kindDef.combatPower;
                        continue;
                    }
                    Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                }
                IL_164:
                this.pointsLeft = 0f;
                RimWorld.SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
                return;
            }
            goto IL_164;
        }

        private bool CanSpawnMechanoidAt(IntVec3 c)
        {
            return c.Walkable(this.parent.Map);
        }
    }
}
