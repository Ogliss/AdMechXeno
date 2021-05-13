using System;
using AdeptusMechanicus;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_ChaosDeamon
    public class DeathActionWorker_ChaosDeamon : DeathActionWorker
    {
        Pawn pawn;
        Pawn oldpawn;
        float bioyears;
        float chronoyears;
        Pawn_PlayerSettings playersetting;
        Pawn_TrainingTracker training;
        Pawn_HealthTracker health;
        Pawn_RecordsTracker records;
        Pawn_RelationsTracker relation;
        Pawn_SkillTracker skill;
        Pawn_MindState mind;
        PawnKindDef pawnKindDef;
        PawnGenerationRequest pawnGenerationRequest;
        IntVec3 position;
        Map map;
        Faction faction;
        Pawn newPawn1 = null;
        Pawn newPawn2 = null;
        Pawn lordPawn = null;
        bool spawn = false;
        Lord lord;
        // Token: 0x0600403D RID: 16445 RVA: 0x001E1AD3 File Offset: 0x001DFED3
        public override void PawnDied(Corpse corpse)
        {
            pawn = corpse.InnerPawn;
            oldpawn = corpse.InnerPawn;
            position = corpse.Position;
            map = corpse.Map;
            faction = pawn.Faction;
            bioyears = pawn.ageTracker.AgeBiologicalYears;
            chronoyears = pawn.ageTracker.AgeChronologicalYears;
            playersetting = pawn.playerSettings;
            training = pawn.training;
            health = pawn.health;
            records = pawn.records;
            relation = pawn.relations;
            skill = pawn.skills;
            mind = pawn.mindState;
            newPawn1 = null;
            newPawn2 = null;
            spawn = false;
            lord = pawn.GetLord();
            bool logging = false;

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
            else
            {
                pawnKindDef = corpse.InnerPawn.kindDef;
            }
            if (spawn)
            {
                pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, true, false, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                newPawn1 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                newPawn1.relations = relation;
                newPawn1.training = training;
                newPawn1.records = records;
                newPawn1.skills = skill;
                newPawn1.playerSettings = playersetting;

                pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, true, false, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                newPawn2 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                newPawn2.relations = relation;
                newPawn2.training = training;
                newPawn2.records = records;
                newPawn2.skills = skill;
                newPawn2.playerSettings = playersetting;
                if (map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != newPawn1 && p != newPawn2 && p.GetLord() != null))
                {
                    Predicate<Thing> validator = (Thing p) => p != newPawn1 && p != newPawn2 && ((Pawn)p).GetLord() != null;
                    Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(position, map.mapPawns.SpawnedPawnsInFaction(newPawn1.Faction), 99999f, validator, null);
                    lord = p2.GetLord();
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
                        if (p.MentalStateDef==MentalStateDefOf.PanicFlee  || p.GetLord() == null || p.GetLord().Map == null)
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
            if (position != null && map != null)
            {
                GenExplosion.DoExplosion(position, map, 1.9f, AdeptusDamageDefOf.OG_Chaos_Deamon_WarpfireDeath, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
            }
            if (corpse != null)
            {
                corpse.Destroy();
            }
        }
    }
}
