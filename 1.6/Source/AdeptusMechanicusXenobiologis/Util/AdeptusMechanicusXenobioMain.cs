using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class OGNecronModification
    {
        static OGNecronModification()
        {
            DefDatabase<ThingDef>.AllDefsListForReading.ForEach(action: td =>
            {
                if (td.defName.StartsWith("Corpse_Necron_") && !td.defName.Contains("ScarabSwarm") && td.IsCorpse)
                {
                    if (td.GetCompProperties<CompProperties_Rottable>() != null)
                    {
                        if (td.GetCompProperties<CompProperties_Rottable>() is CompProperties compprops)
                        {
                            td.comps.Remove(compprops);
                        }
                    }
                    if (td.GetCompProperties<CompProperties_SpawnerFilth>() != null)
                    {
                        if (td.GetCompProperties<CompProperties_SpawnerFilth>() is CompProperties compprops)
                        {
                            td.comps.Remove(compprops);
                        }
                    }
                    if (td.ingestible!=null)
                    {
                        td.ingestible.preferability = FoodPreferability.NeverForNutrition;
                    }
                }
            });
        }
    }
}