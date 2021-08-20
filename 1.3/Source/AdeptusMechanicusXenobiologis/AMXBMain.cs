using System;
using System.Collections.Generic;
using AdeptusMechanicus.settings;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class AMXBMain
    {
        static AMXBMain()
        {
            ThingDef thingDef = DefDatabase<ThingDef>.GetNamed("Human");
            AdeptusThingDefOf.OG_Human_Mechanicus.recipes = thingDef.recipes;
            //    Log.Message(string.Format("adding {0} to Mechanicus_Race", OGAdeptusMechanicusDefOf.OG_Human_Mechanicus.recipes.Count));
            /*
            foreach (var item in ModLister.AllInstalledMods)
            {
            //    log.message(string.Format("{0}", item));
            }
            */
            if (!AMAMod.settings.AllowTyranid || !AMAMod.settings.AllowTyranidWeapons || !AMAMod.settings.AllowTyranidInfestation)
            {
                AdeptusIncidentDefOf.OG_Tyranid_Infestation.baseChance = 0f;
                AdeptusIncidentDefOf.OG_Tyranid_Infestation.baseChanceWithRoyalty = 0f;
            }
            if (!AMAMod.settings.AllowChaosDeamons || !AMAMod.settings.AllowChaosDeamonicInfestation || !AMAMod.settings.AllowChaosDeamonicIncursion)
            {
                if (!AMAMod.settings.AllowChaosDeamons || !AMAMod.settings.AllowChaosDeamonicInfestation)
                {
                    AdeptusIncidentDefOf.OG_Chaos_Deamon_Daemonic_Infestation.baseChance = 0f;
                    AdeptusIncidentDefOf.OG_Chaos_Deamon_Daemonic_Infestation.baseChanceWithRoyalty = 0f;
                }
                if (!AMAMod.settings.AllowChaosDeamons || !AMAMod.settings.AllowChaosDeamonicIncursion)
                {
                    AdeptusIncidentDefOf.OG_Chaos_Deamon_Deamonic_Incursion.baseChance = 0f;
                    AdeptusIncidentDefOf.OG_Chaos_Deamon_Deamonic_Incursion.baseChanceWithRoyalty = 0f;
                }
            }
            if (!AMAMod.settings.AllowEldarWraithguard)
            {
                FactionDef fd = AdeptusFactionDefOf.OG_Eldar_Craftworld_Faction;
                ThingDef td = AdeptusThingDefOf.OG_Eldar_Wraithguard_Race;
                Filter(fd,td);
            }
        //    Log.Message(string.Format("post AllowEldarWraithguard"));
            if (!AMAMod.settings.AllowKrootAuxiliaries)
            {
                FactionDef fd = AdeptusFactionDefOf.OG_Tau_Faction;
                ThingDef td = AdeptusThingDefOf.OG_Alien_Kroot;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_Kroothound_Kindred;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_Knarloc_Kindred;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_KrootOx_Kindred;
                Filter(fd, td);
            }
        //    Log.Message(string.Format("post AllowKrootAuxiliaries"));
            if (!AMAMod.settings.AllowGueVesaAuxiliaries)
            {
                FactionDef fd = AdeptusFactionDefOf.OG_Tau_Faction;
                ThingDef td = ThingDefOf.Human;
                Filter(fd, td);
            }
        //    Log.Message(string.Format("post AllowGueVesaAuxiliaries"));
        }

        public static void Filter(FactionDef fd, ThingDef rd)
        {
            if (fd.pawnGroupMakers.Any(x => x.options.Any(y => y.kind.race == rd)))
            {
                int count = 0;
                foreach (PawnGroupMaker pgm in fd.pawnGroupMakers.FindAll(x => x.options.Any(y => y.kind.race == rd)))
                {
                    foreach (var opt in pgm.options.FindAll(z => z.kind.race == rd))
                    {
                        count++;
                        opt.selectionWeight = 0;
                    }
                    /*
                    for (int i = 0; i < pgm.options.Count; i++)
                    {
                        if (true)
                        {

                        }
                    }
                    */
                    /*
                    foreach (var opt in pgm.options.FindAll(z => z.kind.race == rd))
                    {
                        count++;
                        pgm.options.Remove(opt);
                    }
                    */
                }
            //    log.message("removed: " + count + " " + rd.LabelCap + " from: " + fd.LabelCap);
            }
            else
            {
            //    log.message("No " + rd.LabelCap + " from: " + fd.LabelCap + " found");
            }
        }
    }
}