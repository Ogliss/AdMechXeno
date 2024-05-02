using System;

namespace AdeptusMechanicus.settings
{
    // Token: 0x020001D0 RID: 464
    public class XenobiologisSettingsRef
    {
        // Imperial Options
        public bool AllowImperial = AMXBSettings.Instance.ShowImperium;
        // Imperial Factions
        public bool AllowAstartes = AMXBSettings.Instance.AllowAdeptusAstartes;
        public bool AllowMechanicus = AMXBSettings.Instance.AllowAdeptusMechanicus;
        public bool AllowMilitarum = AMXBSettings.Instance.AllowAdeptusMilitarum;
        public bool AllowSororitas = AMXBSettings.Instance.AllowAdeptusSororitas;
        // Imperial Sub-Factions

        // Imperial Incidents
        
        // Chaos Options
        public bool AllowChaos = AMXBSettings.Instance.ShowChaos;
        // Chaos Factions
        public bool AllowChaosMarine = AMXBSettings.Instance.AllowChaosMarine;
        public bool AllowChaosMechanicus = AMXBSettings.Instance.AllowChaosMechanicus;
        public bool AllowChaosGuard = AMXBSettings.Instance.AllowChaosGuard;
        public bool AllowChaosDeamons = AMXBSettings.Instance.AllowChaosDeamons;
        // Chaos Sub-Factions

        // Chaos Incidents
        public bool AllowChaosDeamonicIncursion = AMXBSettings.Instance.AllowChaosDeamonicIncursion;
        public bool AllowChaosDeamonicInfestation = AMXBSettings.Instance.AllowChaosDeamonicInfestation;
        public bool AllowWarpstorm = AMXBSettings.Instance.AllowWarpstorm;
        
        // Dark Eldar Options
        public bool AllowDarkEldar = AMXBSettings.Instance.AllowDarkEldar;
        // Dark Eldar Sub-Factions

        // Dark Eldar Incidents

        // Eldar Options
        public bool AllowEldar = AMXBSettings.Instance.AllowEldarCraftworld;
        // Eldar Sub-Factions
        public bool AllowEldarWraithguard = AMXBSettings.Instance.AllowEldarWraithguard;
        // Eldar Incidents


        // Tau Options
        public bool AllowTau = AMXBSettings.Instance.AllowTau;
        // Tau Sub-Factions
        public bool AllowKroot = AMXBSettings.Instance.AllowKroot;
        public bool AllowVespid = AMXBSettings.Instance.AllowVespidAuxiliaries;
        // Tau Incidents

        // Ork Options
        public bool AllowOrk = AMXBSettings.Instance.ShowOrk;
        // Ork Sub-Factions
        public bool AllowOrkTek = AMXBSettings.Instance.AllowOrkTek;
        public bool AllowOrkFeral = AMXBSettings.Instance.AllowOrkFeral;
        // Ork Incidents
        public bool AllowOrkRok = AMXBSettings.Instance.AllowOrkRok;

        // Necron Options
        public bool AllowNecron = AMXBSettings.Instance.ShowNecron;

        // Necron Sub-Factions

        // Necron Incidents
        public bool AllowNecronMonolith = AMXBSettings.Instance.AllowNecronMonolith;
        // Necron Special Rules
        public bool AllowNecronWellBeBack = AMXBSettings.Instance.AllowNecronWellBeBack;

        // Tyranid Options
        public bool AllowTyranid = AMXBSettings.Instance.AllowTyranid;
        // Tyranid Incidents
        public bool AllowTyranidInfestation = AMXBSettings.Instance.AllowTyranidInfestation;

    }
}
