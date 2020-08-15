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
    // ScenarioLister.ScenariosInCategory
    [HarmonyPatch(typeof(ScenarioLister), "ScenariosInCategory")]
    public static class AMXB_Page_ScenarioLister_ScenariosInCategory_Patch
    {
        [HarmonyPostfix]
        public static IEnumerable<Scenario> ScenariosInCategoryPrefix(IEnumerable<Scenario> scenario, ScenarioCategory cat)
        {
            if (cat == ScenarioCategory.FromDef)
            {
                IEnumerable<ScenarioDef> scenarios = null;
                scenarios = DefDatabase<ScenarioDef>.AllDefs;
                foreach (ScenarioDef scenDef in DefDatabase<ScenarioDef>.AllDefs)
                {
                    if ((scenDef.defName.Contains("OG_Astartes_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusAstartes)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusSororitas)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosDeamons)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosGuard)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMarine)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMechanicus)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.XBlatest.AllowDarkEldar)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.defName.Contains("Craftworld")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarCraftworld)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Exodite")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarExodite)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarHarlequinn)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.XBlatest.AllowTau)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.XBlatest.AllowKroot)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.XBlatest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Ork_") || scenDef.defName.Contains("OG_Grot_")))
                    {
                        if ((scenDef.defName.Contains("Tek")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkTek)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Feral")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkFeral)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                    }
                    else
                    {
                        yield return scenDef.scenario;
                    }

                }
            }
            else if (cat == ScenarioCategory.CustomLocal)
            {
                IEnumerable<Scenario> scenarios = ScenarioFiles.AllScenariosLocal;
                foreach (Scenario scenDef in scenarios)
                {
                    if ((scenDef.name.Contains("OG_Astartes_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusAstartes)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusSororitas)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosDeamons)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosGuard)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMarine)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.XBlatest.AllowDarkEldar)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.name.Contains("Craftworld")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarCraftworld)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Exodite")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarExodite)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarHarlequinn)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.XBlatest.AllowTau)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.XBlatest.AllowKroot)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.XBlatest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Ork_") || scenDef.name.Contains("OG_Grot_")))
                    {
                        if ((scenDef.name.Contains("Tek")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkTek)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Feral")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkFeral)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else
                    {
                        yield return scenDef;
                    }
                }
            }
            else if (cat == ScenarioCategory.SteamWorkshop)
            {
                IEnumerable<Scenario> scenarios = ScenarioFiles.AllScenariosWorkshop;
                foreach (Scenario scenDef in scenarios)
                {
                    if ((scenDef.name.Contains("OG_Astartes_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusAstartes)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.XBlatest.AllowAdeptusSororitas)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosDeamons)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosGuard)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMarine)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.XBlatest.AllowChaosMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.XBlatest.AllowDarkEldar)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.name.Contains("Craftworld")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarCraftworld)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Exodite")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarExodite)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.XBlatest.AllowEldarHarlequinn)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.XBlatest.AllowTau)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.XBlatest.AllowKroot)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.XBlatest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Ork_") || scenDef.name.Contains("OG_Grot_")))
                    {
                        if ((scenDef.name.Contains("Tek")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkTek)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Feral")))
                        {
                            if (SettingsHelper.XBlatest.AllowOrkFeral)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else
                    {
                        yield return scenDef;
                    }
                }
            }
            yield break;

        }
    }
}