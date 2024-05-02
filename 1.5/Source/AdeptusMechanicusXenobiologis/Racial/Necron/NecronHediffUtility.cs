using System;
using System.Collections.Generic;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000D7C RID: 3452
    public static class NecronHediffUtility
    {
        // Token: 0x06004CA5 RID: 19621 RVA: 0x0023A37C File Offset: 0x0023877C
        public static T TryGetCompFast<T>(this Hediff hd) where T : HediffComp
        {
            HediffWithComps hediffWithComps = hd as HediffWithComps;
            if (hediffWithComps == null)
            {
                return (T)((object)null);
            }
            if (hediffWithComps.comps != null)
            {
                for (int i = 0; i < hediffWithComps.comps.Count; i++)
                {
                    T t = hediffWithComps.comps[i] as T;
                    if (t != null)
                    {
                        return t;
                    }
                }
            }
            return (T)((object)null);
        }

        // Token: 0x06004CA6 RID: 19622 RVA: 0x0023A3F0 File Offset: 0x002387F0
        public static bool IsTended(this Hediff hd)
        {
            HediffWithComps hediffWithComps = hd as HediffWithComps;
            if (hediffWithComps == null)
            {
                return false;
            }
            HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetCompFast<HediffComp_TendDuration>();
            return hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended;
        }

        // Token: 0x06004CA7 RID: 19623 RVA: 0x0023A424 File Offset: 0x00238824
        public static bool IsPermanent(this Hediff hd)
        {
            HediffWithComps hediffWithComps = hd as HediffWithComps;
            if (hediffWithComps == null)
            {
                return false;
            }
            HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetCompFast<HediffComp_GetsPermanent>();
            return hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent;
        }

        // Token: 0x06004CA8 RID: 19624 RVA: 0x0023A458 File Offset: 0x00238858
        public static bool FullyImmune(this Hediff hd)
        {
            HediffWithComps hediffWithComps = hd as HediffWithComps;
            if (hediffWithComps == null)
            {
                return false;
            }
            HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetCompFast<HediffComp_Immunizable>();
            return hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune;
        }

        // Token: 0x06004CA9 RID: 19625 RVA: 0x0023A489 File Offset: 0x00238889
        public static bool CanHealFromTending(this Hediff_Injury hd)
        {
            return hd.IsTended() && !hd.IsPermanent();
        }

        // Token: 0x06004CAA RID: 19626 RVA: 0x0023A4A2 File Offset: 0x002388A2
        public static bool CanHealNaturally(this Hediff_Injury hd)
        {
            return !hd.IsPermanent();
        }

        // Token: 0x06004CAB RID: 19627 RVA: 0x0023A4B0 File Offset: 0x002388B0
        public static int CountAddedParts(this HediffSet hs)
        {
            int num = 0;
            List<Hediff> hediffs = hs.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i] is Hediff_AddedPart)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
