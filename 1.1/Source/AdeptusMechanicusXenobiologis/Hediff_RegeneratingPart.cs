using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AdeptusMechanicus
{
    public class Hediff_RegeneratingPart : Hediff_AddedPart
    {

        public override bool ShouldRemove
        {
            get
            {
                return this.Severity == 0f;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }
        
        public override string TipStringExtra
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(base.TipStringExtra);
                stringBuilder.AppendLine("Progress: " + GenText.ToStringPercent(1f-this.Severity));
                return stringBuilder.ToString();
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            bool flag = this.Severity >= 1f;
            if (flag)
            {
                BodypartUtility.ReplaceHediffFromBodypart(this.pawn, base.Part, AMXenoBiologisHediffDefOf.MissingBodyPart, AMXenoBiologisHediffDefOf.OG_Regenerated);
            }
            if (this.Part.parts != null && this.Part.parts.Any(x => pawn.health.hediffSet.PartIsMissing(x)) && pawn.TryGetCompFast<Comp_NecronOG>() is Comp_NecronOG _Necron)
            {
                _Necron.TryRegrowBodyparts();
            }
        }
        
    }
}
