﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x0200034F RID: 847
    public class IncidentWorker_Monolith : IncidentWorker
    {
        // Token: 0x17000227 RID: 551
        // (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0006C28C File Offset: 0x0006A68C
        protected virtual int CountToSpawn
        {
            get
            {
                return 1;
            }
        }

        public Faction OfNecrons
        {
            get
            {
                return Find.FactionManager.FirstFactionOfDef(OGNecronDefOf.OG_Necron_Faction);
            }
        }
        // Token: 0x06000EA7 RID: 3751 RVA: 0x0006C290 File Offset: 0x0006A690
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return map.listerThings.ThingsOfDef(this.def.mechClusterBuilding).Count <= 0;
        }

        // Token: 0x06000EA8 RID: 3752 RVA: 0x0006C2D0 File Offset: 0x0006A6D0
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
                Building building_CrashedShipPart = (Building)ThingMaker.MakeThing(this.def.mechClusterBuilding, null);
                building_CrashedShipPart.SetFaction(OfNecrons, null);
                building_CrashedShipPart.GetComp<CompPawnSpawnerOnDamaged>().pointsLeft = Mathf.Max(parms.points * 0.9f, 300f);
                Skyfaller skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, building_CrashedShipPart);
                skyfaller.shrapnelDirection = shrapnelDirection;
                GenSpawn.Spawn(skyfaller, intVec, map, WipeMode.Vanish);
                num++;
                list.Add(new TargetInfo(intVec, map, false));
            }
            if (num > 0)
            {
                base.SendStandardLetter(parms, list, Array.Empty<NamedArgument>());
            }
            return num > 0;
        }

        // Token: 0x0400094F RID: 2383
        private const float ShipPointsFactor = 0.9f;

        // Token: 0x04000950 RID: 2384
        private const int IncidentMinimumPoints = 300;
    }
}
