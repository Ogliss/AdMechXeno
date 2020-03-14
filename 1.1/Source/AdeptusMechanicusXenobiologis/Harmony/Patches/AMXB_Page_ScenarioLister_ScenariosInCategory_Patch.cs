using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
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
                        if (SettingsHelper.latest.AllowAdeptusAstartes)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusSororitas)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.latest.AllowChaosDeamons)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.latest.AllowChaosGuard)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMarine)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMechanicus)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.latest.AllowDarkEldar)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.defName.Contains("Craftworld")))
                        {
                            if (SettingsHelper.latest.AllowEldarCraftworld)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Exodite")))
                        {
                            if (SettingsHelper.latest.AllowEldarExodite)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.latest.AllowEldarHarlequinn)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.latest.AllowTau)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.latest.AllowKroot)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.latest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef.scenario;
                        }
                    }
                    else if ((scenDef.defName.Contains("OG_Ork_") || scenDef.defName.Contains("OG_Grot_")))
                    {
                        if ((scenDef.defName.Contains("Tek")))
                        {
                            if (SettingsHelper.latest.AllowOrkTek)
                            {
                                yield return scenDef.scenario;
                            }
                        }
                        if ((scenDef.defName.Contains("Feral")))
                        {
                            if (SettingsHelper.latest.AllowOrkFeral)
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
                        if (SettingsHelper.latest.AllowAdeptusAstartes)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusSororitas)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.latest.AllowChaosDeamons)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.latest.AllowChaosGuard)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMarine)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.latest.AllowDarkEldar)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.name.Contains("Craftworld")))
                        {
                            if (SettingsHelper.latest.AllowEldarCraftworld)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Exodite")))
                        {
                            if (SettingsHelper.latest.AllowEldarExodite)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.latest.AllowEldarHarlequinn)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.latest.AllowTau)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.latest.AllowKroot)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.latest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Ork_") || scenDef.name.Contains("OG_Grot_")))
                    {
                        if ((scenDef.name.Contains("Tek")))
                        {
                            if (SettingsHelper.latest.AllowOrkTek)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Feral")))
                        {
                            if (SettingsHelper.latest.AllowOrkFeral)
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
                        if (SettingsHelper.latest.AllowAdeptusAstartes)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Militarum_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusMilitarum)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Sororitas_")))
                    {
                        if (SettingsHelper.latest.AllowAdeptusSororitas)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Deamons_")))
                    {
                        if (SettingsHelper.latest.AllowChaosDeamons)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Guard_")))
                    {
                        if (SettingsHelper.latest.AllowChaosGuard)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Marine_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMarine)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Choas_Mechanicus_")))
                    {
                        if (SettingsHelper.latest.AllowChaosMechanicus)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Dark_Eldar_")))
                    {
                        if (SettingsHelper.latest.AllowDarkEldar)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Eldar_")))
                    {
                        if ((scenDef.name.Contains("Craftworld")))
                        {
                            if (SettingsHelper.latest.AllowEldarCraftworld)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Exodite")))
                        {
                            if (SettingsHelper.latest.AllowEldarExodite)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Harlequinn")))
                        {
                            if (SettingsHelper.latest.AllowEldarHarlequinn)
                            {
                                yield return scenDef;
                            }
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Tau_")))
                    {
                        if (SettingsHelper.latest.AllowTau)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Kroot_")))
                    {
                        if (SettingsHelper.latest.AllowKroot)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Vespid_")))
                    {
                        if (SettingsHelper.latest.AllowVespidAuxiliaries)
                        {
                            yield return scenDef;
                        }
                    }
                    else if ((scenDef.name.Contains("OG_Ork_") || scenDef.name.Contains("OG_Grot_")))
                    {
                        if ((scenDef.name.Contains("Tek")))
                        {
                            if (SettingsHelper.latest.AllowOrkTek)
                            {
                                yield return scenDef;
                            }
                        }
                        if ((scenDef.name.Contains("Feral")))
                        {
                            if (SettingsHelper.latest.AllowOrkFeral)
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