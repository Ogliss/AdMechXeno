using RimWorld;
using Verse;
using HarmonyLib;
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
	/*
    [HarmonyPatch(typeof(AMAMod), "get_MenuLength")]
    public static class AMXB_AMAMod_MenuLength_Patch
    {
        [HarmonyPostfix]
        public static void MenuLength_Postfix(ref float __result)
        {
            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}",  num2));
            __result += SettingsHelper.latest.ShowImperium ? 60f : 0f;
            __result += SettingsHelper.latest.ShowChaos ? 120f : 0f;
            __result += SettingsHelper.latest.ShowEldar ? 60f : 0f;
            __result += SettingsHelper.latest.ShowTau ? 60f : 0f;
            __result += SettingsHelper.latest.ShowOrk ? 60f : 0;
            __result += SettingsHelper.latest.ShowNecron ? 60f : 0;
            __result += SettingsHelper.latest.ShowTyranid ? 60f : 0;

            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}", num2));
        }
        
    }
	*/
    [HarmonyPatch(typeof(AMMod), "XenobiologisSettings")]
    public static class AMXB_AMMod_XenobiologisSettings_Patch
    {
        [HarmonyPostfix]
        public static void XenobiologisSettings_Postfix(ref Listing_Standard listing_Standard, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMSettings.Instance;
            Rect rect = new Rect(inRect.x, inRect.y - 50, num, num2);
            listing_Standard.Label("AMXB_ModName".Translate() + " Settings");
            listing_Standard.CheckboxLabeled("AMXB_ForceRelations".Translate(), ref settings.ForceRelations, "AMXB_ForceRelationsDesc".Translate());
            listing_Standard.CheckboxLabeled("AMXB_AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AMXB_AllowWarpstormDesc".Translate());
            listing_Standard.Label("AMXB_AllowedRaces".Translate());
        }
		
    }

    [HarmonyPatch(typeof(AMMod), "TyranidSettings")]
    public static class AMXB_AMMod_TyranidSettings_Patch
    {
        [HarmonyPrefix]
        public static void TyranidSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowTyranid".Translate(), ref AMAsettings.ShowTyranid);
            if (AMAsettings.ShowTyranid)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowTyranid".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowTyranid, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowTyranidInfestation".Translate(), ref settings.AllowTyranidInfestation, !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid_Infestation")));
            listing_Standard.EndSection(listing_Standard);
        }
    }

    [HarmonyPatch(typeof(AMMod), "NecronSettings")]
    public static class AMXB_AMMod_NecronSettings_Patch
    {
        [HarmonyPrefix]
        public static void NecronSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowNecron".Translate(), ref AMAsettings.ShowNecron);
            if (AMAsettings.ShowNecron)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecron".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowNecron, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowNecronWellBeBack".Translate(), ref settings.AllowNecronWellBeBack, false);
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowNecronMonolith".Translate(), ref settings.AllowNecronMonolith, false);
            listing_Standard.EndSection(listing_Standard);
        }
    }

    [HarmonyPatch(typeof(AMMod), "DarkEldarSettings")]
    public static class AMXB_AMMod_DarkEldarSettings_Patch
    {
        [HarmonyPrefix]
        public static void DarkEldar_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            if (DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")))
            {
                listing_Standard.CheckboxLabeled("AMXB_ShowDarkEldar".Translate(), ref AMAsettings.ShowDarkEldar);
            }
            else
            {
                listing_Standard.Label("AMXB_ShowDarkEldar".Translate());
            }
            if (AMAsettings.ShowDarkEldar)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowDarkEldar, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")));
            listing_Standard.EndSection(listing_Standard);
        }
    }

    [HarmonyPatch(typeof(AMMod), "ChaosSettings")]
    public static class AMXB_AMMod_ChaosSettings_Patch
    {
        [HarmonyPrefix]
        public static void ChaosSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowChaos".Translate(), ref AMAsettings.ShowChaos);
            if (AMAsettings.ShowChaos)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
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
    }

    [HarmonyPatch(typeof(AMMod), "ImperialSettings")]
    public static class AMXB_AMMod_ImperialSettings_Patch
    {
        [HarmonyPrefix]
        public static void ImperialSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowImperium".Translate(), ref AMAsettings.ShowImperium);
            if (AMAsettings.ShowImperium)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusAstartes".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowAdeptusAstartes, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) || !AMSettings.Instance.AllowImperialWeapons);
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowAdeptusMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowAdeptusMechanicus, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) || !AMSettings.Instance.AllowMechanicusWeapons);
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusMilitarum".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowAdeptusMilitarum, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) || !AMSettings.Instance.AllowImperialWeapons);
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowAdeptusSororitas".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowAdeptusSororitas, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) || !AMSettings.Instance.AllowImperialWeapons);
            listing_Standard.EndSection(listing_Standard);

        }
    }

    [HarmonyPatch(typeof(AMMod), "EldarSettings")]
    public static class AMXB_AMMod_EldarSettings_Patch
    {
        [HarmonyPrefix]
        public static void EldarSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowEldar".Translate(), ref AMAsettings.ShowEldar);
            if (AMAsettings.ShowEldar)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarCraftworld, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarWraithguard".Translate(), ref settings.AllowEldarWraithguard, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowEldarExodite, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarHarlequinn, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")));
            listing_Standard.EndSection(listing_Standard);

        }
    }

    [HarmonyPatch(typeof(AMMod), "OrkSettings")]
    public static class AMXB_AMMod_OrkSettings_Patch
    {
        [HarmonyPrefix]
        public static void OrkSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowOrk".Translate(), ref AMAsettings.ShowOrk);
            if (AMAsettings.ShowOrk)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkTek, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkFeral, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowOrkRok".Translate(), ref settings.AllowOrkRok, false);
            listing_Standard.EndSection(listing_Standard);
        }
    }

    [HarmonyPatch(typeof(AMMod), "TauSettings")]
    public static class AMXB_AMMod_TauSettings_Patch
    {
        [HarmonyPrefix]
        public static void TauSettings_Postfix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMXB_ShowTau".Translate(), ref AMAsettings.ShowTau);
            if (AMAsettings.ShowTau)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 60f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
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
    }

	/*
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMXB_AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix]
        public static void PostModOptions_Prefix(ref Listing_Standard listing_Standard, Rect inRect, ref float num, ref float num2)
        {
            AMSettings settings = AMSettings.Instance;
            settings.Write();
        }

    }
	*/
}