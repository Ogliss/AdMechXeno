using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace AdeptusMechanicus
{
    public class CompProperties_Daemonic : CompProperties
    {
        public CompProperties_Daemonic()
        {
            this.compClass = typeof(CompDaemonic);
        }
        public bool regenerating = false;

    }

    // Token: 0x02000002 RID: 2
    public class CompDaemonic : ThingComp
    {
        public CompProperties_Daemonic Props => (CompProperties_Daemonic)props;
        public Pawn pawn;
        public Lord lord;

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            if (dinfo.Def == OGDamageDefOf.OG_WarpStormStrike)
            {
                absorbed = true;
                SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.parent.Position, base.parent.Map, false));
                Vector3 impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
                Vector3 loc = base.parent.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
                float num = Mathf.Min(10f, 2f + (float)dinfo.Amount / 10f);
                MoteMaker.MakeStaticMote(loc, base.parent.Map, ThingDefOf.Mote_ExplosionFlash, num);
                int num2 = (int)num;
                for (int i = 0; i < num2; i++)
                {
                    Rand.PushState();
                    MoteMaker.ThrowDustPuff(loc, base.parent.Map, Rand.Range(0.8f, 1.2f));
                    Rand.PopState();
                }
            }
            else base.PostPreApplyDamage(dinfo, out absorbed);
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            // Log.Message(string.Format("took damage from {0}'s {1}", dinfo.Instigator.Label, dinfo.Weapon.label));
            if (base.parent is Pawn pawn)
            {
                // Log.Message(string.Format("{0} At: {1}", pawn.Name, pawn.Position, pawn.Map));
                // Log.Message(string.Format("{0} status: downed: {1} dead:{2}", pawn.Name, pawn.Downed, pawn.Dead));
                if (pawn.Downed && !pawn.Dead)
                {
                    pawn.Kill(null);
                }
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            bool flag = Find.Selector.SingleSelectedThing == base.parent;
            if (flag)
            {

            }
            Rand.PushState();
            if (base.parent.IsHashIntervalTick(Rand.RangeInclusive(300, 1200)) && base.parent != null)
            {
                if (base.parent is Pawn pawn && pawn != null )
                {
                    // Log.Message(string.Format("parent = pawn && is not null"));
                    lord = pawn.GetLord();
                    // Log.Message(string.Format("lord: {0}",lord));
                    if (pawn.MentalStateDef == MentalStateDefOf.PanicFlee)
                    {
                        // Log.Message(string.Format("Fleeing"));
                        if (Rand.Chance(0.85f)) pawn.Kill(null);
                    }
                    else if (lord == null && pawn.MentalStateDef != MentalStateDefOf.PanicFlee)
                    {
                        // pawn.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon, null, false, false, null, false);
                    }
                    else
                    {
                        if (pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss))
                        {
                            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss));
                        }
                    }
                }
            }
            Rand.PopState();
        }
    }
}
