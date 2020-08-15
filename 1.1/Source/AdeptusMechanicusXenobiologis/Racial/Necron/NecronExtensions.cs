using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class NecronExtensions
    {
        public static bool isNecron(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == OGNecronDefOf.OG_Flesh_Construct_Necron;
        }
    }
}
