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
            OGAdeptusMechanicusDefOf.OG_Human_Mechanicus.recipes = thingDef.recipes;
        //    Log.Message(string.Format("adding {0} to Mechanicus_Race", OGAdeptusMechanicusDefOf.OG_Human_Mechanicus.recipes.Count));
            /*
            foreach (var item in ModLister.AllInstalledMods)
            {
                Log.Message(string.Format("{0}", item));
            }
            */
            if (!SettingsHelper.XBlatest.AllowEldarWraithguard)
            {
                FactionDef fd = OGEldarDefOf.OG_Eldar_Craftworld_Faction;
                ThingDef td = OGEldarDefOf.OG_Eldar_Wraithguard_Race;
                Filter(fd,td);
            }
        //    Log.Message(string.Format("post AllowEldarWraithguard"));
            if (!SettingsHelper.XBlatest.AllowKrootAuxiliaries)
            {
                FactionDef fd = OGTauDefOf.OG_Tau_Faction;
                ThingDef td = OGKrootDefOf.Tau_Kroot_Warrior;
                Filter(fd, td);
                td = OGKrootDefOf.KindredKrootHound;
                Filter(fd, td);
                td = OGKrootDefOf.KindredKrootKnarloc;
                Filter(fd, td);
                td = OGKrootDefOf.KindredKrootOx;
                Filter(fd, td);
            }
        //    Log.Message(string.Format("post AllowKrootAuxiliaries"));
            if (!SettingsHelper.XBlatest.AllowGueVesaAuxiliaries)
            {
                FactionDef fd = OGTauDefOf.OG_Tau_Faction;
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
                Log.Message("removed: " + count + " " + rd.LabelCap + " from: " + fd.LabelCap);
            }
            else
            {
                Log.Message("No " + rd.LabelCap + " from: " + fd.LabelCap + " found");
            }
        }
    }
}