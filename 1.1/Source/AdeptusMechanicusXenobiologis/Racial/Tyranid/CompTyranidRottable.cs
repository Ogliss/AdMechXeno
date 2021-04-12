using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000257 RID: 599
    public class CompProperties_TyranidRottable : CompProperties
    {
        // Token: 0x06000AC0 RID: 2752 RVA: 0x00055F7F File Offset: 0x0005437F
        public CompProperties_TyranidRottable()
        {
            this.compClass = typeof(CompTyranidRottable);
        }

        // Token: 0x06000AC1 RID: 2753 RVA: 0x00055FB8 File Offset: 0x000543B8
        public CompProperties_TyranidRottable(float daysToRotStart)
        {
            this.daysToRotStart = daysToRotStart;
        }

        // Token: 0x17000198 RID: 408
        // (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00055FE8 File Offset: 0x000543E8
        public int TicksToRotStart
        {
            get
            {
                return Mathf.RoundToInt(this.daysToRotStart * 60000f);
            }
        }

        // Token: 0x17000199 RID: 409
        // (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00055FFB File Offset: 0x000543FB
        public int TicksToDessicated
        {
            get
            {
                return Mathf.RoundToInt(this.daysToDessicated * 60000f);
            }
        }

        // Token: 0x06000AC4 RID: 2756 RVA: 0x00056010 File Offset: 0x00054410
        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string e in base.ConfigErrors(parentDef))
            {
                yield return e;
            }
            if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
            {
                yield return string.Concat(new object[]
                {
                    "CompRottable needs tickerType ",
                    TickerType.Rare,
                    " or ",
                    TickerType.Normal,
                    ", has ",
                    parentDef.tickerType
                });
            }
            yield break;
        }

        // Token: 0x040004C2 RID: 1218
        public float daysToRotStart = 2f;

        // Token: 0x040004C3 RID: 1219
        public bool rotDestroys;

        // Token: 0x040004C4 RID: 1220
        public float rotDamagePerDay = 40f;

        // Token: 0x040004C5 RID: 1221
        public float daysToDessicated = 999f;

        // Token: 0x040004C6 RID: 1222
        public float dessicatedDamagePerDay;

        // Token: 0x040004C7 RID: 1223
        public bool disableIfHatcher;
    }
    // Token: 0x02000760 RID: 1888
    public class CompTyranidRottable : ThingComp
    {
        // Token: 0x17000669 RID: 1641
        // (get) Token: 0x060029A1 RID: 10657 RVA: 0x0013BD0A File Offset: 0x0013A10A
        public CompProperties_TyranidRottable PropsRot
        {
            get
            {
                return (CompProperties_TyranidRottable)this.props;
            }
        }

        // Token: 0x1700066A RID: 1642
        // (get) Token: 0x060029A2 RID: 10658 RVA: 0x0013BD17 File Offset: 0x0013A117
        public float RotProgressPct
        {
            get
            {
                return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
            }
        }

        // Token: 0x1700066B RID: 1643
        // (get) Token: 0x060029A3 RID: 10659 RVA: 0x0013BD2C File Offset: 0x0013A12C
        // (set) Token: 0x060029A4 RID: 10660 RVA: 0x0013BD34 File Offset: 0x0013A134
        public float RotProgress
        {
            get
            {
                return this.rotProgressInt;
            }
            set
            {
                RotStage stage = this.Stage;
                this.rotProgressInt = value;
                if (stage != this.Stage)
                {
                    this.StageChanged();
                }
            }
        }

        // Token: 0x1700066C RID: 1644
        // (get) Token: 0x060029A5 RID: 10661 RVA: 0x0013BD61 File Offset: 0x0013A161
        public RotStage Stage
        {
            get
            {
                if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
                {
                    return RotStage.Fresh;
                }
                if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
                {
                    return RotStage.Rotting;
                }
                return RotStage.Dessicated;
            }
        }

        // Token: 0x1700066D RID: 1645
        // (get) Token: 0x060029A6 RID: 10662 RVA: 0x0013BD98 File Offset: 0x0013A198
        public int TicksUntilRotAtCurrentTemp
        {
            get
            {
                float num = this.parent.AmbientTemperature;
                num = (float)Mathf.RoundToInt(num);
                return this.TicksUntilRotAtTemp(num);
            }
        }

        public bool HeldByNid
        {
            get
            {
                Pawn holder;
                CompEquippable equippable = this.parent.TryGetCompFast<CompEquippable>();
                holder = equippable?.PrimaryVerb?.CasterPawn;
                return holder?.RaceProps?.FleshType == OGTyranidDefOf.OG_Flesh_Tyranid || holder?.RaceProps?.BloodDef == OGTyranidDefOf.OG_FilthBlood_Tyranid || holder?.Faction?.def == OGTyranidDefOf.OG_Tyranid_Faction;
            }
        }

        // Token: 0x1700066E RID: 1646
        // (get) Token: 0x060029A7 RID: 10663 RVA: 0x0013BDC0 File Offset: 0x0013A1C0
        public bool Active
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

        // Token: 0x060029A8 RID: 10664 RVA: 0x0013BDFD File Offset: 0x0013A1FD
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
        }

        // Token: 0x060029A9 RID: 10665 RVA: 0x0013BE1B File Offset: 0x0013A21B
        public override void CompTick()
        {
            this.Tick(1);
        }

        // Token: 0x060029AA RID: 10666 RVA: 0x0013BE24 File Offset: 0x0013A224
        public override void CompTickRare()
        {
            this.Tick(250);
        }

        // Token: 0x060029AB RID: 10667 RVA: 0x0013BE34 File Offset: 0x0013A234
        private void Tick(int interval)
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

        // Token: 0x060029AC RID: 10668 RVA: 0x0013C00C File Offset: 0x0013A40C
        private bool ShouldTakeRotDamage()
        {
            Thing thing = this.parent.ParentHolder as Thing;
            if (HeldByNid)
            {
                return false;
            }
            return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
        }

        // Token: 0x060029AD RID: 10669 RVA: 0x0013C05C File Offset: 0x0013A45C
        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            float t = (float)count / (float)(this.parent.stackCount + count);
            float rotProgress = ((ThingWithComps)otherStack).GetComp<CompTyranidRottable>().RotProgress;
            this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
        }

        // Token: 0x060029AE RID: 10670 RVA: 0x0013C09F File Offset: 0x0013A49F
        public override void PostSplitOff(Thing piece)
        {
            ((ThingWithComps)piece).GetComp<CompTyranidRottable>().RotProgress = this.RotProgress;
        }

        // Token: 0x060029AF RID: 10671 RVA: 0x0013C0B7 File Offset: 0x0013A4B7
        public override void PostIngested(Pawn ingester)
        {
            if (this.Stage != RotStage.Fresh)
            {
                FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
            }
        }

        // Token: 0x060029B0 RID: 10672 RVA: 0x0013C0D4 File Offset: 0x0013A4D4
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

        // Token: 0x060029B1 RID: 10673 RVA: 0x0013C250 File Offset: 0x0013A650
        public int ApproxTicksUntilRotWhenAtTempOfTile(int tile, int ticksAbs)
        {
            float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
            return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
        }

        // Token: 0x060029B2 RID: 10674 RVA: 0x0013C26C File Offset: 0x0013A66C
        public int TicksUntilRotAtTemp(float temp)
        {
            if (!this.Active)
            {
                return 72000000;
            }
            float num = GenTemperature.RotRateAtTemperature(temp);
            if (num <= 0f)
            {
                return 72000000;
            }
            float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
            if (num2 <= 0f)
            {
                return 0;
            }
            return Mathf.RoundToInt(num2 / num);
        }

        // Token: 0x060029B3 RID: 10675 RVA: 0x0013C2CC File Offset: 0x0013A6CC
        private void StageChanged()
        {
            Corpse corpse = this.parent as Corpse;
            if (corpse != null)
            {
                corpse.RotStageChanged();
            }
        }

        // Token: 0x060029B4 RID: 10676 RVA: 0x0013C2F1 File Offset: 0x0013A6F1
        public void RotImmediately()
        {
            if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
            {
                this.RotProgress = (float)this.PropsRot.TicksToRotStart;
            }
        }

        // Token: 0x04001725 RID: 5925
        private float rotProgressInt;
    }
}
