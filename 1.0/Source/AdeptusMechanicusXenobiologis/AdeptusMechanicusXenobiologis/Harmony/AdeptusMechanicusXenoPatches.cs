﻿using RimWorld;
using Verse;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using System;
using Verse.AI;
using System.Text;
using System.Linq;
using Verse.AI.Group;
using RimWorld.Planet;
using UnityEngine;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    class AdeptusMechanicusXenoPatches
    {
        static AdeptusMechanicusXenoPatches()
        {
            //    HarmonyInstance.DEBUG = true;
            var harmony = HarmonyInstance.Create("com.ogliss.rimworld.mod.adeptusmechanicus.xenobiologis");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }
    }
    // 
    /*
    [HarmonyPatch(typeof(Corpse), "TickRare")]
    public static class Necron_Corpse_TickRare_Patch
    {
        [HarmonyPostfix]
        public static void TickRare_Patch(Corpse __instance)
        {
            if (__instance.InnerPawn.RaceProps.FleshType == OGNecronDefOf.OG_Flesh_Construct_Necron)
            {
                
                Comp_NecronOG _NecronOG = __instance.InnerPawn.TryGetComp<Comp_NecronOG>();
                if (_NecronOG!=null)
                {
                    Log.Message(string.Format("Necron Corpse: {0}", __instance.InnerPawn.def.defName));
                    _NecronOG.CompTickRare();
                }
                
            }
        }
    }
    */
    /*
    [HarmonyPatch(typeof(HediffUtility), "CanHealNaturally")]
    public static class Necron_HediffUtility_CanHealNaturally_Patch
    {
        [HarmonyPostfix] // Hediff_Injury hd
        public static void CanHealNaturally_Post(Hediff_Injury hd, ref bool __result)
        {
            if (hd.pawn.RaceProps.FleshType == OGNecronDefOf.Necron)
            {
                //    __result = hd.
            }
        }
    }
    */
    /*
    [HarmonyPatch(typeof(RaceProperties), "get_IsFlesh"), StaticConstructorOnStartup]
    public static class Necron_RaceProperties_IsFlesh_Patch
    {
        [HarmonyPostfix]
        public static void Necron_IsFlesh_Post(RaceProperties __instance, ref bool __result)
        {
            
            if (__instance.FleshType.defName.Contains("OG_Flesh_Construct_"))
            {
                __result = false;
                //    Log.Message(string.Format("OG_Flesh_Construct_ IsFlesh result = {0}", __result ));
            }

        }
    }
    */
}