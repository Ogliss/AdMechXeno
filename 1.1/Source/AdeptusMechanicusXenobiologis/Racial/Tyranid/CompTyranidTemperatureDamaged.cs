using System;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000B48 RID: 2888
    public class CompProperties_TyranidTemperatureDamaged : CompProperties
    {
        // Token: 0x06004013 RID: 16403 RVA: 0x001E07B4 File Offset: 0x001DEBB4
        public CompProperties_TyranidTemperatureDamaged()
        {
            this.compClass = typeof(CompTyranidTemperatureDamaged);
        }

        // Token: 0x0400292F RID: 10543
        public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

        // Token: 0x04002930 RID: 10544
        public int damagePerTickRare = 1;
    }

    // Token: 0x02000E60 RID: 3680
    public class CompTyranidTemperatureDamaged : ThingComp
	{
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x0026623B File Offset: 0x0026463B
		public CompProperties_TyranidTemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TyranidTemperatureDamaged)this.props;
			}
		}

		// Token: 0x0600541C RID: 21532 RVA: 0x00266248 File Offset: 0x00264648
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600541D RID: 21533 RVA: 0x00266265 File Offset: 0x00264665
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

        public bool HeldByNid
        {
            get
            {
                Pawn holder;
                CompEquippable equippable = this.parent.TryGetCompFast<CompEquippable>();
                holder = equippable?.PrimaryVerb?.CasterPawn;
                return holder?.RaceProps?.FleshType == OGTyranidDefOf.OG_Flesh_Tyranid || holder?.RaceProps?.BloodDef == OGTyranidDefOf.OG_FilthBlood_Tyranid  || holder?.Faction?.def == OGTyranidDefOf.OG_Tyranid_Faction;
            }
        }

		// Token: 0x0600541E RID: 21534 RVA: 0x00266270 File Offset: 0x00264670
		private void CheckTakeDamage()
		{
            if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature) && !HeldByNid)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
