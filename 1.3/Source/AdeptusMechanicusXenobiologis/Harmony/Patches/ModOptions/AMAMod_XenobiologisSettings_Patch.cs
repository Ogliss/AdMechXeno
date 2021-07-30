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
        public static void ModsLoaded(ref string __result)
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
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private static bool ShowImperial => settings.ShowImperium;
        private static bool AllowAstartes => settings.AllowAdeptusAstartes;
        private static bool ShowAstartes => settings.ShowAstartes;
        public static float Listing_BaseLength => mod.Length(ShowXB, 1, lineheight, 0, 0);
        public static float Listing_BaseOptionsLength => mod.Length(ShowXB, 1, lineheight, 0, 0);
        public static float Listing_RacesLength => raceMenu != 0 ? raceMenu : mod.Length(settings.ShowAllowedRaceSettings && ShowXB, 8, lineheight, 8, ShowXB ? 1 : 0, 12);
        private static float Listing_XenobiologisLength => Listing_BaseLength + Listing_BaseOptionsLength + Listing_RacesLength;
        public static float Listing_XenobiologisLengthImperial => mod.Length(ShowXB && ShowRaces && ShowImperial, 1, lineheight, 8, ShowRaces ? 1 : 0) + Listing_XenobiologisLengthImperialOptions + (AdeptusIntergrationUtility.enabled_AdeptusAstartes && ShowXB && ShowRaces && ShowImperial && settings.AllowAdeptusAstartes ? Listing_XenobiologisLengthIAstartes : 0);
        public static float Listing_XenobiologisLengthImperialOptions => mod.Length(ShowXB && ShowRaces && ShowImperial, 2, lineheight, 0, 0);
        public static float Listing_XenobiologisLengthIAstartes => AdeptusIntergrationUtility.enabled_AdeptusAstartes ? mod.Length((ShowXB && ShowRaces && ShowImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis && AllowAstartes && ShowAstartes, 1, lineheight, 8, (ShowXB && ShowRaces && ShowImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis && AllowAstartes ? 1 : 0) + Listing_XenobiologisLengthIAstartesOptions : 0f;
        public static float Listing_XenobiologisLengthIAstartesOptions => AdeptusIntergrationUtility.enabled_AdeptusAstartes ? mod.Length(((ShowXB && ShowRaces && ShowImperial) || !AdeptusIntergrationUtility.enabled_MagosXenobiologis) && AllowAstartes && ShowAstartes, 2, lineheight, 42, 0) : 0;

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
        public static void XenobiologisSettings_Prefix(ref Listing_StandardExpanding listing_Main,ref Rect inRect)
        {
            // Armoury Mod Options
            //    listing_Menu = listing_Main.BeginSection(listing_ArmouryLength, false, 3, 4, 0); 
            if (ShowXB)
            {
                listing_Menu.maxOneColumn = true;
                listing_Menu = listing_Main.BeginSection(Listing_XenobiologisLength + mod.XenobiologisMenuInc + raceMenuInc, false, 0, 4, 0);
                // Xenobiologis Mod Options

                {
                    listing_General = listing_Menu.BeginSection(Listing_BaseOptionsLength, true, 3, 4, 4);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.ForceRelations".Translate(), ref settings.ForceRelations, "AdeptusMechanicus.Xenobiologis.ForceRelationsDesc".Translate());
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AdeptusMechanicus.Xenobiologis.AllowWarpstormDesc".Translate());
                    listing_Menu.EndSection(listing_General);
                }

                listing_Races = listing_Menu.BeginSection(Listing_RacesLength + raceMenuInc, out Rect _frame, out Rect _contents, false, 3, 4, 4);
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