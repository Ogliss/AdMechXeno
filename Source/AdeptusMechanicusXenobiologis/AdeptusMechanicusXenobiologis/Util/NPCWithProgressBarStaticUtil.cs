using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public static class NPCWithProgressBarStaticUtil
    {
        // Token: 0x06003BF5 RID: 15349 RVA: 0x001C4174 File Offset: 0x001C2574
        public static Toil NPCWithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
        {
            Effecter effecter = null;
            toil.AddPreTickAction(delegate
            {
                /*
                if (toil.actor.Faction != Faction.OfPlayer)
                {
                    return;
                }
                */
                if (effecter == null)
                {
                    EffecterDef progressBar = EffecterDefOf.ProgressBar;
                    effecter = progressBar.Spawn();
                }
                else
                {
                    LocalTargetInfo target = toil.actor.CurJob.GetTarget(ind);
                    if (!target.IsValid || (target.HasThing && !target.Thing.Spawned))
                    {
                        effecter.EffectTick(toil.actor, TargetInfo.Invalid);
                    }
                    else if (interpolateBetweenActorAndTarget)
                    {
                        effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
                    }
                    else
                    {
                        effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
                    }
                    MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
                    if (mote != null)
                    {
                        mote.progress = Mathf.Clamp01(progressGetter());
                        mote.offsetZ = offsetZ;
                    }
                }
            });
            toil.AddFinishAction(delegate
            {
                if (effecter != null)
                {
                    effecter.Cleanup();
                    effecter = null;
                }
            });
            return toil;
        }

        // Token: 0x06003BF6 RID: 15350 RVA: 0x001C41E8 File Offset: 0x001C25E8
        public static Toil NPCWithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
        {
            return toil.NPCWithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
        }

        // Token: 0x06003BF7 RID: 15351 RVA: 0x001C421C File Offset: 0x001C261C
        public static Toil NPCWithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
        {
            return toil.NPCWithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
        }

    }
}
