using RimWorld;
using System;

namespace Verse.AI
{
    public class MentalState_OGChaosDeamon : MentalState
    {

        public override bool ForceHostileTo(Thing t)
        {
            return t.Faction != null && this.ForceHostileTo(t.Faction);
        }

        public override bool ForceHostileTo(Faction f)
        {
            return f.def.humanlikeFaction || f == Faction.OfMechanoids || f != Find.FactionManager.FirstFactionOfDef(OGChaosDeamonDefOf.OG_Chaos_Deamon_Faction) || f != AdeptusMechanicus.AdeptusIntergrationUtil.OfChaosMarine || f != AdeptusMechanicus.AdeptusIntergrationUtil.OfTraitorGuard;
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}
