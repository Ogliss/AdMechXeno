﻿using System;

namespace Verse.AI
{
    public abstract class ThinkNode_OGAMXBChancePerHour : ThinkNode_Priority
    {
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 100)
            {
                return ThinkResult.NoJob;
            }
            this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
            float num = this.MtbHours(pawn);
            if (num <= 0f)
            {
                return ThinkResult.NoJob;
            }
            Rand.PushState();
            int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
            Rand.Seed = pawn.RandSeedForHour(salt);
            bool flag = Rand.MTBEventOccurs(num, 250f, 250f);
            Rand.PopState();
            if (flag)
            {
                return base.TryIssueJobPackage(pawn, jobParams);
            }
            return ThinkResult.NoJob;
        }

        protected abstract float MtbHours(Pawn pawn);

        private int GetLastTryTick(Pawn pawn)
        {
            int result;
            if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
            {
                return result;
            }
            return -99999;
        }

        private void SetLastTryTick(Pawn pawn, int val)
        {
            pawn.mindState.thinkData[base.UniqueSaveKey] = val;
        }
    }
}
