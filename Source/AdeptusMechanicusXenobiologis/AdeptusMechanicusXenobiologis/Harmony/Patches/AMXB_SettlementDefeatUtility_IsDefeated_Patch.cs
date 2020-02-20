using RimWorld;
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
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus
{
    // SettlementDefeatUtility.IsDefeated
    [HarmonyPatch(typeof(SettlementDefeatUtility), "IsDefeated")]
    public static class AMXB_SettlementDefeatUtility_IsDefeated_Patch
    {
        [HarmonyPostfix]
        public static void IsDefeatedPostfix(Map map, Faction faction, ref bool __result)
        {
            if (__result)
            {
                if (faction.def.pawnGroupMakers.Any(x=>x.options.Any(y=> !y.kind.RaceProps.Humanlike)))
                {
                    List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Pawn pawn = list[i];
                        if (GenHostility.IsActiveThreatToPlayer(pawn))
                        {
                            __result = false;
                        }
                    }
                }
            }
        }
    }
}