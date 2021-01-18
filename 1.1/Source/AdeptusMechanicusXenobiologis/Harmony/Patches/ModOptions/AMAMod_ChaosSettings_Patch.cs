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

        private static bool showXB => settings.ShowXenobiologisSettings;
        private static bool showRaces => settings.ShowAllowedRaceSettings && showXB;
        private static bool setting => settings.ShowAllowedRaceSettings && settings.ShowChaos;

        private static int Options = 4;
        private static float RaceSettings => mod.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0, 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void ChaosSettings_Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, Rect rect, Rect inRect, float num,ref float num2)
        {
            if (AdeptusIntergrationUtility.enabled_XenobiologisChaos)
            {
                return;
            }
            if (showRaces)
            {
                string label = "AMXB_ShowChaos".Translate() + " Settings";
                string tooltip = "AMA_ShowSpecialRulesDesc".Translate();
                if (Dev)
                {
                    label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
                }
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowChaos, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
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
                    // move to intergration menu

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
                    MenuLength = listing_General.CurHeight != 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength - inc;
            }

        }
    }

}