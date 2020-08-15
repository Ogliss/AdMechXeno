using RimWorld;
using Verse;
using HarmonyLib;
using System.Linq;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    // Prevents disabled factions from generating in a new world
    [HarmonyPatch(typeof(FactionGenerator), "GenerateFactionsIntoWorld", null)]
    public static class AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch
    {
        // Token: 0x06000012 RID: 18 RVA: 0x000027D0 File Offset: 0x000017D0
        public static bool Prefix()
        {
            foreach (FactionDef factionDef in DefDatabase<FactionDef>.AllDefs)
            {
                if (!factionDef.isPlayer)
                {
                    string defName = factionDef.defName;
                    if (factionDef.defName.Contains("OG_Astartes_"))
                    {
                        if (!SettingsHelper.latest.AllowAdeptusAstartes)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Mechanicus_"))
                    {
                        if (!SettingsHelper.latest.AllowAdeptusMechanicus)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Militarum_"))
                    {
                        if (!SettingsHelper.latest.AllowAdeptusMilitarum)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Sororitas_"))
                    {
                        if (!SettingsHelper.latest.AllowAdeptusSororitas)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Chaos_"))
                    {
                        if (factionDef.defName.Contains("Deamon"))
                        {
                            if (!SettingsHelper.latest.AllowChaosDeamons)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                        if (factionDef.defName.Contains("Marine"))
                        {
                            if (!SettingsHelper.latest.AllowChaosMarine)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                        if (factionDef.defName.Contains("Guard"))
                        {
                            if (!SettingsHelper.latest.AllowChaosGuard)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                        if (factionDef.defName.Contains("Mechanicus"))
                        {
                            if (!SettingsHelper.latest.AllowChaosMechanicus)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                    }

                    if (factionDef.defName.Contains("OG_Eldar_"))
                    {
                        if (factionDef.defName.Contains("Craftworld"))
                        {
                            if (!SettingsHelper.latest.AllowEldarCraftworld)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                            if (!SettingsHelper.latest.AllowEldarWraithguard)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefRemoveRace(factionDef, OGEldarDefOf.OG_Eldar_Wraithguard_Race);
                            }
                        }
                        if (factionDef.defName.Contains("Exodite"))
                        {
                            if (!SettingsHelper.latest.AllowEldarExodite)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                        if (factionDef.defName.Contains("Harlequin"))
                        {
                            if (!SettingsHelper.latest.AllowEldarHarlequinn)
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                            }
                            else
                            {
                                AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                            }
                        }
                    }
                    if (factionDef.defName.Contains("OG_Dark_Eldar_"))
                    {
                        if (!SettingsHelper.latest.AllowDarkEldar)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Kroot_"))
                    {
                        if (!SettingsHelper.latest.AllowKroot)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Tau_"))
                    {
                        if (!SettingsHelper.latest.AllowTau)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }

                    if (factionDef.defName.Contains("OG_Necron_"))
                    {
                        if (!SettingsHelper.latest.AllowNecron)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Ork_Tek_"))
                    {
                        if (!SettingsHelper.latest.AllowOrkTek)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Ork_Feral_"))
                    {
                        if (!SettingsHelper.latest.AllowOrkFeral)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Ork_Hulk") || factionDef.defName.Contains("OG_Ork_Rok"))
                    {
                        if (!SettingsHelper.latest.AllowOrkRok)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    
                    if (factionDef.defName.Contains("OG_Tyranid_") || factionDef.defName.Contains("OG_Genestealer_Cult"))
                    {
                        if (!SettingsHelper.latest.AllowTyranid)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    if (factionDef.defName.Contains("OG_Vespid_") || factionDef.defName.Contains("OG_Vespid_Feral_"))
                    {
                        if (!SettingsHelper.latest.AllowVespid)
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 0);
                        }
                        else
                        {
                            AMXB_FactionGenerator_GenerateFactionsIntoWorld_Patch.UpdateDefrequiredCount(factionDef, 1);
                        }
                    }
                    
                }
            }
            return true;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002C2C File Offset: 0x00001C2C
        private static void UpdateDef(FactionDef def, int requiredCount)
        {
            def.requiredCountAtGameStart = requiredCount;
            if (def.requiredCountAtGameStart < 1)
            {
                def.maxCountAtGameStart = 0;
                return;
            }
            def.maxCountAtGameStart = 100;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002C2C File Offset: 0x00001C2C
        private static void UpdateDefrequiredCount(FactionDef def, int requiredCount)
        {
            def.requiredCountAtGameStart = requiredCount;
            if (def.requiredCountAtGameStart < 1)
            {
                def.maxCountAtGameStart = 0;
                def.canMakeRandomly = false;
                return;
            }
            def.maxCountAtGameStart = 100;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002C2C File Offset: 0x00001C2C
        private static void UpdateDefRemoveRace(FactionDef def, ThingDef raceDef)
        {
            if (def.pawnGroupMakers.Any(x => x.options.Any(y => y.kind.race == raceDef)))
            {
                foreach (PawnGroupMaker pgm in def.pawnGroupMakers.FindAll(x => x.options.Any(y => y.kind.race == raceDef)))
                {
                    foreach (var opt in pgm.options.FindAll(z => z.kind.race == raceDef))
                    {
                        pgm.options.Remove(opt);
                    }
                }
            }
        }
    }
}