using Verse;
using System.Reflection;
using HarmonyLib;
using System.Linq;
using System;

namespace AdeptusMechanicus.HarmonyInstance
{
    [StaticConstructorOnStartup]
    class AdeptusMechanicusXenoPatches
    {
        static AdeptusMechanicusXenoPatches()
        {
            //    HarmonyInstance.DEBUG = true;
            var harmony = new Harmony("com.ogliss.rimworld.mod.adeptus.xenobiologis");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            if (Prefs.DevMode) Log.Message(string.Format("Magos Xenobiologis: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))), false);
        }
    }

}