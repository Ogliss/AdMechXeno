using Verse;

namespace AdeptusMechanicus.settings
{
    public class AMXBSettings : ModSettings
    {
        public bool  ForceRelations = true;

        public bool  ShowImperium = false, 
            AllowAdeptusAstartes = false, 
            AllowAdeptusMechanicus = true, 
            AllowAdeptusMilitarum = true, 
            AllowAdeptusSororitas = false;

        public bool ShowChaos = false, 
            AllowChaosMarine = false, 
            AllowChaosGuard = false, 
            AllowChaosMechanicus = false, 
            AllowWarpstorm = true, 
            AllowChaosDeamons = true, 
            AllowChaosDeamonicIncursion = true, 
            AllowChaosDeamonicInfestation = true;

        public bool ShowEldar = false, 
            AllowEldarCraftworld = true, 
            AllowEldarExodite = false, 
            AllowEldarHarlequinn = false, 
            AllowEldarWraithguard = true;

        public bool ShowDarkEldar = false,
            AllowDarkEldar = false;

        public bool ShowTau = false,
            AllowTau = true, 
            AllowKroot = true, 
            AllowKrootAuxiliaries = true, 
            AllowVespid = false, 
            AllowVespidAuxiliaries = false, 
            AllowGueVesaAuxiliaries = false;

        public bool ShowOrk = false, 
            AllowOrkTek = true, 
            AllowOrkFeral = true, 
            AllowOrkRok = true;

        public bool ShowNecron = false, 
            AllowNecron = true, 
            AllowNecronMonolith = true, 
            AllowNecronWellBeBack = true;

        public bool ShowTyranid = false, 
            AllowTyranid = false, 
            AllowTyranidInfestation = false; 

        public AMXBSettings()
        {
            AMXBSettings.Instance = this;
        }

        public static AMXBSettings Instance;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.ForceRelations, "AMXB_ForceRelations", true);
            Scribe_Values.Look(ref this.ShowImperium, "AMXB_AllowImperial", false);
            Scribe_Values.Look(ref this.AllowAdeptusAstartes, "AMXB_AllowAdeptusAstartes", false && AMSettings.Instance.AllowImperialWeapons);
            Scribe_Values.Look(ref this.AllowAdeptusMechanicus, "AMXB_AllowAdeptusMechanicus", true && AMSettings.Instance.AllowMechanicusWeapons);
            Scribe_Values.Look(ref this.AllowAdeptusMilitarum, "AMXB_AllowAdeptusMilitarum", true && AMSettings.Instance.AllowImperialWeapons);
            Scribe_Values.Look(ref this.AllowAdeptusSororitas, "AMXB_AllowAdeptusSororitas", false && AMSettings.Instance.AllowImperialWeapons);
            Scribe_Values.Look(ref this.ShowChaos, "AMXB_AllowChaos", false && AMSettings.Instance.AllowChaosWeapons);
            Scribe_Values.Look(ref this.AllowChaosMarine, "AMXB_AllowChaosMarine", false && AMSettings.Instance.AllowChaosWeapons);
            Scribe_Values.Look(ref this.AllowChaosGuard, "AMXB_AllowChaosGuard", false && AMSettings.Instance.AllowChaosWeapons);
            Scribe_Values.Look(ref this.AllowChaosMechanicus, "AMXB_AllowChaosMechanicus", false && AMSettings.Instance.AllowChaosWeapons);
            Scribe_Values.Look(ref this.AllowWarpstorm, "AMXB_AllowWarpstorm", true);
            Scribe_Values.Look(ref this.AllowChaosDeamons, "AMXB_AllowChaosDeamons", true);
            Scribe_Values.Look(ref this.AllowChaosDeamonicIncursion, "AMXB_AllowChaosDeamonicIncursion", true);
            Scribe_Values.Look(ref this.AllowChaosDeamonicInfestation, "AMXB_AllowChaosDeamonicInfestation", true);
            Scribe_Values.Look(ref this.ShowDarkEldar, "AMXB_AllowDarkEldar", false);
            Scribe_Values.Look(ref this.AllowDarkEldar, "AMXB_AllowDarkEldar", false && AMSettings.Instance.AllowDarkEldarWeapons);
            Scribe_Values.Look(ref this.ShowEldar, "AMXB_AllowEldar", false);
            Scribe_Values.Look(ref this.AllowEldarCraftworld, "AMXB_AllowEldar", true && AMSettings.Instance.AllowEldarWeapons);
            Scribe_Values.Look(ref this.AllowEldarExodite, "AMXB_AllowEldar", false && AMSettings.Instance.AllowEldarWeapons);
            Scribe_Values.Look(ref this.AllowEldarHarlequinn, "AMXB_AllowEldar", false && AMSettings.Instance.AllowEldarWeapons);
            Scribe_Values.Look(ref this.AllowEldarWraithguard, "AMXB_AllowEldarWraithguard", true && AMSettings.Instance.AllowEldarWeapons);
            Scribe_Values.Look(ref this.ShowTau, "AMXB_AllowTau", false);
            Scribe_Values.Look(ref this.AllowTau, "AMXB_AllowTau", true && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.AllowGueVesaAuxiliaries, "AMXB_AllowGueVesaAuxiliaries", true && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.AllowKrootAuxiliaries, "AMXB_AllowKrootAuxiliaries", true && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.AllowKroot, "AMXB_AllowKroot", true && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.AllowVespidAuxiliaries, "AMXB_AllowVespidAuxiliaries", false && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.AllowVespid, "AMXB_AllowVespid", false && AMSettings.Instance.AllowTauWeapons);
            Scribe_Values.Look(ref this.ShowOrk, "AMXB_AllowOrk", true);
            Scribe_Values.Look(ref this.AllowOrkTek, "AMXB_AllowOrkTek", true && AMSettings.Instance.AllowOrkWeapons);
            Scribe_Values.Look(ref this.AllowOrkFeral, "AMXB_AllowOrkFeral", true && AMSettings.Instance.AllowOrkWeapons);
            Scribe_Values.Look(ref this.AllowOrkRok, "AMXB_AllowOrkRok", true && AMSettings.Instance.AllowOrkWeapons);
            Scribe_Values.Look(ref this.ShowNecron, "AMXB_AllowNecron", true);
            Scribe_Values.Look(ref this.AllowNecron, "AMXB_AllowNecron", true && AMSettings.Instance.AllowNecronWeapons);
            Scribe_Values.Look(ref this.AllowNecronMonolith, "AMXB_AllowNecronMonolith", true && AMSettings.Instance.AllowNecronWeapons);
            Scribe_Values.Look(ref this.AllowNecronWellBeBack, "AMXB_AllowNecronWellBeBack", true && AMSettings.Instance.AllowNecronWeapons);
            Scribe_Values.Look(ref this.ShowTyranid, "AMXB_AllowTyranid", false && AMSettings.Instance.AllowTyranidWeapons);
            Scribe_Values.Look(ref this.AllowTyranid, "AMXB_AllowTyranid", false && AMSettings.Instance.AllowTyranidWeapons);
            Scribe_Values.Look(ref this.AllowTyranidInfestation, "AMXB_AllowTyranidInfestation", false && AMSettings.Instance.AllowTyranidWeapons);
        }


    }
}