using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
	internal class IncidentWorker_RipperSwarms : IncidentWorker
	{
		public override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(map.Tile, PawnKindDefOf.Alphabeaver.race) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
		}

		public override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnKindDef ripperSwarm = AdeptusPawnKindDefOf.OG_Tyranid_Ripper_Swarm;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			float freeColonistsCount = (float)map.mapPawns.FreeColonistsCount;
			float randomInRange = IncidentWorker_RipperSwarms.CountPerColonistRange.RandomInRange;
			int num = Mathf.Clamp(GenMath.RoundRandom(freeColonistsCount * randomInRange), MinCount, MaxCount);
			for (int i = 0; i < num; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
				((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(ripperSwarm, null), loc, map, WipeMode.Vanish)).needs.food.CurLevelPercentage = 1f;
			}
			base.SendStandardLetter("AdeptusMechanicus.Tyranid.LetterLabelRippersArrived".Translate(), "AdeptusMechanicus.Tyranid.RippersArrived".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 3f);

		private const int MinCount = 1;
		private const int MaxCount = 10;
	}
}
