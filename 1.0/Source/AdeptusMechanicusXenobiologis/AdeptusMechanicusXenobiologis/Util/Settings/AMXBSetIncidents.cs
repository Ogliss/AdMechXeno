using System;
using RimWorld;
using AdeptusMechanicus.settings;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000007 RID: 7
    public static class AMXBSetIncidents
    {
        // Token: 0x06000010 RID: 16 RVA: 0x000026AC File Offset: 0x000016AC
        public static void SetIncidentLevels()
        {
            foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefsListForReading)
            {
                if (incidentDef == OGNecronDefOf.OGN_MonolithAppears)
                {
                    if (SettingsHelper.XBlatest.AllowNecronMonolith)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
                if (incidentDef == OGChaosDeamonDefOf.OG_Chaos_Deamon_Warpstorm_Deamonic)
                {
                    if (SettingsHelper.XBlatest.AllowWarpstorm)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
                if (incidentDef == OGChaosDeamonDefOf.OG_Chaos_Deamon_Deamonic_Incursion)
                {
                    if (SettingsHelper.XBlatest.AllowChaosDeamonicIncursion)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
                if (incidentDef == OGChaosDeamonDefOf.OG_Chaos_Deamon_Daemonic_Infestation)
                {
                    if (SettingsHelper.XBlatest.AllowChaosDeamonicInfestation)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
                if (incidentDef == OGOrkDefOf.OG_Ork_Rok_Crash)
                {
                    if (SettingsHelper.XBlatest.AllowOrkRok)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
            }
        }
    }
}
