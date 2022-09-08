using System;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    public class CompProperties_TyranidTemperatureDamaged : CompProperties
    {
        public CompProperties_TyranidTemperatureDamaged()
        {
            this.compClass = typeof(CompTyranidTemperatureDamaged);
        }

        public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

        public int damagePerTickRare = 1;
    }

    public class CompTyranidTemperatureDamaged : ThingComp
	{
		public CompProperties_TyranidTemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TyranidTemperatureDamaged)this.props;
			}
		}

		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

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
                return holder?.RaceProps?.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Tyranid || holder?.RaceProps?.BloodDef == AdeptusThingDefOf.OG_FilthBlood_Tyranid  || holder?.Faction?.def == AdeptusFactionDefOf.OG_Tyranid_Faction;
            }
        }

		private void CheckTakeDamage()
		{
            if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature) && !HeldByNid)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
