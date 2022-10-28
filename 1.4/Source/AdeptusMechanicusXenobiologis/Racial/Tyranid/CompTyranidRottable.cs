using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
    public class CompProperties_TyranidRottable : CompProperties_Rottable
    {
        public CompProperties_TyranidRottable()
        {
            this.compClass = typeof(CompTyranidRottable);
        }

        public CompProperties_TyranidRottable(float daysToRotStart)
        {
            this.daysToRotStart = daysToRotStart;
        }

    }

    public class CompTyranidRottable : CompRottable
    {
        public new CompProperties_TyranidRottable PropsRot
        {
            get
            {
                return (CompProperties_TyranidRottable)this.props;
            }
        }

        public bool HeldByNid
        {
            get
            {
                Pawn holder;
                CompEquippable equippable = this.parent.TryGetCompFast<CompEquippable>();
                holder = equippable?.PrimaryVerb?.CasterPawn;
                return holder?.RaceProps?.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Tyranid || holder?.RaceProps?.BloodDef == AdeptusThingDefOf.OG_FilthBlood_Tyranid || holder?.Faction?.def == AdeptusFactionDefOf.OG_Tyranid_Faction;
            }
        }

        public new bool Active
        {
            get
            {
                if (HeldByNid)
                {
                    return false;
                }
                if (this.PropsRot.disableIfHatcher)
                {
                    CompHatcher compHatcher = this.parent.TryGetCompFast<CompHatcher>();
                    if (compHatcher != null && !compHatcher.TemperatureDamaged)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
        }

        public override void CompTick()
        {
            this.Tick(1);
        }

        public override void CompTickRare()
        {
            this.Tick(250);
        }

        private new void Tick(int interval)
        {
            if (!this.Active)
            {
                return;
            }
            float rotProgress = this.RotProgress;
            float ambientTemperature = this.parent.AmbientTemperature;
            float num = GenTemperature.RotRateAtTemperature(ambientTemperature);
            this.RotProgress += num * (float)interval;
            if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
            {
                if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
                {
                    Messages.Message("MessageRottedAwayInStorage".Translate(this.parent.Label, this.parent).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
                    LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
                }
                this.parent.Destroy(DestroyMode.Vanish);
                return;
            }
            bool flag = Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f);
            if (flag && this.ShouldTakeRotDamage())
            {
                if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
                {
                    this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
                }
                else if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
                {
                    this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
                }
            }
        }

        private new bool ShouldTakeRotDamage()
        {
            Thing thing = this.parent.ParentHolder as Thing;
            if (HeldByNid)
            {
                return false;
            }
            return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
        }

        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            float t = (float)count / (float)(this.parent.stackCount + count);
            float rotProgress = ((ThingWithComps)otherStack).GetComp<CompTyranidRottable>().RotProgress;
            this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
        }

        public override void PostSplitOff(Thing piece)
        {
            ((ThingWithComps)piece).GetComp<CompTyranidRottable>().RotProgress = this.RotProgress;
        }

        public override void PostIngested(Pawn ingester)
        {
            if (this.Stage != RotStage.Fresh)
            {
                FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
            }
        }

        public override string CompInspectStringExtra()
        {
            if (!this.Active)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            RotStage stage = this.Stage;
            if (stage != RotStage.Fresh)
            {
                if (stage != RotStage.Rotting)
                {
                    if (stage == RotStage.Dessicated)
                    {
                        stringBuilder.Append("RotStateDessicated".Translate() + ".");
                    }
                }
                else
                {
                    stringBuilder.Append("RotStateRotting".Translate() + ".");
                }
            }
            else
            {
                stringBuilder.Append("RotStateFresh".Translate() + ".");
            }
            float num = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
            if (num > 0f)
            {
                float num2 = this.parent.AmbientTemperature;
                num2 = (float)Mathf.RoundToInt(num2);
                float num3 = GenTemperature.RotRateAtTemperature(num2);
                int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
                stringBuilder.AppendLine();
                if (num3 < 0.001f)
                {
                    stringBuilder.Append("CurrentlyFrozen".Translate() + ".");
                }
                else if (num3 < 0.999f)
                {
                    stringBuilder.Append("CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod()) + ".");
                }
                else
                {
                    stringBuilder.Append("NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod()) + ".");
                }
            }
            return stringBuilder.ToString();
        }

    }
}
