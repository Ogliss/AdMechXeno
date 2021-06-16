using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.IncidentWorker_Monolith
    public class IncidentWorker_Monolith : IncidentWorker
    {
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
                return Find.FactionManager.FirstFactionOfDef(AdeptusFactionDefOf.OG_Necron_Faction);
            }
        }

        public override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return map.listerThings.ThingsOfDef(this.def.mechClusterBuilding).Count <= 0;
        }

        public override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            int num = 0;
            int countToSpawn = this.CountToSpawn;
            List<TargetInfo> list = new List<TargetInfo>();
            Rand.PushState();
            float shrapnelDirection = Rand.Range(0f, 360f);
            Rand.PopState();
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

        private const float ShipPointsFactor = 0.9f;
        private const int IncidentMinimumPoints = 300;
    }
}
