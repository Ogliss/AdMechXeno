using System;
using System.Runtime.CompilerServices;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    public class SporeMine : ArcingBullet
    {

        public override Graphic Graphic
        {
            get
            {
               return base.DefaultGraphic.GetColoredVersion(base.DefaultGraphic.Shader, Launcher.DrawColor, DrawColorTwo);
            }
        }

		public override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Faction faction = this.launcher.Faction;
			ThingDef raceDef = this.def.projectile.postExplosionSpawnThingDef;
            if (raceDef != null)
            {
                PawnKindDef mineDef = PawnKindDef.Named("OG_" + raceDef.defName);
                if (mineDef != null)
                {
                    Pawn pawn = GenSpawn.Spawn(PawnGenerator.GeneratePawn(mineDef, faction), base.Position, base.Map, WipeMode.Vanish) as Pawn;
                    if (pawn != null)
                    {
                        pawn.health.AddHediff(HediffDefOf.Scaria, null, null, null);
                        pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false);
                    }
                }
                else
                {
                    Log.Warning($"Spore mine PawnKindDef missing for {this.def.defName}()");
                }
            }
            else
            {
                Log.Warning($"Spore mine race def missing for {this.def.defName}");
            }
			GenClamor.DoClamor(this, 2.1f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}
	}
}
