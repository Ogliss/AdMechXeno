﻿using RimWorld;
using Verse;
using HarmonyLib;
using System;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    // DamageWorker_AddInjury.FinalizeAndAddInjury
    [HarmonyPatch(typeof(DamageWorker_AddInjury), "FinalizeAndAddInjury", new Type[] { typeof(Pawn), typeof(Hediff_Injury), typeof(DamageInfo), typeof(DamageWorker.DamageResult) })]
    public static class DamageWorker_AddInjury_FinalizeAndAddInjury_NecronWellBeBack_Patch
    {
        [HarmonyPostfix]
        public static void FinalizeAndAddInjuryPostfix(DamageWorker_AddInjury __instance, Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
        {
            if (pawn!=null)
            {
                if (injury!=null)
                {
                    Comp_NecronOG _Necron = pawn.TryGetCompFast<Comp_NecronOG>();
                    if (_Necron != null)
                    {
                        DamageInfo dInfo = dinfo;
                        DamageDef damageDef = dinfo.Def;
                        float AP = dinfo.ArmorPenetrationInt;
                        float AV = (pawn.kindDef.race.statBases.GetStatValueFromList(dinfo.Def.armorCategory.armorRatingStat, 0f));
                        bool healable = AP < AV;
                        if (healable)
                        {
                            _Necron.healableHediffs.Add(injury);
                        }
                        else
                        {
                            _Necron.unhealableHediffs.Add(injury);
                        }
                    //    Log.Message(string.Format("{1} got a {2} AP: {7} on its {3} AV:{6}, healable: {0}, total Healable: {5}, Unhealable: {4}", healable, pawn.Label, injury.Label, injury.Part, _Necron.unhealableHediffs.Count, _Necron.healableHediffs.Count, AV, AP));
                    }
                }
            }
        }
    }
}