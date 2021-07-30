using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "TauSettings")]
    public static class AMAMod_TauSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private static bool Setting => ShowRaces && settings.ShowTau;

        private static int Options = 3;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void TauSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {

            if (AdeptusIntergrationUtility.enabled_XenobiologisTau)
            {
                return;
            }
            if (ShowRaces)
            {
                string label = "AdeptusMechanicus.Xenobiologis.ShowTau".Translate() + " Settings";
                string tooltip = string.Empty;
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowTau, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.32f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowTau".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowTau,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) || !settings.AllowTauWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Tau")) && settings.AllowTauWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowGueVesa".Translate() + (!DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Auxiliaries".Translate()),
                        ref settings.AllowGueVesaAuxiliaries,
                        null,
                        !DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) || !settings.AllowTauWeapons,
                        DefDatabase<PawnKindDef>.AllDefs.Any(x => x.defName.Contains("OG_Guevesa")) && settings.AllowTauWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowKroot".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowKroot,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) || !settings.AllowTauWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Kroot")) && settings.AllowTauWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowKroot".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Auxiliaries".Translate()),
                        ref settings.AllowKrootAuxiliaries,
                        null,
                        !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) || !settings.AllowTauWeapons,
                        DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Kroot")) && settings.AllowTauWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowVespid,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")) || !settings.AllowTauWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Vespid")) && settings.AllowTauWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowVespid".Translate() + (!DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Auxiliaries".Translate()),
                        ref settings.AllowVespidAuxiliaries,
                        null,
                        !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) || !settings.AllowTauWeapons,
                        DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("OG_Alien_Vespid")) && settings.AllowTauWeapons);
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