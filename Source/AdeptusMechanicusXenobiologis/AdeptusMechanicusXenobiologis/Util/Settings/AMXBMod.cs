using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;
using RimWorld;
using AdeptusMechanicus.settings; 

namespace AdeptusMechanicus.settings
{

    class AMXBMod : Mod
    {
        public static AMXBMod XBInstance;
        public AMXBSettings XBsettings;
        public AMXBMod(ModContentPack content) : base(content)
        {
            this.XBsettings = GetSettings<AMXBSettings>();
            SettingsHelper.XBlatest = this.XBsettings;
            AMXBMod.XBInstance = this;
            AMXBSettings.Instance = base.GetSettings<AMXBSettings>();
        }

        /*
        public override string SettingsCategory() => "AM_ModSeries".Translate() + ": " + "AMXB_ModName".Translate();
        
        public override void DoSettingsWindowContents(Rect inRect)
        {
        //    base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            float num = 800f;
            float num2 = 14.5f + (XBsettings.ShowImperium ? 4 : 0) + (XBsettings.ShowChaos ? 7 : 0) + (XBsettings.ShowEldar ? 1 : 0) + (XBsettings.ShowTau ? 1 : 0) + (XBsettings.ShowOrk ? 3 : 0) + (XBsettings.ShowNecron ? 2 : 0) + (XBsettings.ShowTyranid ? 1 : 0);
            Rect rect = new Rect(inRect.x, inRect.y-50, num, num2 * 22f);
            listing_Standard.Label("AMXB_AllowedRaces".Translate());
            listing_Standard.BeginScrollView(inRect.ContractedBy(10), ref pos, ref rect);
            listing_Standard.CheckboxLabeled("AMXB_ForceRelations".Translate(), ref XBsettings.ForceRelations, "AMXB_ForceRelationsDesc".Translate());
            listing_Standard.CheckboxLabeled("AMXB_ShowImperium".Translate(), ref XBsettings.ShowImperium, "AMXB_ShowImperiumDesc".Translate());
            Rect rectImperium = new Rect(rect.x, rect.y+10, num, 60f);
            if (XBsettings.ShowImperium)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectImperium.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusAstartes".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowAdeptusAstartes, !DefDatabase<FactionDef>.AllDefs.Any(x=>x.defName.Contains("OG_Astartes")) || !AMASettings.Instance.AllowImperialWeapons);
                Widgets.CheckboxLabeled(rectImperium.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowAdeptusMechanicus, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) || !AMASettings.Instance.AllowMechanicusWeapons);
                Widgets.CheckboxLabeled(rectImperium.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusMilitarum".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowAdeptusMilitarum, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) || !AMASettings.Instance.AllowImperialWeapons);
                Widgets.CheckboxLabeled(rectImperium.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusSororitas".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowAdeptusSororitas, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) || !AMASettings.Instance.AllowImperialWeapons);
                listing_Standard.EndSection(listing_Standard);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowChaos".Translate(), ref XBsettings.ShowChaos);
            Rect rectChaos = new Rect(rect.x, rect.y + 10, num, 120f);
            if (XBsettings.ShowChaos)
            {
                listing_Standard.BeginSection(120f);
                Widgets.CheckboxLabeled(rectChaos.TopHalf().TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosMarine".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowChaosMarine, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")));
                Widgets.CheckboxLabeled(rectChaos.TopHalf().BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosGuard".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowChaosGuard, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")));
                Widgets.CheckboxLabeled(rectChaos.TopHalf().TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowChaosMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowChaosMechanicus, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")));
                Widgets.CheckboxLabeled(rectChaos.TopHalf().BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowChaosDeamons".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowChaosDeamons, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));
                Widgets.CheckboxLabeled(rectChaos.BottomHalf().TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosDeamonicIncursion".Translate(), ref XBsettings.AllowChaosDeamonicIncursion, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")));
                Widgets.CheckboxLabeled(rectChaos.BottomHalf().BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosDeamonicInfestation".Translate(), ref XBsettings.AllowChaosDeamonicInfestation, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")));
                listing_Standard.EndSection(listing_Standard);
            }
            if (DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")))
            {
                listing_Standard.CheckboxLabeled("AMXB_ShowDarkEldar".Translate(), ref XBsettings.ShowDarkEldar);
            }
            else
            {
                listing_Standard.Label("AMXB_ShowDarkEldar".Translate());
            }
            Rect rectDarkEldar = new Rect(rect.x, rect.y + 10, num, 60f);
            if (XBsettings.ShowDarkEldar)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectDarkEldar.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowDarkEldar, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")));
                listing_Standard.EndSection(listing_Standard);
            }

            listing_Standard.CheckboxLabeled("AMXB_ShowEldar".Translate(), ref XBsettings.ShowEldar);
            Rect rectEldar = new Rect(rect.x, rect.y + 10, num, 60f);
            if (XBsettings.ShowEldar)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectEldar.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowEldarCraftworld, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")));
                Widgets.CheckboxLabeled(rectEldar.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarWraithguard".Translate(), ref XBsettings.AllowEldarWraithguard, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")));
                Widgets.CheckboxLabeled(rectEldar.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowEldarExodite, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")));
                Widgets.CheckboxLabeled(rectEldar.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowEldarHarlequinn, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")));
                listing_Standard.EndSection(listing_Standard);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowTau".Translate(), ref XBsettings.ShowTau);
            Rect rectTau = new Rect(rect.x, rect.y + 10, num, 60f);
            if (XBsettings.ShowTau)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectTau.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowTau".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowTau, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")));
                Widgets.CheckboxLabeled(rectTau.BottomHalf().RightHalf().LeftHalf().ContractedBy(4), "AMXB_AllowKroot".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref XBsettings.AllowKrootAuxiliaries, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")));
                Widgets.CheckboxLabeled(rectTau.BottomHalf().RightHalf().RightHalf().ContractedBy(4), "AMXB_AllowKroot".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowKroot, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")));
                Widgets.CheckboxLabeled(rectTau.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowGueVesa".Translate() + (!DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref XBsettings.AllowGueVesaAuxiliaries, !DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")));
                Widgets.CheckboxLabeled(rectTau.TopHalf().RightHalf().LeftHalf().ContractedBy(4), "AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref XBsettings.AllowVespidAuxiliaries, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")));
                Widgets.CheckboxLabeled(rectTau.TopHalf().RightHalf().RightHalf().ContractedBy(4), "AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowVespid, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")));
                listing_Standard.EndSection(listing_Standard);
            }

            listing_Standard.CheckboxLabeled("AMXB_ShowOrk".Translate(), ref XBsettings.ShowOrk);
            Rect rectOrk = new Rect(rect.x, rect.y + 10, num, 60f);
            if (XBsettings.ShowOrk)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectOrk.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowOrkTek, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")));
                Widgets.CheckboxLabeled(rectOrk.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref XBsettings.AllowOrkFeral, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")));
                Widgets.CheckboxLabeled(rectOrk.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowOrkRok".Translate(), ref XBsettings.AllowOrkRok, false);
                listing_Standard.EndSection(listing_Standard);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowNecron".Translate(), ref XBsettings.ShowNecron);
            Rect rectNecron = new Rect(rect.x, rect.y + 10, num, 60f);
            if (XBsettings.ShowNecron)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectNecron.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecron".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowNecron, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")));
                Widgets.CheckboxLabeled(rectNecron.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecronWellBeBack".Translate(), ref XBsettings.AllowNecronWellBeBack, false);
                Widgets.CheckboxLabeled(rectNecron.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowNecronMonolith".Translate(), ref XBsettings.AllowNecronMonolith, false);
                listing_Standard.EndSection(listing_Standard);
            }
            if (DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")))
            {
                listing_Standard.CheckboxLabeled("AMXB_ShowTyranid".Translate(), ref XBsettings.ShowTyranid);
            }
            else
            {
                listing_Standard.Label("AMXB_ShowTyranid".Translate());
            }
            Rect rectTyranid = new Rect(rect.x, rect.y + 10, num, 120f);
            if (XBsettings.ShowTyranid)
            {
                listing_Standard.BeginSection(60f);
                Widgets.CheckboxLabeled(rectNecron.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowTyranid".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref XBsettings.AllowTyranid, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")));
                Widgets.CheckboxLabeled(rectNecron.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowTyranidInfestation".Translate(), ref XBsettings.AllowTyranidInfestation, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid_Infestation")));
                listing_Standard.EndSection(listing_Standard);
            }
            listing_Standard.CheckboxLabeled("AMXB_AllowWarpstorm".Translate(), ref XBsettings.AllowWarpstorm, "AMXB_AllowWarpstormDesc".Translate());
            listing_Standard.EndScrollView(ref inRect);
            
            this.XBsettings.Write();
        }
        private Vector2 pos = new Vector2(0f, -200f);
        private Vector2 pos2 = new Vector2(0f, 0f);
        
        */
        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
    
}