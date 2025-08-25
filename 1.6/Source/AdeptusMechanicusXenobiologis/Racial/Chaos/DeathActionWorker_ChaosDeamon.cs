using System;
using System.Collections.Generic;
using AdeptusMechanicus;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_ChaosDeamon
    public class DeathActionWorker_ChaosDeamon : DeathActionWorker_WarpSpawn
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            Pawn pawn = corpse.InnerPawn;
            IntVec3 position = corpse.Position;
            Map map = corpse.Map;
            base.PawnDied(corpse, prevLord);
            if (position != null && map != null)
            {
                GenExplosion.DoExplosion(position, map, 1.9f * pawn.BodySize, AdeptusDamageDefOf.OG_Chaos_Deamon_WarpfireDeath, corpse.InnerPawn, -1, -1f, null, null, null, null, SpawnedThingOnDeath(pawn), 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, ignoredThings: ThingsToIgnore);
            }
        }
        public virtual List<Thing> ThingsToIgnore { get; set; } = new List<Thing>();
        public virtual ThingDef SpawnedThingOnDeath(Pawn pawn)
        {
            return null;
        }
        public virtual float SpawnThingOnDeathChance(Pawn pawn)
        {
            return 0f;
        }
    }
}
