using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
	public class DeathActionWorker_SporeMine : DeathActionWorker
	{

		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (corpse == null )
			{
				return;
			}
			if (corpse.Map == null || corpse.Position == null || corpse.Position == IntVec3.Invalid)
			{
				return;
			}
			Pawn innerPawn = corpse.InnerPawn;
			IntVec3 position = corpse.Position;
			Detonate(corpse, position);
			corpse.Destroy(0);
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			if (innerPawn.def.killedLeavings!=null)
			{
				for (int i = 0; i < innerPawn.def.killedLeavings.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(innerPawn.def.killedLeavings[i].thingDef, null);
					thing.stackCount = innerPawn.def.killedLeavings[i].count;
					thingOwner.TryAdd(thing, true);
				}
				for (int j = 0; j < thingOwner.Count; j++)
				{
					GenPlace.TryPlaceThing(thingOwner[j], position, this.map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		private Map map;
		public void Detonate(Corpse corpse, IntVec3 position)
		{
			Pawn innerPawn = corpse.InnerPawn;
			float r;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				r = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				r = 4.9f;
			}
			else
			{
				r = 2.9f;
			};
			this.map = corpse.Map;
			CompSporeMine comp = innerPawn.TryGetCompFast<CompSporeMine>();
			if (comp != null)
            {

                CompProperties_SporeMine props = comp.Props;
                if (props != null)
                {
                    float num = comp.ExplosiveRadius();
                    IntVec3 positionHeld = innerPawn.PositionHeld;
                    float radius = num;
                    DamageDef explosiveDamageType = props.explosiveDamageType;
                    Thing thing = innerPawn;
                    int damageAmountBase = props.damageAmountBase;
                    float armorPenetrationBase = props.armorPenetrationBase;
                    SoundDef explosionSound = props.explosionSound;
                    ThingDef weapon = null;
                    ThingDef projectile = null;
                    Thing intendedTarget = null;
                    ThingDef postExplosionSpawnThingDef = props.postExplosionSpawnThingDef;
                    float postExplosionSpawnChance = props.postExplosionSpawnChance;
                    int postExplosionSpawnThingCount = props.postExplosionSpawnThingCount;
                    GasType? postExplosionGasType = props.postExplosionGasType;
                    bool applyDamageToExplosionCellsNeighbors = props.applyDamageToExplosionCellsNeighbors;
                    ThingDef preExplosionSpawnThingDef = props.preExplosionSpawnThingDef;
                    float preExplosionSpawnChance = props.preExplosionSpawnChance;
                    int preExplosionSpawnThingCount = props.preExplosionSpawnThingCount;
                    float chanceToStartFire = props.chanceToStartFire;
                    bool damageFalloff = props.damageFalloff;
                    float? direction = null;
                    List<Thing> ignoredThings = comp.thingsIgnoredByExplosion;
                    FloatRange? affectedAngle = null;
                    bool doVisualEffects = props.doVisualEffects;
                    bool doSoundEffects = props.doSoundEffects;
                    GenExplosion.DoExplosion(positionHeld, map, radius, explosiveDamageType, thing, damageAmountBase, armorPenetrationBase, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, null, 255 ,applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, chanceToStartFire, damageFalloff, direction, ignoredThings, affectedAngle, doVisualEffects, props.propagationSpeed, 0f, doSoundEffects, null, 1f, null, null);
                }
            }

		}
	}
}
