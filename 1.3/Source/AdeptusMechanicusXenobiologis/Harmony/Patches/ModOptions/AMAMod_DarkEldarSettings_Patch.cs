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
        private static AMSettings Settings => AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;
        private static bool factionExists = DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_DarkEldar"));
        private static bool hidden = DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_DarkEldar") && x.hidden);

        private static bool Dev => AMAMod.Dev;
        private static bool ShowXB => Settings.ShowXenobiologisSettings;
        private static bool ShowRaces => Settings.ShowAllowedRaceSettings && ShowXB;
        private static bool Setting => ShowRaces && Settings.ShowDarkEldar;

        private static int Options = 2;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void DarkEldarSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            if (!AdeptusIntergrationUtility.enabled_XenobiologisEldar)
            {
                if (ShowRaces)
                {
                    string label = "AdeptusMechanicus.Xenobiologis.ShowDarkEldar".Translate() + " Settings";
                    string tooltip = string.Empty;
                    if (Dev)
                    {
                        label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                    }

                    Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                    if (listing_Race.CheckboxLabeled(label, ref Settings.ShowDarkEldar, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                    {
                        Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                        listing_General.ColumnWidth *= 0.488f;
                        listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowDarkEldar".Translate() + (!factionExists ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : !hidden ? "AdeptusMechanicus.Xenobiologis.Faction".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                            ref Settings.AllowDarkEldar,
                            null,
                            !factionExists || !Settings.AllowDarkEldarWeapons,
                            factionExists && Settings.AllowDarkEldarWeapons);
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


}