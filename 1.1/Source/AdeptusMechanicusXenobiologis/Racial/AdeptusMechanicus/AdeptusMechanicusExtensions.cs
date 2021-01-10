using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class MechanicusExtensions
    {
        public static bool isMechanicus(this Pawn pawn)
        {
            return pawn.def == OGAdeptusMechanicusDefOf.OG_Human_Mechanicus;
        }

    }

}
