using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "EldarSettings")]
    public static class AMAMod_EldarSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private static bool Setting => ShowRaces && settings.ShowEldar;

        private static int Options = 3;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void EldarSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            if (AdeptusIntergrationUtility.enabled_XenobiologisEldar)
            {
                return;
            }
            if (ShowRaces)
            {
                string label = "AdeptusMechanicus.Xenobiologis.ShowEldar".Translate() + " Settings";
                string tooltip = string.Empty;
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }

                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowEldar, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowEldarCraftworld,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) && settings.AllowEldarWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarWraithguard".Translate(),
                        ref settings.AllowEldarWraithguard,
                        null,
                        !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) || !settings.AllowEldarWeapons,
                        DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) && settings.AllowEldarWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowEldarExodite,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) && settings.AllowEldarWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowEldarHarlequinn,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) && settings.AllowEldarWeapons);
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