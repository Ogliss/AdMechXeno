using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "DarkEldarSettings")]
    public static class AMAMod_DarkEldarSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool showXB => settings.ShowXenobiologisSettings;
        private static bool showRaces => settings.ShowAllowedRaceSettings && showXB;
        private static bool setting => showRaces && settings.ShowDarkEldar;

        private static int Options = 2;
        private static float RaceSettings => mod.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void EldarSettings_Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, Rect rect, Rect inRect, float num, ref float num2)
        {
            if (AdeptusIntergrationUtility.enabled_XenobiologisDarkEldar)
            {
                return;
            }
            if (showRaces)
            {
                string label = "AMXB_ShowDarkEldar".Translate() + " Settings";
                string tooltip = string.Empty;
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }

                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowDarkEldar, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AMXB_AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()),
                        ref settings.AllowDarkEldar,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) || !settings.AllowDarkEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Dark_Eldar")) && settings.AllowDarkEldarWeapons);
                    listing_General.NewColumn();
                    listing_Race.EndSection(listing_General);
                    MenuLength = listing_General.CurHeight != 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength - inc;
            }
        }
    }


}