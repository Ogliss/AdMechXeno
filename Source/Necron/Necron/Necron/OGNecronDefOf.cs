using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGNecronDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGNecronDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGNecronDefOf));
        }

        //    public static HediffDef RRY_FaceHuggerInfection;
        //    public static HediffDef RRY_XenomorphImpregnation;

        public static FactionDef NecronFaction;

        public static ThingDef MonolithIncoming;

        public static PawnKindDef NecronScarabSwarm;
        public static PawnKindDef NecronDestroyer;
        public static PawnKindDef NecronImmortal;
        public static PawnKindDef NecronWarrior;
        public static PawnKindDef NecronLord;
        
    }
}
