using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ImperialSettings")]
    public static class AMAMod_ImperialSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool showXB => settings.ShowXenobiologisSettings;
        private static bool showRaces => settings.ShowAllowedRaceSettings && showXB;
        private static bool setting => showRaces && settings.ShowImperium;

        private static float options => mod.Length(setting, 2, lineheight, 0, 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        public static float MenuLengthAstartes = 0;
        public static float MenuLengthMechanicus = 0;
        public static float MenuLengthMilitarum = 0;
        public static float MenuLengthSororitas = 0;
        public static float MenuLengthInquisition = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static bool ImperialSettings_Prefix(ref AMAMod __instance ,ref Listing_StandardExpanding listing_Main, Rect rect, ref Rect inRect, float num, ref float num2)
        {
            string label = "AdeptusMechanicus.Xenobiologis.ShowImperium".Translate() + " Settings";
            string tooltip = string.Empty;
            if (Dev)
            {
                label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
            }
            float w = rect.width * 0.480f;
            if (showRaces)
            {
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection(num2 + inc, false, 3, 4, 0);
                if (listing_Race.CheckboxLabeled(label, ref settings.ShowImperium, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                {
                    // left side
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true, parent: listing_Main);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowAdeptusAstartes".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowAdeptusAstartes,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Astartes")) || !AMSettings.Instance.AllowImperialWeapons,
                        AMSettings.Instance.AllowImperialWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowAdeptusMechanicus".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowAdeptusMechanicus,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Mechanicus")) || !AMSettings.Instance.AllowMechanicusWeapons);

                    listing_General.NewColumn();
                    // right side
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowAdeptusMilitarum".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowAdeptusMilitarum,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Militarum")) || !AMSettings.Instance.AllowImperialWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowAdeptusSororitas".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowAdeptusSororitas,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Sororitas")) || !AMSettings.Instance.AllowImperialWeapons);
                    listing_Race.EndSection(listing_General);
                    MenuLength = listing_General.CurHeight != 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                    // add Astartes options if enabled
                    if (AdeptusIntergrationUtility.enabled_AdeptusAstartes && settings.AllowAdeptusAstartes)
                    {
                        __instance.AstartesSettings(ref listing_Race, rect, inRect, w, ref MenuLengthAstartes);
                    }
                    /*
                    // add Mechanicus options if enabled
                    if (settings.AllowAdeptusMechanicus && AdeptusIntergrationUtil.enabled_AdeptusMechanicus)
                    {
                        __instance.MechanicusSettings(ref listing_Main, rect, inRect, w, ref MenuLengthMechanicus);
                    }
                    // add Militarum options if enabled
                    if (settings.AllowAdeptusMilitarum && AdeptusIntergrationUtil.enabled_AdeptusMilitarum)
                    {
                        __instance.MilitarumSettings(ref listing_Main, rect, inRect, w, ref MenuLengthMilitarum);
                    }
                    // add Sororitas options if enabled
                    if (settings.AllowAdeptusSororitas && AdeptusIntergrationUtil.enabled_AdeptusSororitas)
                    {
                        __instance.SororitasSettings(ref listing_Main, rect, inRect, w, ref MenuLengthSororitas);
                    }
                    // add Inquisition options if enabled
                    if (settings.AllowInquisition && AdeptusIntergrationUtil.enabled_OrdoInquisition)
                    {
                        __instance.SororitasSettings(ref listing_Main, rect, inRect, w, ref MenuLengthInquisition);
                    }
                    */
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength;
            }
            return false;
        }
    }

}