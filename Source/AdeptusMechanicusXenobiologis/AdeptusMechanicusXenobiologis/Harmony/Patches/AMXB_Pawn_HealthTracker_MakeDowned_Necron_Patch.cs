using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Harmony;
using Verse.Sound;
using UnityEngine;
using System.Reflection;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.Harmony
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
    public static class AMXB_Pawn_HealthTracker_MakeDowned_Necron_Patch
    {
        public static FieldInfo pawn = typeof(Pawn_HealthTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        public static FieldInfo subgraphic = typeof(Pawn_HealthTracker).GetField("healthState", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        [HarmonyPostfix]
        public static void MakeDowned_Postfix(ref Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff)
        {
            if (__instance!=null)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)AM_PreApplyDamage_HediffComp_Shield_Patch.pawn.GetValue(__instance);
                if (pawn !=null)
                {
                    Comp_NecronOG _Necron = pawn.TryGetComp<Comp_NecronOG>();
                    if (_Necron!=null)
                    {
                        if (_Necron.originalWeapon != null)
                        {
                            if (_Necron.originalWeapon.Spawned)
                            {
                                _Necron.originalWeapon.DeSpawn();
                            }
                            pawn.inventory.innerContainer.TryAddOrTransfer(_Necron.originalWeapon);
                        }
                        if (_Necron.secondryWeapon != null)
                        {
                            if (_Necron.secondryWeapon.Spawned)
                            {
                                _Necron.secondryWeapon.DeSpawn();
                            }
                            pawn.inventory.innerContainer.TryAddOrTransfer(_Necron.secondryWeapon);
                        }
                    }
                }
            }
        }
    }
}
