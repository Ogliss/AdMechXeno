using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using System.Text.RegularExpressions;

namespace AdeptusMechanicus.HarmonyInstance
{

    [HarmonyPatch(typeof(BackCompatibility), "BackCompatibleDefName")]
    public static class BackCompatibility_BackCompatibleDefName_Patch
    {
        [HarmonyPostfix]
        public static void BackCompatibleDefName_Postfix(Type defType, string defName, bool forDefInjections, ref string __result)
        {
            if (GenDefDatabase.GetDefSilentFail(defType, defName, false) == null)
            {
                string newName = string.Empty;
            //    Log.Message(string.Format("Checking for replacement for {0} Type: {1}", defName, defType));
                if (defType == typeof(ThingDef))
                {

                }
                if (defType == typeof(FactionDef))
                {
                    if (defName == "TyranidFaction")
                    {
                        newName = "OG_Tyranid_Faction";
                    }
                }
                if (defType == typeof(PawnKindDef))
                {

                }
                if (defType == typeof(ResearchProjectDef))
                {

                }
                if (defType == typeof(HediffDef))
                {

                }
                if (defType == typeof(BodyDef))
                {

                }
                if (defType == typeof(ScenarioDef))
                {

                }
                if (!newName.NullOrEmpty())
                {
                    __result = newName;
                }
                if (defName == __result)
                {
                //    Log.Warning(string.Format("AMA No replacement found for: {0} T:{1}", defName, defType));
                }
                else
                {
                //    Log.Message(string.Format("Replacement found: {0} T:{1}", __result, defType));
                }
            }
        }
    }

}
