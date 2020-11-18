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
            if (Prefs.DevMode) Log.Message(string.Format("Magos Xenobiologis: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))), false);
        }

        public static void SOSDeamonConstructPatch()
        {
            harmony.Patch(typeof(SaveOurShip2.ShipInteriorMod2).GetMethod("hasSpaceSuit"),new HarmonyMethod(typeof(AdeptusMechanicusXenoPatches), nameof(SOSDaemonSpaceSuitPostfix)));
        }

        private static bool SOSDaemonSpaceSuitPostfix(Pawn pawn, ref bool __result)
        {
            if (pawn.RaceProps.FleshType == OGChaosDeamonDefOf.OG_Flesh_Chaos_Deamon || pawn.RaceProps.FleshType.defName.Contains("OG_Flesh_Construct"))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

}