using System.Collections.Generic;
using AdeptusMechanicus.settings;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_ChaosHorror
    public class DeathActionWorker_ChaosHorror : DeathActionWorker_ChaosDeamon
    {
        public override List<Thing> ThingsToIgnore
        {
            get
            {
                return spawnedPawns;
            }
        }
        public override float SpawnThingOnDeathChance(Pawn pawn)
        {
            return base.SpawnThingOnDeathChance(pawn);
        }
        List<Thing> spawnedPawns = new List<Thing>();
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            PawnGenerationRequest pawnGenerationRequest;
            Pawn pawn = corpse.InnerPawn;
            if (AMAMod.Dev && pawn == null) Log.Message("pawn null");
            PawnKindDef pawnKindDef = pawn.kindDef;
            if (AMAMod.Dev && pawnKindDef == null) Log.Message("pawnKindDef null");
            IntVec3 position = corpse.Position;
            if (AMAMod.Dev && position == null) Log.Message("position null");
            Map map = corpse.Map;
            Faction faction = pawn.Faction;
            if (AMAMod.Dev && faction == null) Log.Message("faction null");
            float bioyears = pawn.ageTracker.AgeBiologicalYears;
            float chronoyears = pawn.ageTracker.AgeChronologicalYears;
            /*
            Pawn_MindState mind = pawn.mindState;
            if (AMAMod.Dev && mind == null) Log.Message("mind null");
            */
            Pawn newPawn1 = null;
            Pawn newPawn2 = null;
            bool spawn = false;
            Lord  lord = pawn.GetLord();
            if (AMAMod.Dev && lord == null) Log.Message("lord null");
            base.PawnDied(corpse, prevLord);
            if (map != null && position != null)
            {
                PawnKindDef newKindDef = null;
                if (pawnKindDef == AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue || pawnKindDef == AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink)
                {
                    newKindDef = pawnKindDef == AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink ? AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue : AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone;
                }
                if (newKindDef != null)
                {
                    pawnGenerationRequest = new PawnGenerationRequest(newKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, true, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                    newPawn1 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                    newPawn2 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                    spawnedPawns = new List<Thing>()
                    {
                        newPawn1,
                        newPawn2
                    };
                    if (pawn.Faction != null)
                    {
                        if (map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != newPawn1 && p != newPawn2 && p.GetLord() != null))
                        {
                            bool validator(Thing p) => p != newPawn1 && p != newPawn2 && ((Pawn)p).GetLord() != null;
                            Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(position, map.mapPawns.SpawnedPawnsInFaction(newPawn1.Faction), 99999f, validator, null);
                            lord = p2.GetLord();
                        }
                    }
                    if (lord == null)
                    {
                        try
                        {
                            LordJob_AssaultColony lordJob = new LordJob_AssaultColony(faction, false, false);
                            lord = LordMaker.MakeNewLord((pawn.Faction), lordJob, map, null);
                        }
                        catch
                        {
                            //    newPawn1.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                            //    newPawn2.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        }
                    }
                    else
                    {
                        try
                        {
                            newPawn1.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
                            newPawn2.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
                        }
                        catch
                        {
                            //    newPawn1.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                            //    newPawn2.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        }
                    }
                    if (spawn)
                    {
                        Pawn p = (Pawn)GenClosest.ClosestThing_Global(position, map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, null, null);
                        if (p.InMentalState)
                        {
                            if (p.MentalStateDef == MentalStateDefOf.PanicFlee || p.GetLord() == null || p.GetLord().Map == null)
                            {
                                spawn = false;
                            }
                        }
                    }
                    if (spawn && pawnKindDef != pawn.kindDef)
                    {
                        if (newPawn1 != null)
                        {
                            GenSpawn.Spawn(newPawn1, position, map, 0);
                            if (lord != null)
                            {
                                lord.AddPawn(newPawn1);
                            }
                        }
                        if (newPawn2 != null)
                        {
                            GenSpawn.Spawn(newPawn2, position, map, 0);
                            if (lord != null)
                            {
                                lord.AddPawn(newPawn2);
                            }
                        }
                    }
                }
            }
        }

    }
}
