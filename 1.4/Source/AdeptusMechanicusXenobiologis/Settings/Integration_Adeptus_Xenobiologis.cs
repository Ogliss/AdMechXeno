using AdeptusMechanicus.settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{

    public class Integration_Adeptus_Xenobiologis : Integration_Adeptus
    {
        public override string PackageID => "Ogliss.AdMech.Xenobiologis";

        private static List<Integration_Adeptus> subMenus;
        public static List<Integration_Adeptus> SubMenus
        {
            get
            {
                if (subMenus == null)
                {
                    subMenus = new List<Integration_Adeptus>();

                    subMenusImperial = new List<Integration_Adeptus>();
                    subMenusChaos = new List<Integration_Adeptus>();
                    subMenusXenos = new List<Integration_Adeptus>();
                    foreach (var type in AMAMod.AdeptusMenus)
                    {
                        if (!subMenus.Any(x => x.GetType() == type))
                        {
                            var menu = (Integration_Adeptus)Activator.CreateInstance(type, null);
                            if (menu.XenobiologisSub)
                            {
                                if (menu.Catergory == "Imperial")
                                {
                                    subMenusImperial.Add(menu);
                                }
                                if (menu.Catergory == "Chaos")
                                {
                                    subMenusChaos.Add(menu);
                                }
                                if (menu.Catergory == "Xeno")
                                {
                                    subMenusXenos.Add(menu);
                                }
                            //    Log.Message($"XenobiologisSub loading {type.Name}");
                            }
                        }
                    }
                    if (!subMenusImperial.NullOrEmpty()) subMenus.Concat(subMenusImperial);
                    if (!subMenusChaos.NullOrEmpty()) subMenus.Concat(subMenusChaos);
                    if (!subMenusXenos.NullOrEmpty()) subMenus.Concat(subMenusXenos);
                }
                return subMenus;

            }
        }
        public static List<Integration_Adeptus> subMenusImperial;
        public static List<Integration_Adeptus> subMenusChaos;
        public static List<Integration_Adeptus> subMenusXenos;

        private bool ShowRaces => settings.ShowAllowedRaceSettings && ShowXB;
        private bool ShowImperial => settings.ShowImperium;
        private bool AllowAstartes => settings.AllowAdeptusAstartes;
        private bool ShowAstartes => settings.ShowAstartes;

        private float length_RaceMenu = 0;
        private float length_RaceMenuInc = 0;
        private float length_RaceMenuContent = 0;
        private float length_Imperial = 0;
        private float length_ImperialInc = 0;
        private float length_ImperialContent = 0;
        private float length_Choas = 0;
        private float length_ChoasInc = 0;
        private float length_ChoasContent = 0;
        private float length_Xenos = 0;
        private float length_XenosInc = 0;
        private float length_XenosContent = 0;
        public override void DrawSettings(Listing_StandardExpanding listing_Main)
        {
            string labelXB = "AdeptusMechanicus.Xenobiologis.ModName".Translate() + " Settings " + SubMenus.Count() + " Race settings";
            string tooltipXB = "AdeptusMechanicus.Xenobiologis.ShowOptionsDesc".Translate();
            if (listing_Main.ButtonText(labelXB, ref settings.ShowXenobiologisSettings, Dev, ref length_MenuInc, tooltipXB))
            {
                //    listing_Menu = listing_Main.BeginSection(listing_ArmouryLength, false, 3, 4, 0); 
                if (ShowXB)
                {
                    Listing_StandardExpanding listing_Menu = listing_Main.BeginSection(length_Menu + length_MenuInc, false, 0, 4, 0);
                    // Xenobiologis Mod General Options
                    {
                        Listing_StandardExpanding listing_General = listing_Menu.BeginSection(length_MenuContent, true, 3, 4, 4);
                        listing_General.ColumnWidth *= 0.488f;
                        listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.ForceRelations".Translate(), ref settings.ForceRelations, "AdeptusMechanicus.Xenobiologis.ForceRelationsDesc".Translate());
                        listing_General.NewColumn();
                        {
                            bool set = settings.AllowWarpstorm;
                            listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowWarpstorm".Translate(), ref settings.AllowWarpstorm, "AdeptusMechanicus.Xenobiologis.AllowWarpstormDesc".Translate());
                            if (set != settings.AllowWarpstorm)
                            {
                                AMAMod.updateIncidents_Disabled = true;
                            }
                        }
                        listing_Menu.EndSection(listing_General);
                        length_MenuContent = listing_General.MaxColumnHeightSeen;
                    }
            //        DrawFactionSettings(listing_Menu);
                    // Xenobiologis Mod Race Options
                    Listing_StandardExpanding listing_XBRaces = listing_Menu.BeginSection(length_RaceMenu + length_RaceMenuInc, out Rect _frame, out Rect _contents, false, 3, 4, 4);
                    //   Log.Message(listing_Menu.listingRect.height + " " + listing_Menu.CurHeight + " " + listing_Menu.MaxColumnHeightSeen);
                    string labelR = "AdeptusMechanicus.Xenobiologis.AllowedRaces".Translate() + " Settings";
                    string tooltipR = "AdeptusMechanicus.ShowSpecialRulesDesc".Translate();
                    if (Dev)
                    {
                        labelR += " Length: " + length_RaceMenu;
                    }
                    if (listing_XBRaces.CheckboxLabeled(labelR, ref settings.ShowAllowedRaceSettings, Dev, ref length_RaceMenuInc, tooltipR, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                    {
                        {
                            string label = "AdeptusMechanicus.Xenobiologis.ShowImperium".Translate() + " Settings";
                            string tooltip = string.Empty;
                            if (Dev)
                            {
                                label += " Main Length: " + length_Imperial + " SubLength: " + length_MenuContent + " Inc: " + length_ImperialInc;
                            }
                            Listing_StandardExpanding listing_Imperial = listing_XBRaces.BeginSection(length_Imperial + length_ImperialInc, false, 3, 4, 0);
                            if (listing_Imperial.CheckboxLabeled(label, ref settings.ShowImperium, Dev, ref length_ImperialInc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex))
                            {
                                DrawSubmenu_Imperial(listing_Imperial, subMenusImperial, 2f);
                            }
                            listing_XBRaces.EndSection(listing_Imperial);
                            length_Imperial = listing_Imperial.MaxColumnHeightSeen; // Math.Max(listing_Imperial.curY, listing_Imperial.MaxColumnHeightSeen);// listing_Race.CurHeight;
                        }
                        DrawSubmenus(listing_XBRaces, subMenusChaos);
                        DrawSubmenus(listing_XBRaces, subMenusXenos);
                    }
                    listing_Menu.EndSection(listing_XBRaces);
                    length_RaceMenu = listing_XBRaces.MaxColumnHeightSeen;
                    listing_Main.EndSection(listing_Menu);
                    length_Menu = listing_Menu.MaxColumnHeightSeen;
                }
                //    XenobiologisMenus.XenobiologisModOptionsMenu(ref __instance, ref listing_Main, rect, ref inRect, num, xenobiologisMenuLenght);
            }
        }

        public override void DrawFactionSettings(Listing_StandardExpanding listing_Races)
        {
            foreach (var item in ManagedFactions)
            {
                listing_Races.FactionSettingFor(item.FactionDef.LabelCap, item);
            }
        }

        public void DrawSubmenu_Imperial(Listing_StandardExpanding listing_Races, List<Integration_Adeptus> subMenus = null, float? Columns = null)
        {
            if (!subMenus.NullOrEmpty())
            {
                List<Integration_Adeptus> l = subMenus.OrderBy(x => x.Priority).ToList();
                float columns = Columns ?? 1f;
                float perColumn = l.Count / columns;
                Listing_StandardExpanding listing_Race = listing_Races.BeginSection(length_ImperialContent, true);
                if (Columns.HasValue) listing_Race.ColumnWidth *= (0.979f / columns);
                for (int i = 0; i < l.Count; i++)
                {
                    Integration_Adeptus cur = l[i];
                    if (Columns.HasValue && i != 0 && i % perColumn == 0)
                    {
                        listing_Race.NewColumn();
                    }
                    cur.DrawSettings(listing_Race);
                }
                listing_Races.EndSection(listing_Race);
                length_ImperialContent = listing_Race.MaxColumnHeightSeen;
            }
        }
        public void DrawSubmenus(Listing_StandardExpanding listing_Races, List<Integration_Adeptus> subMenus = null)
        {
            if (!subMenus.NullOrEmpty())
            {
                List<Integration_Adeptus> l = subMenus.OrderBy(x => x.Priority).ToList();
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].DrawSettings(listing_Races);
                }
            }
        }
    }
}
