using RimWorld;
using System;
using Verse;

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

		public override void PawnDied(Corpse corpse)
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
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 4.9f;
			}
			else
			{
				radius = 2.9f;
			};
			this.map = corpse.Map;
			DamageDef damage = DamageDefOf.Flame;
			int damageAmount = 0;
			float ap = 0f;
			CompProperties_SporeMine explosive = innerPawn.def.GetCompProperties<CompProperties_SporeMine>();
			if (explosive != null)
			{
				damage = explosive.explosiveDamageType;
				radius = explosive.explosiveRadius;
				damageAmount = explosive.explosiveDamageType.defaultDamage;
				ap = explosive.explosiveDamageType.defaultArmorPenetration;
			}
			/*
			if (innerPawn.def == OGTyranidDefOf.Tyranid_SporeMine_HE)
			{
				damage = DamageDefOf.Bomb;
			}
			Log.Message("PawnDied 10");
			Log.Message("position " + position.ToString());
			Log.Message("map " + this.map.ToString());
			Log.Message("radius " + radius.ToString());
			Log.Message("damage " + damage.ToString());
			Log.Message("InnerPawn " + corpse.InnerPawn.ToString());
			Log.Message("defaultDamage " + damageAmount.ToString());
			Log.Message("defaultArmorPenetration " + ap.ToString());
			*/
			GenExplosion.DoExplosion(position, this.map, radius, damage, corpse.InnerPawn, damageAmount, ap, null, null, null, null, null, 0f, 1, GasType.RotStink, false, null, 0f, 1, 0f, false, null, null);

		}
	}
}
