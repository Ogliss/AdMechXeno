using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace AdeptusMechanicus
{
    public class CompProperties_ChaosDeamonOG : CompProperties
    {
        public CompProperties_ChaosDeamonOG()
        {
            this.compClass = typeof(CompChaosDeamonOG);
        }
        public bool regenerating = false;

    }

    // Token: 0x02000002 RID: 2
    public class CompChaosDeamonOG : ThingComp
    {
        public CompProperties_ChaosDeamonOG Props => (CompProperties_ChaosDeamonOG)props;
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
                     //    log.message(string.Format("Not Fleeing but null lord"));
                        /*
                     //    log.message(string.Format("{0} At: {1}", pawn.Name, pawn.Position, pawn.Map));
                     //    log.message(string.Format("{0} MentalState: {1}", pawn.Name, pawn.MentalState, pawn.Map));
                     //    log.message(string.Format("{0} mindState: {1}", pawn.Name, pawn.mindState, pawn.Map));
                     //    log.message(string.Format("{0} mindState duty: {1} thinkData: {2}", pawn.Name, pawn.mindState.duty, pawn.mindState.thinkData));
                     //    log.message(string.Format("{0} mindState lastJobTag: {1} canFleeIndividual: {2}", pawn.Name, pawn.mindState.lastJobTag, pawn.mindState.canFleeIndividual));
                     //    log.message(string.Format("{0} jobs: {1}", pawn.Name, pawn.jobs, pawn.Map));
                     //    log.message(string.Format("{0} jobs curDriver: {1} curJob: {2}", pawn.Name, pawn.jobs.curDriver, pawn.jobs.curJob));
                     //    log.message(string.Format("{0} jobs jobs: {1} jobs: {2}", pawn.Name, pawn.jobs.jobQueue, pawn.jobs.posture));
                     //    log.message(string.Format("{0} CurJob: {1}", pawn.Name, pawn.CurJob, pawn.Map));
                     //    log.message(string.Format("{0} CurJob lord: {1} def: {2}", pawn.Name, pawn.CurJob.lord, pawn.CurJob.def));
                     //    log.message(string.Format("{0} CurJobDef: {1}", pawn.Name, pawn.CurJobDef, pawn.Map));
                        */

                        // pawn.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon, null, false, false, null, false);
                    }
                    else
                    {
                        if (pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss))
                        {
                            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss));
                        }
                        /*
                        // Log.Message(string.Format("{0} At: {1}", pawn.Name, pawn.Position, pawn.Map));
                        // Log.Message(string.Format("{0} MentalState: {1}", pawn.Name, pawn.MentalState, pawn.Map));
                        // Log.Message(string.Format("{0} mindState: {1}", pawn.Name, pawn.mindState, pawn.Map));
                        // Log.Message(string.Format("{0} mindState duty: {1} thinkData: {2}", pawn.Name, pawn.mindState.duty, pawn.mindState.thinkData));
                        // Log.Message(string.Format("{0} mindState lastJobTag: {1} canFleeIndividual: {2}", pawn.Name, pawn.mindState.lastJobTag, pawn.mindState.canFleeIndividual));
                        // Log.Message(string.Format("{0} jobs: {1}", pawn.Name, pawn.jobs, pawn.Map));
                        // Log.Message(string.Format("{0} jobs curDriver: {1} curJob: {2}", pawn.Name, pawn.jobs.curDriver, pawn.jobs.curJob));
                        // Log.Message(string.Format("{0} jobs jobs: {1} jobs: {2}", pawn.Name, pawn.jobs.jobQueue, pawn.jobs.posture));
                        // Log.Message(string.Format("{0} CurJob: {1}", pawn.Name, pawn.CurJob, pawn.Map));
                        // Log.Message(string.Format("{0} CurJob lord: {1} def: {2}", pawn.Name, pawn.CurJob.lord, pawn.CurJob.def));
                        // Log.Message(string.Format("{0} CurJobDef: {1}", pawn.Name, pawn.CurJobDef, pawn.Map));
                        */
                    }
                }
            }
            Rand.PopState();
        }
    }
}
