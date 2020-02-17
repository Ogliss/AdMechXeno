using RimWorld;
using Verse;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus
{
    [HarmonyPatch(typeof(AMAMod), "SettingsCategory")]
    public static class AMXB_AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void SettingsCategory_Postfix(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AMXB_ModName".Translate();
        }
    }

    [HarmonyPatch(typeof(AMAMod), "PreModOptions")]
    public static class AMXB_AMAMod_PreModOptions_Patch
    {
        [HarmonyPrefix]
        public static void PreModOptions_Prefix(ref Listing_Standard listing_Standard, Rect inRect, float num, ref float num2)
        {
        //    Log.Message("PreModOptions_Prefix");
            AMXBSettings XBsettings = AMXBSettings.Instance;

        //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}",  num2));
            num2 = num2 + (XBsettings.ShowImperium ? 60f : 0f) + (XBsettings.ShowChaos ? 120f : 0f) + (XBsettings.ShowEldar ? 60f : 0f) + (XBsettings.ShowTau ? 60f : 0f) + (XBsettings.ShowOrk ? 60f : 0) + (XBsettings.ShowNecron ? 60f : 0) + (XBsettings.ShowTyranid ? 60f : 0);
        //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}", num2));
        }
        
    }

    [HarmonyPatch(typeof(AMAMod), "ModOptions")]
    public static class AMXB_AMAMod_ModOptions_Patch
    {
        [HarmonyPostfix]
        public static void ModOptions_Postfix(ref Listing_Standard listing_Standard, Rect inRect, float num, float num2)
        {
            AMXBSettings settings = AMXBSettings.Instance;
            Rect rect = new Rect(inRect.x, inRect.y - 50, num, num2);
            listing_Standard.Label("AMXB_ModName".Translate() + " Settings");
            listing_Standard.Label("AMXB_AllowedRaces".Translate());
            listing_Standard.CheckboxLabeled("AMXB_ForceRelations".Translate(), ref settings.ForceRelations, "AMXB_ForceRelationsDesc".Translate());
            listing_Standard.CheckboxLabeled("AMXB_ShowImperium".Translate(), ref settings.ShowImperium, "AMXB_ShowImperiumDesc".Translate());
            if (settings.ShowImperium)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowImperium(ref listing_Standard, rect2, settings);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowChaos".Translate(), ref settings.ShowChaos);
            if (settings.ShowChaos)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                ShowChaos(ref listing_Standard, rect2, settings);
            }
            if (DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")))
            {
                listing_Standard.CheckboxLabeled("AMXB_ShowDarkEldar".Translate(), ref settings.ShowDarkEldar);
            }
            else
            {
                listing_Standard.Label("AMXB_ShowDarkEldar".Translate());
            }
            if (settings.ShowDarkEldar)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowDarkEldar(ref listing_Standard, rect2, settings);
            }

            listing_Standard.CheckboxLabeled("AMXB_ShowEldar".Translate(), ref settings.ShowEldar);
            if (settings.ShowEldar)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowEldar(ref listing_Standard, rect2, settings);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowTau".Translate(), ref settings.ShowTau);
            if (settings.ShowTau)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowTau(ref listing_Standard, rect2, settings);
            }

            listing_Standard.CheckboxLabeled("AMXB_ShowOrk".Translate(), ref settings.ShowOrk);
            if (settings.ShowOrk)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowOrk(ref listing_Standard, rect2, settings);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowNecron".Translate(), ref settings.ShowNecron);
            if (settings.ShowNecron)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                ShowNecron(ref listing_Standard, rect2, settings);
            }
            listing_Standard.CheckboxLabeled("AMXB_ShowTyranid".Translate(), ref settings.ShowTyranid);
            if (settings.ShowTyranid)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                ShowTyranid(ref listing_Standard, rect2, settings);
            }
            listing_Standard.CheckboxLabeled("AMXB_AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AMXB_AllowWarpstormDesc".Translate());

        }

        static void ShowImperium(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusAstartes".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowAdeptusAstartes, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) || !AMASettings.Instance.AllowImperialWeapons);
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowAdeptusMechanicus, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) || !AMASettings.Instance.AllowMechanicusWeapons);
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusMilitarum".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowAdeptusMilitarum, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) || !AMASettings.Instance.AllowImperialWeapons);
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusSororitas".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowAdeptusSororitas, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) || !AMASettings.Instance.AllowImperialWeapons);
            listing_Standard.EndSection(listing_Standard);

        }
        static void ShowChaos(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(120f);
            Widgets.CheckboxLabeled(rect2.TopHalf().TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosMarine".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowChaosMarine, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")));
            Widgets.CheckboxLabeled(rect2.TopHalf().BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosGuard".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowChaosGuard, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")));
            Widgets.CheckboxLabeled(rect2.TopHalf().TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowChaosMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowChaosMechanicus, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")));
            Widgets.CheckboxLabeled(rect2.TopHalf().BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowChaosDeamons".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowChaosDeamons, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosDeamonicIncursion".Translate(), ref settings.AllowChaosDeamonicIncursion, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowChaosDeamonicInfestation".Translate(), ref settings.AllowChaosDeamonicInfestation, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")));
            listing_Standard.EndSection(listing_Standard);
        }
        static void ShowDarkEldar(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowDarkEldar, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")));
            listing_Standard.EndSection(listing_Standard);
        }
        static void ShowEldar(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarCraftworld, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarWraithguard".Translate(), ref settings.AllowEldarWraithguard, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowEldarExodite, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarHarlequinn, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")));
            listing_Standard.EndSection(listing_Standard);

        }
        static void ShowTau(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowTau".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowTau, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().LeftHalf().ContractedBy(4), "AMXB_AllowKroot".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref settings.AllowKrootAuxiliaries, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().RightHalf().ContractedBy(4), "AMXB_AllowKroot".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowKroot, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowGueVesa".Translate() + (!DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref settings.AllowGueVesaAuxiliaries, !DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().LeftHalf().ContractedBy(4), "AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), ref settings.AllowVespidAuxiliaries, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().RightHalf().ContractedBy(4), "AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowVespid, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")));
            listing_Standard.EndSection(listing_Standard);
        }
        static void ShowOrk(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkTek, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkFeral, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowOrkRok".Translate(), ref settings.AllowOrkRok, false);
            listing_Standard.EndSection(listing_Standard);
        }
        static void ShowNecron(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecron".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowNecron, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecronWellBeBack".Translate(), ref settings.AllowNecronWellBeBack, false);
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowNecronMonolith".Translate(), ref settings.AllowNecronMonolith, false);
            listing_Standard.EndSection(listing_Standard);
        }
        static void ShowTyranid(ref Listing_Standard listing_Standard, Rect rect2, AMXBSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowTyranid".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowTyranid, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowTyranidInfestation".Translate(), ref settings.AllowTyranidInfestation, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid_Infestation")));
            listing_Standard.EndSection(listing_Standard);
        }
    }
    
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMXB_AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix]
        public static void PostModOptions_Prefix(ref Listing_Standard listing_Standard, Rect inRect, ref float num, ref float num2)
        {
        //    Log.Message("PostModOptions_Postfix");
            AMXBSettings XBsettings = AMXBSettings.Instance;
            XBsettings.Write();
        }

    }
}