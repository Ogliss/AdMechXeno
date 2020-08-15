using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace AdeptusMechanicus
{
    static class Util
    {
        private static bool initialized = false;
        private static Faction AstraChaosMarine = null;
        private static Faction AstraTraitorGuard = null;
        private static Faction AstraTyranid = null;

        public static bool TryGetAstraFactions(out Faction AstraChaosMarine, out Faction AstraTraitorGuard, out Faction AstraTyranid)
        {
            if (!initialized)
            {
                initialized = true;
                Initialized();
            }

            AstraChaosMarine = null;
            AstraTraitorGuard = null;
            AstraTyranid = null;
            return false;
        }

        private static void Initialized()
        {
            foreach (ModContentPack p in LoadedModManager.RunningMods)
            {
                string name;
                if (p.Name.Contains("Astra Militarum Imperial Guard Factions Mod"))
                {
                    name = p.Name;
                    if (p.Name.IndexOf(name) != -1)
                    {
                        AstraChaosMarine = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("IG_ChaosMarineFaction"));
                        Log.Message(string.Format("{0}", AstraChaosMarine));
                        AstraTraitorGuard = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("TraitorGuardFaction"));
                        Log.Message(string.Format("{0}", AstraTraitorGuard));
                        AstraTyranid = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("TyranidFaction"));
                        Log.Message(string.Format("{0}", AstraTyranid));
                    }
                }
            }
        }
    }
}