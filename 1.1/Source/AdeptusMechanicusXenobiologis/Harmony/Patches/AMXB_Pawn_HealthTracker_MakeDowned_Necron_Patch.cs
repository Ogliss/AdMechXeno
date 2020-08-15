using Verse;
using HarmonyLib;
using System.Reflection;
using AdeptusMechanicus.HarmonyInstance;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Pawn_EquipmentTracker), "DropAllEquipment")]
    public static class AMXB_Pawn_EquipmentTracker_DropAllEquipment_Necron_Patch
    {
        public static bool Prefix(ref Pawn ___pawn)
        {
            if (___pawn != null && ___pawn.isNecron())
            {
                return false;
            }
            return true;
        }
    }
}
