using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ChaosSettings")]
    public static class AMAMod_ChaosSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;

        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private static bool Setting => settings.ShowAllowedRaceSettings && settings.ShowChaos;

        private static int Options = 4;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0, 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void ChaosSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            if (AdeptusIntergrationUtility.enabled_XenobiologisChaos)
            {
                return;
            }
            if (ShowRaces)
            {
                string label = "AdeptusMechanicus.Xenobiologis.ShowChaos".Translate() + " Settings";
                string tooltip = "AdeptusMechanicus.ShowSpecialRulesDesc".Translate();
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowChaos, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= AdeptusIntergrationUtility.enabled_EndTimesWithGuns ? 0.32f : 0.488f;

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosMarine".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowChaosMarine,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) || !settings.AllowChaosWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Marine")) && settings.AllowChaosWeapons);

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosGuard".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowChaosGuard,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) || !settings.AllowChaosWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Guard")) && settings.AllowChaosWeapons);

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowChaosMechanicus,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) && settings.AllowChaosWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Mechanicus")) && settings.AllowChaosWeapons);

                    listing_General.NewColumn();

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosDeamons".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowChaosDeamons,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")),
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosDeamonicIncursion".Translate(),
                        ref settings.AllowChaosDeamonicIncursion,
                        null,
                        !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")) || !settings.AllowChaosDeamons,
                        DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Deamonic_Incursion")) && settings.AllowChaosDeamons);

                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowChaosDeamonicInfestation".Translate(),
                        ref settings.AllowChaosDeamonicInfestation,
                        null,
                        !DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")) || !settings.AllowChaosDeamons,
                        DefDatabase<IncidentDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon_Daemonic_Infestation")) && settings.AllowChaosDeamons);
                    // move to intergration menu

                    if (AdeptusIntergrationUtility.enabled_EndTimesWithGuns)
                    {
                        listing_General.NewColumn();

                        listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergration".Translate(),
                            ref settings.EndTimesIntergrateDeamons,
                            "AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergrationDesc".Translate(),
                            !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")),
                            DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")));
                        listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergration_GreatPortal".Translate(),
                            ref settings.EndTimesIntergrateDeamonsGreat,
                            "AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergration_GreatPortalDesc".Translate(),
                            !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) || !settings.EndTimesIntergrateDeamons,
                            DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) && settings.EndTimesIntergrateDeamons);
                        listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergration_SmallPortal".Translate(),
                            ref settings.EndTimesIntergrateDeamonsSmall,
                            "AdeptusMechanicus.Xenobiologis.EndTimesChaosDeamonIntergration_SmallPortalDesc".Translate(),
                            !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) || !settings.EndTimesIntergrateDeamons,
                            DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Chaos_Deamon")) && settings.EndTimesIntergrateDeamons);

                    }

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