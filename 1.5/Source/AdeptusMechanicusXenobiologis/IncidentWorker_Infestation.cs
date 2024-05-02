using AdeptusMechanicus.settings;
using RimWorld;
using Verse;

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
}