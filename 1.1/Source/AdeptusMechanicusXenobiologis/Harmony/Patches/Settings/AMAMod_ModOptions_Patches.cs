using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ModLoaded")]
    public static class AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void ModsLoaded(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AMXB_ModName".Translate() ;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "XenobiologisSettings")]
    public static class AMMod_XenobiologisSettings_Patch
    {
        [HarmonyPrefix]
        public static bool XenobiologisSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, ref float xenobiologisMenuLenght)
        {
            Listing_Standard listing_XenobiologisSettings = new Listing_Standard();
            Listing_Standard listing_Races = new Listing_Standard();
            Listing_Standard listing_General = new Listing_Standard();
            AMSettings settings = AMAMod.settings;
            bool XBOptions = settings.ShowXenobiologisSettings;
            bool XBRaceOptions = settings.ShowAllowedRaceSettings && XBOptions;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float menuLengthXBRace = __instance.Length(XBRaceOptions, 1, lineheight, 64, XBOptions ? 1:0);
            float menuLengthXBMain = __instance.Length(XBOptions, 3, lineheight,0);// + menuLengthXBMainOptions + menuLengthXBRace;
            //    menuLengthXBMain += menuLengthXBRace;

            //    __instance.XenobiologisMenuLength = __instance.Length(XBRaceOptions, 1, lineheight, 4) + (XBOptions ? __instance.XenobiologisRules + __instance.AllowedRaceSettings : 0);
            float w = rect.width * 0.480f;
            listing_XenobiologisSettings = listing_Main.BeginSection(__instance.MenuLengthXenobiologis, false, 3);
            listing_XenobiologisSettings.CheckboxLabeled("AMXB_ModName".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + (__instance.MenuLengthXenobiologis) : "") + " Options" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + __instance.MenuLengthXenobiologisRaces : ""), ref settings.ShowXenobiologisSettings, null, false, true);
            
            if (XBOptions)
            {
                {
                    listing_General = listing_XenobiologisSettings.BeginSection(__instance.MenuLengthXenobiologisBaseOptions, true);
                    listing_General.ColumnWidth = w;
                    listing_General.CheckboxLabeled("AMXB_ForceRelations".Translate(), ref settings.ForceRelations, "AMXB_ForceRelationsDesc".Translate());
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AMXB_AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AMXB_AllowWarpstormDesc".Translate());
                    listing_XenobiologisSettings.EndSection(listing_General);
                }

                listing_Races = listing_XenobiologisSettings.BeginSection(__instance.MenuLengthXenobiologisRaces, false, 3);
                listing_Races.CheckboxLabeled("AMXB_AllowedRaces".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + __instance.MenuLengthXenobiologisRaces : ""), ref settings.ShowAllowedRaceSettings, null, false, true);
                if (XBRaceOptions)
                {
                    __instance.ImperialSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisImperialMenuLength);
                    __instance.ChaosSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisChaosMenuLength);
                    __instance.EldarSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisEldarMenuLength);
                    __instance.DarkEldarSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisDarkEldarMenuLength);
                    __instance.OrkSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisOrkMenuLength);
                    __instance.TauSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisTauMenuLength);
                    __instance.NecronSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisNecronMenuLength);
                    __instance.TyranidSettings(ref listing_Races, rect, inRect, w, __instance.XenobiologisTyranidMenuLength);
                    
                }
                listing_XenobiologisSettings.EndSection(listing_Races);
                __instance.XenobiologisRaceMenuLength = menuLengthXBRace;
            }
            listing_Main.EndSection(listing_XenobiologisSettings);
            __instance.XenobiologisMenuLength = menuLengthXBMain;
            return false;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "ImperialSettings")]
    public static class AMMod_ImperialSettings_Patch
    {
        [HarmonyPrefix]
        public static bool ImperialSettings_Prefix(ref AMAMod __instance ,ref Listing_Standard listing_Main, Rect rect, ref Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            string ImperialLabel = "AMXB_ShowImperium".Translate() + " Settings";
            string ImperialToolTip = " Main: " + __instance.MenuLengthXenobiologisRacesImperial;
            ImperialToolTip += " Options: " + __instance.MenuLengthXenobiologisRacesImperialOptions;
            ImperialToolTip += " Astartes: " + __instance.MenuLengthXenobiologisRacesAstartes;
            bool showRaces = settings.ShowXenobiologisSettings && settings.ShowAllowedRaceSettings;
            bool setting = showRaces && settings.ShowImperium;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            float options = __instance.Length(setting, 2, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, __instance.MenuLengthXenobiologisRacesImperial);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, __instance.MenuLengthXenobiologisRacesImperial);
            }

            listing_Race.CheckboxLabeled(ImperialLabel, ref settings.ShowImperium, ImperialToolTip, false, true);
            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(__instance.MenuLengthXenobiologisRacesImperialOptions, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AMXB_AllowAdeptusAstartes".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowAdeptusAstartes, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) || !AMSettings.Instance.AllowImperialWeapons,
                    AMSettings.Instance.AllowImperialWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowAdeptusMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowAdeptusMechanicus, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) || !AMSettings.Instance.AllowMechanicusWeapons);

                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowAdeptusMilitarum".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowAdeptusMilitarum, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) || !AMSettings.Instance.AllowImperialWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowAdeptusSororitas".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowAdeptusSororitas, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) || !AMSettings.Instance.AllowImperialWeapons);
                listing_Race.EndSection(listing_General);
                if (AdeptusIntergrationUtility.enabled_AdeptusAstartes && settings.AllowAdeptusAstartes)
                {
                    __instance.AstartesSettings(ref listing_Race, rect, inRect, w, __instance.MenuLengthXenobiologisRacesAstartes);
                }
                /*
                if (settings.AllowAdeptusMechanicus && AdeptusIntergrationUtil.enabled_AdeptusMechanicus)
                {
                    __instance.MechanicusSettings(ref listing_Main, rect, inRect, w, __instance.MenuLengthXenobiologisMechanicus);
                }
                if (settings.AllowAdeptusMilitarum && AdeptusIntergrationUtil.enabled_AdeptusMilitarum)
                {
                    __instance.MilitarumSettings(ref listing_Main, rect, inRect, w, __instance.MenuLengthXenobiologisMilitarum);
                }
                if (settings.AllowAdeptusSororitas && AdeptusIntergrationUtil.enabled_AdeptusSororitas)
                {
                    __instance.SororitasSettings(ref listing_Main, rect, inRect, w, __instance.MenuLengthXenobiologisSororitas);
                }
                */
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisImperialMenuLength = listing_Race.CurHeight;
            //    __instance.XenobiologisImperialMenuLength = RaceSettings;
            return false;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "ChaosSettings")]
    public static class AMMod_ChaosSettings_Patch
    {
        [HarmonyPrefix]
        public static void ChaosSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowChaos;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 4;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowChaos".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowChaos, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= AdeptusIntergrationUtility.enabled_EndTimesWithGuns ? 0.32f : 0.488f;

                listing_General.CheckboxLabeled("AMXB_AllowChaosMarine".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowChaosMarine, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) || !settings.AllowChaosWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) && settings.AllowChaosWeapons);

                listing_General.CheckboxLabeled("AMXB_AllowChaosGuard".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowChaosGuard, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) || !settings.AllowChaosWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) && settings.AllowChaosWeapons);

                listing_General.CheckboxLabeled("AMXB_AllowChaosMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowChaosMechanicus, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) && settings.AllowChaosWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) && settings.AllowChaosWeapons);

                listing_General.NewColumn();

                listing_General.CheckboxLabeled("AMXB_AllowChaosDeamons".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowChaosDeamons, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")), 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));

                listing_General.CheckboxLabeled("AMXB_AllowChaosDeamonicIncursion".Translate(), 
                    ref settings.AllowChaosDeamonicIncursion, 
                    null, 
                    !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")) || !settings.AllowChaosDeamons,
                    DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")) && settings.AllowChaosDeamons);

                listing_General.CheckboxLabeled("AMXB_AllowChaosDeamonicInfestation".Translate(), 
                    ref settings.AllowChaosDeamonicInfestation, 
                    null, 
                    !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")) || !settings.AllowChaosDeamons,
                    DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")) && settings.AllowChaosDeamons);
                if (AdeptusIntergrationUtility.enabled_EndTimesWithGuns)
                {
                    listing_General.NewColumn();

                    listing_General.CheckboxLabeled("AMXB_EndTimesChaosDeamonIntergration".Translate(),
                        ref settings.EndTimesIntergrateDeamons,
                        "AMXB_EndTimesChaosDeamonIntergrationDesc".Translate(),
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")),
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));
                    listing_General.CheckboxLabeled("AMXB_EndTimesChaosDeamonIntergration_GreatPortal".Translate(),
                        ref settings.EndTimesIntergrateDeamonsGreat,
                        "AMXB_EndTimesChaosDeamonIntergration_GreatPortalDesc".Translate(),
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) || !settings.EndTimesIntergrateDeamons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) && settings.EndTimesIntergrateDeamons);
                    listing_General.CheckboxLabeled("AMXB_EndTimesChaosDeamonIntergration_SmallPortal".Translate(),
                        ref settings.EndTimesIntergrateDeamonsSmall,
                        "AMXB_EndTimesChaosDeamonIntergration_SmallPortalDesc".Translate(),
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) || !settings.EndTimesIntergrateDeamons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) && settings.EndTimesIntergrateDeamons);

                }
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisChaosMenuLength = listing_Race.CurHeight;
        //    __instance.XenobiologisChaosMenuLength = RaceSettings;

        }
    }

    [HarmonyPatch(typeof(AMAMod), "EldarSettings")]
    public static class AMMod_EldarSettings_Patch
    {
        [HarmonyPrefix]
        public static bool EldarSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowEldar;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 3;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowEldar".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + __instance.XenobiologisEldarMenuLength : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowEldar, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowEldarCraftworld, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowEldarWraithguard".Translate(), 
                    ref settings.AllowEldarWraithguard, 
                    null, 
                    !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) || !settings.AllowEldarWeapons, 
                    DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) && settings.AllowEldarWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowEldarExodite, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) || !settings.AllowEldarWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowEldarHarlequinn, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) && settings.AllowEldarWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisEldarMenuLength = listing_Race.CurHeight;

            return false;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "DarkEldarSettings")]
    public static class AMMod_DarkEldarSettings_Patch
    {
        [HarmonyPrefix]
        public static void DarkEldar_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowDarkEldar;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 2;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowDarkEldar".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowDarkEldar, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AMXB_AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowDarkEldar, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) || !settings.AllowDarkEldarWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) && settings.AllowDarkEldarWeapons);
                listing_General.NewColumn();
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisDarkEldarMenuLength = listing_Race.CurHeight;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "OrkSettings")]
    public static class AMMod_OrkSettings_Patch
    {
        [HarmonyPrefix]
        public static void OrkSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowOrk;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 2;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowOrk".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowOrk, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.32f;
                listing_General.CheckboxLabeled("AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowOrkTek, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowOrkFeral, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) || !settings.AllowOrkWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowOrkRok".Translate(), 
                    ref settings.AllowOrkRok,
                    null, 
                    !settings.AllowOrkTek || !settings.AllowOrkWeapons,
                    settings.AllowOrkTek && settings.AllowOrkWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisOrkMenuLength = RaceSettings;

        }
    }

    [HarmonyPatch(typeof(AMAMod), "TauSettings")]
    public static class AMMod_TauSettings_Patch
    {
        [HarmonyPrefix]
        public static void TauSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {

            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowTau;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 3;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowTau".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowTau, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.32f;
                listing_General.CheckboxLabeled("AMXB_AllowTau".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowTau, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) || !settings.AllowTauWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) && settings.AllowTauWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowGueVesa".Translate() + (!DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), 
                    ref settings.AllowGueVesaAuxiliaries, 
                    null, 
                    !DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) || !settings.AllowTauWeapons, 
                    DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) && settings.AllowTauWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowKroot".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowKroot, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) || !settings.AllowTauWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) && settings.AllowTauWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowKroot".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), 
                    ref settings.AllowKrootAuxiliaries, 
                    null, 
                    !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) || !settings.AllowTauWeapons, 
                    DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) && settings.AllowTauWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), 
                    ref settings.AllowVespid, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")) || !settings.AllowTauWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")) && settings.AllowTauWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Auxiliaries".Translate()), 
                    ref settings.AllowVespidAuxiliaries, 
                    null, 
                    !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) || !settings.AllowTauWeapons, 
                    DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) && settings.AllowTauWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisTauMenuLength = RaceSettings;

        }
    }

    [HarmonyPatch(typeof(AMAMod), "NecronSettings")]
    public static class AMMod_NecronSettings_Patch
    {
        [HarmonyPrefix]
        public static void NecronSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowNecron;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 2;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowNecron".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowNecron, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.32f;

                listing_General.CheckboxLabeled("AMXB_AllowNecron".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowNecron, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) || !settings.AllowNecronWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Necron")) && settings.AllowNecronWeapons);

                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowNecronWellBeBack".Translate(),
                    ref settings.AllowNecronWellBeBack, 
                    null, 
                    !settings.AllowNecron || !settings.AllowNecronWeapons, 
                    settings.AllowNecron && settings.AllowNecronWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowNecronMonolith".Translate(), 
                    ref settings.AllowNecronMonolith, 
                    null, 
                    !settings.AllowNecron || !settings.AllowNecronWeapons, 
                    settings.AllowNecron && settings.AllowNecronWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisNecronMenuLength = RaceSettings;

        }
    }

    [HarmonyPatch(typeof(AMAMod), "TyranidSettings")]
    public static class AMMod_TyranidSettings_Patch
    {
        [HarmonyPrefix]
        public static void TyranidSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {

            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowTyranid;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 2;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowTyranid".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowTyranid, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AMXB_AllowTyranid".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), 
                    ref settings.AllowTyranid, 
                    null, 
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) || !settings.AllowTyranidWeapons, 
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid")) && settings.AllowTyranidWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowTyranidInfestation".Translate(), 
                    ref settings.AllowTyranidInfestation, 
                    null, 
                    !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid_Infestation")) || !settings.AllowTyranid || !settings.AllowTyranidWeapons, 
                    DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Tyranid_Infestation")) && settings.AllowTyranidWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisTyranidMenuLength = RaceSettings;

        }
    }

}