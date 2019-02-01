using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000956 RID: 2390
    [DefOf]
    public static class OGChaosDeamonDefOf
    {
        // Token: 0x06003781 RID: 14209 RVA: 0x001A8393 File Offset: 0x001A6793
        static OGChaosDeamonDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGChaosDeamonDefOf));
        }

        // Deamon ThingDefs
        public static ThingDef Warpfire;
        public static ThingDef WarpSpark;
        public static ThingDef Mote_MicroSparksWarp;
        public static ThingDef Mote_WarpFireGlow;
        public static ThingDef OGCDWarpTunnel;

        // Deamon FactionDefs
        public static FactionDef OGChaosDeamonFaction;

        // Deamon DamageDefs
        public static DamageDef OGCDWarpfire;
        public static DamageDef OGCDWarpfireDeath;
        //   public static DamageDef OGCDWarpStorm;
        public static DamageDef OGCDWarpStormStrike;

        // Deamon HediffDefs
        //    public static HediffDef RRY_FaceHuggerInfection;
        //    public static HediffDef RRY_XenomorphImpregnation;

        // Deamon MentalStateDefs
        public static MentalStateDef MentalState_OGChaosDeamon;

        // Deamon GameConditionDefs
        public static GameConditionDef Warpstorm;

        // Deamon PawnKindDefs
        // Deamons of Tzeentch
        public static PawnKindDef ChaosDeamonFlamerExalted;
        public static PawnKindDef ChaosDeamonFlamer;
        public static PawnKindDef ChaosDeamonHorrorPink;
        public static PawnKindDef ChaosDeamonHorrorBlue;
        public static PawnKindDef ChaosDeamonHorrorBrimstone;
        public static PawnKindDef ChaosDeamonScreamer;

        // Deamons of Nurgle
        public static PawnKindDef ChaosDeamonNurgling;
        public static PawnKindDef ChaosDeamonPlagueBearer;
        //   public static PawnKindDef ChaosDeamonGreatUNcleanOne;

        // Deamons of Slaanesh
        public static PawnKindDef ChaosDeamonDeamonette;
        //   public static PawnKindDef ChaosDeamonMountedDeamonette;
        //   public static PawnKindDef ChaosDeamonScreamer;
        //   public static PawnKindDef ChaosDeamonKeeperOfSecrets;

        // Deamons of Khrone
        //   public static PawnKindDef ChaosDeamonHoundofKhorne;
        //   public static PawnKindDef ChaosDeamonBloodletter;
        //   public static PawnKindDef ChaosDeamonBloodthirster;
        //   public static PawnKindDef ChaosDeamonJuggernaughtofKhorne;

    }
}
