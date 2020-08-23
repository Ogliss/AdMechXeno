using RimWorld;
using Verse;
using HarmonyLib;

namespace AdeptusMechanicus.HarmonyInstance
{
    // Disallows stripping of the Wristblade
    [HarmonyPatch(typeof(Pawn), "AnythingToStrip")]
    public static class Pawn_AnythingToStrip_Necron_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref bool __result)
        {
            __result = __result && __instance.RaceProps.FleshType!=OGNecronDefOf.OG_Flesh_Construct_Necron;

        }
    }
    /*
    // ShotReport.AimOnTargetChance_StandardTarget ShotReport
    [HarmonyPatch(typeof(ShotReport), "get_AimOnTargetChance_StandardTarget")]
    public static class AM_ShotReport_GetAimOnTargetChance_StandardTarget_Necron_Wraith_Patch
    {
        [HarmonyPostfix]
        public static void Get_AimOnTargetChance_Necron_Wraith_Postfix(ref ShotReport __instance, ref LocalTargetInfo ___target, ref float ___distance, ref float __result)
        {
            if (___target != null)
            {
                if (___target.HasThing)
                {
                    if (___target.Thing.def.thingClass == typeof(Pawn))
                    {
                        Pawn pawn = (Pawn)___target.Thing;
                        if (pawn.kindDef == OGNecronDefOf.OG_Necron_Wraith)
                        {
                            if (___distance > 5f && __result > 0.1f)
                            {
                                __result = __result * 0.1f;
                            }
                        }
                    }
                }
            }
        }
    }
    */

    /*
    // ShotReport.HitReportFor
    [HarmonyPatch(typeof(ShotReport), "HitReportFor")]
    public static class AM_ShotReport_HitReportFor_Necron_Wraith_Patch
    {
        public static FieldInfo factorFromShooterAndDist = typeof(ShotReport).GetField("factorFromShooterAndDist", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        [HarmonyPostfix]
        public static void HitReportFor_Necron_Wraith_Postfix(Thing caster, Verb verb, LocalTargetInfo target, ref ShotReport __result)
        {
            Traverse traverse = Traverse.Create(__result);
            float factorFromShooterAndDist = (float)AM_ShotReport_HitReportFor_Necron_Wraith_Patch.factorFromShooterAndDist.GetValue(__result);
            if (target!=null)
            {
                if (target.HasThing)
                {
                    if (target.Thing is Pawn Victim)
                    {
                        if (Victim.kindDef == OGNecronDefOf.OG_Necron_Wraith)
                        {
                        //    log.message(string.Format("factorFromShooterAndDist {0}", factorFromShooterAndDist));
                            if (!caster.Position.InHorDistOf(Victim.Position, 7) && factorFromShooterAndDist > 0.1f)
                            {

                                AM_ShotReport_HitReportFor_Necron_Wraith_Patch.factorFromShooterAndDist.SetValue(__result, 0.1f);
                                factorFromShooterAndDist = (float)AM_ShotReport_HitReportFor_Necron_Wraith_Patch.factorFromShooterAndDist.GetValue(__result);
                                //    ___factorFromShooterAndDist = 0.1f;
                            //    log.message(string.Format("set to ___factorFromShooterAndDist {0}", factorFromShooterAndDist));
                            }
                        }
                    }
                }
            }
        }
    }
    */
    /*
    // CoverUtility.CalculateOverallBlockChance
    [HarmonyPatch(typeof(CoverUtility), "CalculateOverallBlockChance")]
    public static class AM_CoverUtility_Get_CalculateOverallBlockChance_Necron_Wraith_Patch
    {
        [HarmonyPostfix]
        public static void CalculateOverallBlockChance_Necron_Wraith_Postfix(LocalTargetInfo target, IntVec3 shooterLoc, Map map, ref float __result)
        {
            if (target.Thing!=null)
            {
                if (target.Thing is Pawn pawn)
                {
                    if (pawn.kindDef == OGNecronDefOf.OG_Necron_Wraith)
                    {
                        if (!target.Cell.InHorDistOf(shooterLoc, 5) && __result<0.9f)
                        {
                            __result = 0.9f;
                        //    log.message("wraiths are hard to hit at this distance");
                        }
                    }
                }
            }
        }
    }
    */
}