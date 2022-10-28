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
        public static bool IsMechanicus(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Human_Mechanicus;
        }
        public static bool isOgyrn(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Abhuman_Ogryn;
        }
        public static bool isRatlin(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Abhuman_Ratlin;
        }

    }

}
