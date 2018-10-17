using System;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000763 RID: 1891
	public class CompSpawnerNecronsOnDamaged : ThingComp
	{
		// Token: 0x060029B0 RID: 10672 RVA: 0x0013B2C3 File Offset: 0x001396C3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
			Scribe_Values.Look<float>(ref this.pointsLeft, "necronPointsLeft", 0f, false);
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0013B2F4 File Offset: 0x001396F4
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

		// Token: 0x060029B2 RID: 10674 RVA: 0x0013B3A8 File Offset: 0x001397A8
		public void Notify_BlueprintReplacedWithSolidThingNearby(Pawn by)
		{
			if (by.Faction != OfNecrons)
			{
				this.TrySpawnNecrons();
			}
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0013B3C0 File Offset: 0x001397C0
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
					Log.Error("Found no place for mechanoids to defend " + this, false);
					invalid = IntVec3.Invalid;
				}
				LordJob_NecronsDefendShip lordJob = new LordJob_NecronsDefendShip(this.parent, this.parent.Faction, 21f, invalid);
				this.lord = LordMaker.MakeNewLord(OfNecrons, lordJob, this.parent.Map, null);
			}
			PawnKindDef kind;
			while ((from def in DefDatabase<PawnKindDef>.AllDefs
			where def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft
			select def).TryRandomElement(out kind))
			{
				IntVec3 center;
				if ((from cell in GenAdj.CellsAdjacent8Way(this.parent)
				where this.CanSpawnNecronAt(cell)
				select cell).TryRandomElement(out center))
				{
					PawnGenerationRequest request = new PawnGenerationRequest(kind, OfNecrons, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (GenPlace.TryPlaceThing(pawn, center, this.parent.Map, ThingPlaceMode.Near, null, null))
					{
						this.lord.AddPawn(pawn);
						this.pointsLeft -= pawn.kindDef.combatPower;
						continue;
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}

         //       IL_1B9:


			}
            //	goto IL_1B9;
            this.pointsLeft = 0f;
            SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
            return;
        }

		// Token: 0x060029B4 RID: 10676 RVA: 0x0013B5A6 File Offset: 0x001399A6
		private bool CanSpawnNecronAt(IntVec3 c)
		{
			return c.Walkable(this.parent.Map);
		}


        public Faction OfNecrons
        {
            get
            {
                return this.ofNecrons;
            }
        }

        // Token: 0x04001736 RID: 5942
        public float pointsLeft;

		// Token: 0x04001737 RID: 5943
		private Lord lord;

		// Token: 0x04001738 RID: 5944
		private const float MechanoidsDefendRadius = 21f;

		// Token: 0x04001739 RID: 5945
		public static readonly string MemoDamaged = "ShipPartDamaged";

        // Token: 0x04000FA5 RID: 4005
        public Faction ofNecrons;

    }
}
