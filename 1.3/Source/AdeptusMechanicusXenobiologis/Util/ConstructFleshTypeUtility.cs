using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class ConstructFleshTypeUtility
    {
        static ConstructFleshTypeUtility()
        {
            DefDatabase<ThingDef>.AllDefsListForReading.ForEach(action: td =>
            {
                if (td.IsCorpse && td.defName.StartsWith("Corpse"))
                {
                    if (td.ingestible.sourceDef is ThingDef raceDef)
                    {
                        if (raceDef.race.FleshType!=null)
                        {
                            if (raceDef.race.FleshType.defName.Contains("OG_Flesh_Construct_"))
                            {
                            //    Log.Message(string.Format("removing rottability from {0} corpses", raceDef.LabelCap));
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
                                if (td.ingestible != null)
                                {
                                    td.ingestible.preferability = FoodPreferability.NeverForNutrition;
                                }
                            }
                        }
                        else
                        {
                            Log.Warning("race has no fleshtype");
                        }
                    }
                    else
                    {
                        Log.Warning("race is null");
                    }
                }
            });
        }

    }
}