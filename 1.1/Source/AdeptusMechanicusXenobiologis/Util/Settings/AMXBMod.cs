using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.settings
{

    class AMXBMod : AMMod
    {

        public AMXBMod(ModContentPack content) : base(content)
        {

        }
        /*
        public override void OrkSettings(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkTek, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")));
            Widgets.CheckboxLabeled(rect.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkFeral, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")));
            Widgets.CheckboxLabeled(rect.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowOrkRok".Translate(), ref settings.AllowOrkRok, false);
            listing_Standard.EndSection(listing_Standard);
        }
        */


        public override void WriteSettings()
        {

        }
    }
    
}