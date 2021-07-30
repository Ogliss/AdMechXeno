using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "OrkSettings")]
    public static class AMAMod_OrkSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private static bool Setting => ShowRaces && settings.ShowOrk;

        private static int Options = 2;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void OrkSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            if (AdeptusIntergrationUtility.enabled_XenobiologisOrk)
            {
                return;
            }
            if (ShowRaces)
            {
                string label = "AdeptusMechanicus.Xenobiologis.ShowOrk".Translate() + " Settings";
                string tooltip = "AdeptusMechanicus.ShowSpecialRulesDesc".Translate();
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowOrk, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.32f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowOrkTek,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowOrkFeral,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) || !settings.AllowOrkWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) && settings.AllowOrkWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkRok".Translate(),
                        ref settings.AllowOrkRok,
                        null,
                        !settings.AllowOrkTek || !settings.AllowOrkWeapons,
                        settings.AllowOrkTek && settings.AllowOrkWeapons);
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