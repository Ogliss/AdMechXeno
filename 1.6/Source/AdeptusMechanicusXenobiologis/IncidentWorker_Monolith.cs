using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x0200034B RID: 843
    internal abstract class IncidentWorker_Monolith : IncidentWorker
    {
        // Token: 0x1700021F RID: 543
        // (get) Token: 0x06000E84 RID: 3716 RVA: 0x0006AC8E File Offset: 0x0006908E
        protected virtual int CountToSpawn
        {
            get
            {
                return 1;
            }
        }

        // Token: 0x06000E85 RID: 3717 RVA: 0x0006AC94 File Offset: 0x00069094
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return map.listerThings.ThingsOfDef(this.def.shipPart).Count <= 0;
        }

        // Token: 0x06000E86 RID: 3718 RVA: 0x0006ACD4 File Offset: 0x000690D4
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            int num = 0;
            int countToSpawn = this.CountToSpawn;
            List<TargetInfo> list = new List<TargetInfo>();
            float shrapnelDirection = Rand.Range(0f, 360f);
            for (int i = 0; i < countToSpawn; i++)
            {
                IntVec3 intVec;
                if (!CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.CrashedShipPartIncoming, map, out intVec, 14, default(IntVec3), -1, false, true, true, true, true, false, null))
                {
                    break;
                }
                Building_Monolith building_Monolith = (Building_Monolith)ThingMaker.MakeThing(this.def.shipPart, null);
                building_Monolith.SetFaction(OGNFaction.OfNecrons, null);
                building_Monolith.GetComp<CompSpawnerNecronsOnDamaged>().pointsLeft = Mathf.Max(parms.points * 0.9f, 300f);
                Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, building_Monolith);
                skyfaller.shrapnelDirection = shrapnelDirection;
                GenSpawn.Spawn(skyfaller, intVec, map, WipeMode.Vanish);
                num++;
                list.Add(new TargetInfo(intVec, map, false));
            }
            if (num > 0)
            {
                base.SendStandardLetter(list, null, new string[0]);
            }
            return num > 0;
        }

        // Token: 0x04000942 RID: 2370
        private const float ShipPointsFactor = 0.9f;

        // Token: 0x04000943 RID: 2371
        private const int IncidentMinimumPoints = 300;
    }
}
