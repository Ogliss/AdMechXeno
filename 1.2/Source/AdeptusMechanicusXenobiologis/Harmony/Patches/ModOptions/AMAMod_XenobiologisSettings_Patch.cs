using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ModLoaded")]
    public static class AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void ModsLoaded(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AdeptusMechanicus.Xenobiologis.ModName".Translate() ;
        }
    }

    [HarmonyPatch(typeof(AMAMod), "XenobiologisSettings")]
    public static class AMAMod_XenobiologisSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static Listing_StandardExpanding listing_Menu = new Listing_StandardExpanding();
        private static Listing_StandardExpanding listing_Races = new Listing_StandardExpanding();
        private static Listing_StandardExpanding listing_General = new Listing_StandardExpanding();

        private static bool Dev => AMAMod.Dev;
        private static bool showXB => settings.ShowXenobiologisSettings;
        private static bool showRaces => settings.ShowAllowedRaceSettings && showXB;
        private static bool showImperial => settings.ShowImperium;
        private static bool allowAstartes => settings.AllowAdeptusAstartes;
        private static bool showAstartes => settings.ShowAstartes;
        public static float listing_BaseLength => mod.Length(showXB, 1, lineheight, 0, 0);
        public static float listing_BaseOptionsLength => mod.Length(showXB, 1, lineheight, 0, 0);
        public static float listing_RacesLength => raceMenu != 0 ? raceMenu : mod.Length(settings.ShowAllowedRaceSettings && showXB, 8, lineheight, 8, showXB ? 1 : 0, 12);
        private static float listing_XenobiologisLength => listing_BaseLength + listing_BaseOptionsLength + listing_RacesLength;
        public static float listing_XenobiologisLengthImperial => mod.Length(showXB && showRaces && showImperial, 1, lineheight, 8, showRaces ? 1 : 0) + listing_XenobiologisLengthImperialOptions + (AdeptusIntergrationUtility.enabled_AdeptusAstartes && showXB && showRaces && showImperial && settings.AllowAdeptusAstartes ? listing_XenobiologisLengthIAstartes : 0);
        public static float listing_XenobiologisLengthImperialOptions => mod.Length(showXB && showRaces && showImperial, 2, lineheight, 0, 0);
        public static float listing_XenobiologisLengthIAstartes => AdeptusIntergrationUtility.enabled_AdeptusAstartes ? mod.Length((showXB && showRaces && showImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis && allowAstartes && showAstartes, 1, lineheight, 8, (showXB && showRaces && showImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis && allowAstartes ? 1 : 0) + listing_XenobiologisLengthIAstartesOptions : 0f;
        public static float listing_XenobiologisLengthIAstartesOptions => AdeptusIntergrationUtility.enabled_AdeptusAstartes ? mod.Length(((showXB && showRaces && showImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis) && allowAstartes && showAstartes, 2, lineheight, 42, 0) : 0;

        private static float raceMenuInc = 0;
        private static float raceMenu = 0;
        private static float raceMenuImperial = 0;
        private static float raceMenuChaos = 0;
        private static float raceMenuEldar = 0;
        private static float raceMenuDarkEldar = 0;
        private static float raceMenuOrk = 0;
        private static float raceMenuTau = 0;
        private static float raceMenuNecron = 0;
        private static float raceMenuTyranid = 0;
        [HarmonyPrefix]
        public static void XenobiologisSettings_Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, Rect rect, ref Rect inRect, float num, float xenobiologisMenuLenght)
        {
            // Armoury Mod Options
            //    listing_Menu = listing_Main.BeginSection(listing_ArmouryLength, false, 3, 4, 0); 
            if (showXB)
            {
                listing_Menu.maxOneColumn = true;
                listing_Menu = listing_Main.BeginSection(listing_XenobiologisLength + mod.XenobiologisMenuInc + raceMenuInc, false, 0, 4, 0);
                // Xenobiologis Mod Options

                {
                    listing_General = listing_Menu.BeginSection(listing_BaseOptionsLength, true, 3, 4, 4);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.ForceRelations".Translate(), ref settings.ForceRelations, "AdeptusMechanicus.Xenobiologis.ForceRelationsDesc".Translate());
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AdeptusMechanicus.Xenobiologis.AllowWarpstormDesc".Translate());
                    listing_Menu.EndSection(listing_General);
                }

                listing_Races = listing_Menu.BeginSection(listing_RacesLength + raceMenuInc, out Rect frane, out Rect contents, false, 3, 4, 4);
                //   Log.Message(listing_Menu.listingRect.height + " " + listing_Menu.CurHeight + " " + listing_Menu.MaxColumnHeightSeen);
                string labelR = "AdeptusMechanicus.Xenobiologis.AllowedRaces".Translate() + " Settings";
                string tooltipR = "AdeptusMechanicus.ShowSpecialRulesDesc".Translate();
                if (Dev)
                {
                    labelR +=" Length: "+ raceMenu;
                }
                if (listing_Races.CheckboxLabeled(labelR, ref settings.ShowAllowedRaceSettings, Dev, ref raceMenuInc, tooltipR, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {

                    mod.ImperialSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuImperial);
                    mod.ChaosSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuChaos);
                    mod.EldarSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuEldar);
                    mod.DarkEldarSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuDarkEldar);
                    mod.OrkSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuOrk);
                    mod.TauSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuTau);
                    mod.NecronSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuNecron);
                    mod.TyranidSettings(ref listing_Races, listing_Races.listingRect, inRect, listing_Races.listingRect.width, ref raceMenuTyranid);

                }
                listing_Menu.EndSection(listing_Races);
                raceMenu = listing_Races.CurHeight;
                listing_Main.EndSection(listing_Menu);
            }
            //    XenobiologisMenus.XenobiologisModOptionsMenu(ref __instance, ref listing_Main, rect, ref inRect, num, xenobiologisMenuLenght);
        }
    }

}