using Verse;
using System.Reflection;
using HarmonyLib;
using System.Linq;
using System;
using RimWorld;
using System.Collections.Generic;

namespace AdeptusMechanicus.HarmonyInstance
{
    [StaticConstructorOnStartup]
    public class AdeptusMechanicusXenoPatches
    {
        public static Harmony harmony;
        static AdeptusMechanicusXenoPatches()
        {

            harmony = new Harmony("com.ogliss.rimworld.mod.adeptus.xenobiologis");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (AdeptusIntergrationUtility.enabled_SOS2)
            {
                SOSDeamonConstructPatch();
            }
            if (AdeptusIntergrationUtility.enabled_EndTimesWithGuns)
            {
                EndTimesPortalPatch();
            }
            if (Prefs.DevMode) Log.Message(string.Format("Magos Xenobiologis: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))));
        }

        public static void EndTimesPortalPatch()
        {
            MethodInfo method3 = AccessTools.TypeByName("TheEndTimes.ChaosPortalGreat").GetMethod("ResetStaticData");
            MethodInfo method4 = typeof(AdeptusMechanicusXenoPatches).GetMethod("EndTimesWithGunsDeamonPatch");
            MethodInfo method5 = AccessTools.TypeByName("TheEndTimes.ChaosPortalSmall").GetMethod("ResetStaticData");
            if (method3 != null || method5 != null)
            {
            //    Log.Message("Rimhammer: End Times WITH GUNS detected");
                if (settings.AMSettings.Instance.EndTimesIntergrateDeamons)
                {
                    if (method3 == null)
                    {
                        Log.Warning("TheEndTimes.ChaosPortalGreat ResetStaticData method null");
                    }
                    else
                    {
                        if (settings.AMSettings.Instance.EndTimesIntergrateDeamonsGreat)
                        {
                            if (harmony.Patch(method3, postfix: new HarmonyMethod(method4)) != null)
                            {
                            //    Log.Message("Magos Xenobiologis: successfully added chaos deamons to End Times Great Portal spawns");
                            }
                            else
                            {
                                Log.Warning("Magos Xenobiologis: Failed to add chaos deamons to End Times Great Portal spawns");
                            }
                        }
                    }
                    if (method5 == null)
                    {
                        Log.Warning("TheEndTimes.ChaosPortalSmall ResetStaticData method null");
                    }
                    else
                    {
                        if (settings.AMSettings.Instance.EndTimesIntergrateDeamonsSmall)
                        {
                            if (harmony.Patch(method5, postfix: new HarmonyMethod(method4)) != null)
                            {
                            //    Log.Message("Magos Xenobiologis: successfully added chaos deamons to End Times Small Portal spawns");
                            }
                            else
                            {
                                Log.Warning("Magos Xenobiologis: Failed to add chaos deamons to End Times Small Portal spawns");
                            }
                        }
                    }
                }
            }
        }
        public static void EndTimesWithGunsDeamonPatch(Building __instance)
        {
            if (__instance is TheEndTimes.ChaosPortalGreat PortalGreat)
            {
                foreach (PawnKindDef item in DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.RaceProps.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Chaos_Deamon))
                {
                    TheEndTimes.ChaosPortalGreat.spawnablePawnKinds.Add(item);
                }
                return;
            }
            if (__instance is TheEndTimes.ChaosPortalSmall PortalSmall)
            {
                foreach (PawnKindDef item in DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.RaceProps.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Chaos_Deamon))
                {
                    TheEndTimes.ChaosPortalSmall.spawnablePawnKinds.Add(item);
                }
                return;
            }
        }

        public static void SOSDeamonConstructPatch()
        {
            harmony.Patch(typeof(SaveOurShip2.ShipInteriorMod2).GetMethod("hasSpaceSuit"),new HarmonyMethod(typeof(AdeptusMechanicusXenoPatches), nameof(SOSDaemonSpaceSuitPostfix)));
        }

        private static bool SOSDaemonSpaceSuitPostfix(Pawn pawn, ref bool __result)
        {
            if (pawn.RaceProps.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Chaos_Deamon || pawn.RaceProps.FleshType.defName.Contains("OG_Flesh_Construct"))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

}