using System;
using System.Collections.Generic;
using AdeptusMechanicus;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_ChaosDeamon
    public class DeathActionWorker_ChaosDeamon : DeathActionWorker_WarpSpawn
    {
        public override void PawnDied(Corpse corpse)
        {
            PawnGenerationRequest pawnGenerationRequest;
            PawnKindDef pawnKindDef = corpse.InnerPawn.kindDef;
            Pawn pawn = corpse.InnerPawn;
            IntVec3 position = corpse.Position;
            Map map = corpse.Map;
            Faction faction = pawn.Faction;
            float bioyears = pawn.ageTracker.AgeBiologicalYears;
            float chronoyears = pawn.ageTracker.AgeChronologicalYears;
            Pawn_MindState mind = pawn.mindState;
            Pawn newPawn1 = null;
            Pawn newPawn2 = null;
            bool spawn = false;
            Lord  lord = pawn.GetLord();
            List<Thing> spawned = new List<Thing>();
            if (map != null)
            {
                if (corpse.InnerPawn.kindDef == AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue)
                {
                    spawn = true;
                    pawnKindDef = AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone;
                }
                else if (corpse.InnerPawn.kindDef == AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink)
                {
                    spawn = true;
                    pawnKindDef = AdeptusPawnKindDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue;
                }
                if (spawn)
                {
                    pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, true, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                    newPawn1 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                    newPawn2 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                    spawned = new List<Thing>()
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
                    if (spawn && pawnKindDef != corpse.InnerPawn.kindDef && lord != null && map != null)
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
            if (position != null && map != null)
            {
                GenExplosion.DoExplosion(position, map, 1.9f, AdeptusDamageDefOf.OG_Chaos_Deamon_WarpfireDeath, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, GasType.Unused, false, null, 0f, 1, 0f, false, null, spawned);
            }
            base.PawnDied(corpse);
        }

    }
}
