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
        bool flag1;
        bool flag2;
        bool flag3;
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

            if (logging == true) Log.Message(string.Format("DDAWC 0"));
            if (corpse.InnerPawn.kindDef == OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue)
            {
                if (logging == true) Log.Message(string.Format("DDAWC 0 a"));
                spawn = true;
                pawnKindDef = OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Brimstone;
            }
            else if (corpse.InnerPawn.kindDef == OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Pink)
            {
                if (logging == true) Log.Message(string.Format("DDAWC 0 b"));
                spawn = true;
                pawnKindDef = OGChaosDeamonDefOf.OG_Chaos_Deamon_Lessar_Horror_Blue;
            }
            else
            {
                if (logging == true) Log.Message(string.Format("DDAWC 0 c"));
                pawnKindDef = corpse.InnerPawn.kindDef;
            }
            if (logging == true) Log.Message(string.Format("DDAWC 1"));
            if (spawn)
            {
                if (logging == true) Log.Message(string.Format("DDAWC 1 a"));
                pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, true, false, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                newPawn1 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                newPawn1.relations = relation;
                newPawn1.training = training;
                newPawn1.records = records;
                newPawn1.skills = skill;
                newPawn1.playerSettings = playersetting;

                if (logging == true) Log.Message(string.Format("DDAWC 1 b"));
                pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, true, false, true, true, 20f, fixedBiologicalAge: bioyears, fixedChronologicalAge: chronoyears);
                newPawn2 = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                newPawn2.relations = relation;
                newPawn2.training = training;
                newPawn2.records = records;
                newPawn2.skills = skill;
                newPawn2.playerSettings = playersetting;
                if (logging == true) Log.Message(string.Format("DDAWC 1 c"));
                if (lord!=null && lord.Map != null)
                {
                    if (logging == true) Log.Message(string.Format("Lord is {0}", lord));
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 d"));
                flag1 = map.mapPawns.SpawnedPawnsInFaction(pawn.Faction).Any((Pawn p) => p != newPawn1 && p != newPawn2 && p.GetLord() != null);
                if (logging == true) Log.Message(string.Format("DDAWC 1 e"));
                if (flag1)
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 e 1"));
                    Predicate<Thing> validator = (Thing p) => p != newPawn1 && p != newPawn2 && ((Pawn)p).GetLord() != null;
                    if (logging == true) Log.Message(string.Format("DDAWC 1 e 2"));
                    Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(position, map.mapPawns.SpawnedPawnsInFaction(newPawn1.Faction), 99999f, validator, null);
                    if (logging == true) Log.Message(string.Format("DDAWC 1 e 3"));
                    lord = p2.GetLord();
                    if (logging == true) Log.Message(string.Format("DDAWC 1 e 4"));
                    if (logging == true) Log.Message(string.Format("Lord is {0}", lord));
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 f "));
                flag2 = lord == null;
                if (logging == true) Log.Message(string.Format("DDAWC 1 g "));
                flag3 = flag2;
                if (logging == true) Log.Message(string.Format("DDAWC 1 h "));
                if (flag3)
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 a"));
                    try
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 a 1"));
                        LordJob_AssaultColony lordJob = new LordJob_AssaultColony(faction, false, false);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 a 2"));
                        lord = LordMaker.MakeNewLord((pawn.Faction), lordJob, map, null);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 a 3"));
                    }
                    catch
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 b 1"));
                    //    newPawn1.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 b 2"));
                    //    newPawn2.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 1 b 3"));
                    }
                }
                else
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 a"));
                    try
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 a 1"));
                        newPawn1.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 a 2"));
                        newPawn2.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 a 3"));
                    }
                    catch
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 b 1"));
                    //    newPawn1.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 b 2"));
                    //    newPawn2.mindState.mentalStateHandler.TryStartMentalState(OGChaosDeamonDefOf.MentalState_OGChaosDeamon);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 h 2 b 3"));
                    }
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 i "));
                if (spawn)
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 i 1 "));
                    Pawn p = (Pawn)GenClosest.ClosestThing_Global(position, map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, null, null);
                    if (logging == true) Log.Message(string.Format("DDAWC 1 i 2 "));
                    if (p.InMentalState)
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 i 2 a "));
                        if (p.MentalStateDef==MentalStateDefOf.PanicFlee  || p.GetLord() == null || p.GetLord().Map == null)
                        {
                            if (logging == true) Log.Message(string.Format("DDAWC 1 i 2 a 1"));
                            spawn = false;
                            if (logging == true) Log.Message(string.Format("{0} At: {1}", p.Name, p.Position, map));
                            if (logging == true) Log.Message(string.Format("{0} MentalState: {1}", p.Name, p.MentalState, map));
                            if (logging == true) Log.Message(string.Format("{0} mindState: {1}", p.Name, p.mindState, map));
                            if (logging == true) Log.Message(string.Format("{0} mindState duty: {1} thinkData: {2}", p.Name, p.mindState.duty, p.mindState.thinkData));
                            if (logging == true) Log.Message(string.Format("{0} mindState lastJobTag: {1} canFleeIndividual: {2}", p.Name, p.mindState.lastJobTag, p.mindState.canFleeIndividual));
                            if (logging == true) Log.Message(string.Format("{0} jobs: {1}", p.Name, p.jobs, map));
                            if (logging == true) Log.Message(string.Format("{0} jobs curDriver: {1} curJob: {2}", p.Name, p.jobs.curDriver, p.jobs.curJob));
                            if (logging == true) Log.Message(string.Format("{0} jobs jobs: {1} jobs: {2}", p.Name, p.jobs.jobQueue, p.jobs.posture));
                            if (logging == true) Log.Message(string.Format("{0} CurJob: {1}", p.Name, p.CurJob, map));
                            if (logging == true) Log.Message(string.Format("{0} CurJob lord: {1} def: {2}", p.Name, p.CurJob.lord, p.CurJob.def));
                            if (logging == true) Log.Message(string.Format("{0} CurJobDef: {1}", p.Name, p.CurJobDef, map));

                        }
                        if (logging == true) Log.Message(string.Format("DDAWC 1 i 2 b "));
                    }
                    if (logging == true) Log.Message(string.Format("DDAWC 1 i 3 "));
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 j "));
                if (map == null || position == null)
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 j a"));
                    if (logging == true) Log.Message(string.Format("no map or position"));
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 k "));
                if (spawn && pawnKindDef != corpse.InnerPawn.kindDef && lord != null && map != null)
                {
                    if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 "));
                    if (newPawn1 != null)
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 a"));
                        GenSpawn.Spawn(newPawn1, position, map, 0);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 b"));
                        if (lord != null)
                        {
                            if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 b 1"));
                            lord.AddPawn(newPawn1);
                            if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 b 2"));
                        }
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 1 c"));
                    }
                    if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 "));
                    if (newPawn2 != null)
                    {
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 a"));
                        GenSpawn.Spawn(newPawn2, position, map, 0);
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 b"));
                        if (lord != null)
                        {
                            if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 b 1"));
                            lord.AddPawn(newPawn2);
                            if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 b 2"));
                        }
                        if (logging == true) Log.Message(string.Format("DDAWC 1 k 2 c"));
                    }
                    if (logging == true) Log.Message(string.Format("DDAWC 1 k 3 "));
                }
                if (logging == true) Log.Message(string.Format("DDAWC 1 k "));
            }
            if (logging == true) Log.Message(string.Format("DDAWC 2"));
            if (position != null && map != null)
            {
                if (logging == true) Log.Message(string.Format("DDAWC 2 a"));
                GenExplosion.DoExplosion(position, map, 1.9f, OGChaosDeamonDefOf.OG_Chaos_Deamon_WarpfireDeath, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
            }
            if (logging == true) Log.Message(string.Format("DDAWC 3"));
            if (corpse != null)
            {
                if (logging == true) Log.Message(string.Format("DDAWC 3 a"));
                corpse.Destroy();
                if (logging == true) Log.Message(string.Format("DDAWC 3 b"));
            }
            if (logging == true) Log.Message(string.Format("DDAWC 4"));
        }
    }
}
