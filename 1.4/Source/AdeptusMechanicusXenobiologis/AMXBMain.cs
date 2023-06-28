using System;
using System.Collections.Generic;
using AdeptusMechanicus.settings;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.IncidentWorker_Infestation
    public class IncidentWorker_Infestation : ExtraHives.IncidentWorker_Infestation
    {

        public override bool CanFireNowSub(IncidentParms parms)
        {
            if (this.def == AdeptusIncidentDefOf.OG_Tyranid_Infestation)
            {
                if (Prefs.DevMode) Log.Message("OG_Tyranid_Infestation");
                return AMAMod.settings.AllowTyranidInfestation && base.CanFireNowSub(parms);
            }
            if (this.def == AdeptusIncidentDefOf.OG_Chaos_Deamon_Daemonic_Infestation)
            {
                if (Prefs.DevMode) Log.Message("OG_Chaos_Deamon_Daemonic_Infestation");
                return AMAMod.settings.AllowChaosDeamonicInfestation && base.CanFireNowSub(parms);
            }
            if (this.def == AdeptusIncidentDefOf.OG_Chaos_Deamon_Deamonic_Incursion)
            {
                if (Prefs.DevMode) Log.Message("OG_Chaos_Deamon_Deamonic_Incursion");
                return AMAMod.settings.AllowChaosDeamonicIncursion && base.CanFireNowSub(parms);
            }
            return base.CanFireNowSub(parms);
        }

        public override bool TryExecuteWorker(IncidentParms parms)
        {
            if (this.def == AdeptusIncidentDefOf.OG_Tyranid_Infestation)
            {
                return AMAMod.settings.AllowTyranidInfestation && base.TryExecuteWorker(parms);
            }
            if (this.def == AdeptusIncidentDefOf.OG_Chaos_Deamon_Daemonic_Infestation)
            {
                return AMAMod.settings.AllowChaosDeamonicInfestation && base.TryExecuteWorker(parms);
            }
            if (this.def == AdeptusIncidentDefOf.OG_Chaos_Deamon_Deamonic_Incursion)
            {
                return AMAMod.settings.AllowChaosDeamonicIncursion && base.TryExecuteWorker(parms);
            }
            return base.TryExecuteWorker(parms);
        }
    }


    [StaticConstructorOnStartup]
    public class AMXBMain
    {
        static AMXBMain()
        {
            ThingDef thingDef = DefDatabase<ThingDef>.GetNamed("Human");
            AdeptusThingDefOf.OG_Human_Mechanicus.recipes = thingDef.recipes;
            Update();
            //    Log.Message(string.Format("adding {0} to Mechanicus_Race", OGAdeptusMechanicusDefOf.OG_Human_Mechanicus.recipes.Count));
            /*
            foreach (var item in ModLister.AllInstalledMods)
            {
            //    log.message(string.Format("{0}", item));
            }
            */
            //    Log.Message(string.Format("post AllowGueVesaAuxiliaries"));
        }

        public static void Update()
        {
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
                Filter(fd, td);
            }
            //    Log.Message(string.Format("post AllowEldarWraithguard"));
            if (!AMAMod.settings.AllowKrootAuxiliaries)
            {
                FactionDef fd = AdeptusFactionDefOf.OG_Tau_Faction;
                ThingDef td = AdeptusThingDefOf.OG_Alien_Kroot;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_Kroothound;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_Knarloc;
                Filter(fd, td);
                td = AdeptusThingDefOf.OG_KrootOx;
                Filter(fd, td);
            }
            //    Log.Message(string.Format("post AllowKrootAuxiliaries"));
            if (!AMAMod.settings.AllowGueVesaAuxiliaries)
            {
                FactionDef fd = AdeptusFactionDefOf.OG_Tau_Faction;
                ThingDef td = ThingDefOf.Human;
                Filter(fd, td);
            }
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