using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.CompProperties_HediffChangeableProjectile
    public class CompProperties_HediffChangeableProjectile : CompProperties_ChangeableProjectile
    {
		public CompProperties_HediffChangeableProjectile()
		{
			this.compClass = typeof(CompHediffChangeableProjectile);
		}
		public bool consumeSeverity = false;
		public List<Pair<HediffDef, ThingDef>> HediffAmmunition;
    }

    public class CompHediffChangeableProjectile : CompChangeableProjectile
    {
		public Pawn Holder => this.parent.ParentHolder as Pawn;
		public new CompProperties_HediffChangeableProjectile Props => base.Props as CompProperties_HediffChangeableProjectile;

        public new ThingDef LoadedShell
        {
            get
            {
				if (this.Props.HediffAmmunition.NullOrEmpty())
				{
					return null;
				}
				else if (loadedShell == null && Holder != null)
				{
					foreach (var item in this.Props.HediffAmmunition)
					{
						Hediff hediff = Holder.health.hediffSet.GetFirstHediffOfDef(item.First);

                        if (hediff != null)
						{
							this.loadedShell = item.Second;
							break;
                        }
					}
				}
                return this.loadedShell;
            }
        }
        public new ThingDef Projectile
        {
            get
            {
                if (!this.Loaded)
                {
                    return null;
                }
                return this.LoadedShell;
            }
        }
		public override void Notify_ProjectileLaunched()
		{
			base.Notify_ProjectileLaunched();
		}

        public new bool StorageTabVisible
        {
            get
            {
				return false;
            }
        }
    }

    public class CompProperties_SporeMine : CompProperties_Explosive
    {
        public CompProperties_SporeMine()
        {
            this.compClass = typeof(CompSporeMine);
        }

    }
    public class CompSporeMine : CompExplosive
	{
		public new CompProperties_SporeMine Props
		{
			get
			{
				return (CompProperties_SporeMine)this.props;
			}
		}

		public override void CompTick()
		{
			if (this.countdownTicksLeft > 0)
			{
				this.countdownTicksLeft--;
				if (this.countdownTicksLeft == 0)
				{
					this.StartWick(null);
					this.countdownTicksLeft = -1;
				}
			}
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld, false);
				}
            }
			detCheckTicks--;
			if (detCheckTicks == 0)
			{
                Thing danger;
                if (parent is Pawn pawn && GenAI.EnemyIsNear(pawn, Props.explosiveRadius - 0.5f, out danger, false, true))
                {
                    this.Detonate(danger.Map, false);
                }
				else
				{
					detCheckTicks = 60;
                }
            }
            /*
        else
        {
            if (parent is Pawn wearer)
            {
                Thing thing = wearer.mindState.enemyTarget;
                if (thing != null)
                {
                    if (thing.Position.DistanceTo(wearer.Position) < (float)(Props.explosiveRadius * 0.75f))
                    {
                        this.Detonate(this.parent.MapHeld, false);
                    }
                }
            }
        }
            */
        }
		private int detCheckTicks = 60;

		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.ExternalViolenceFor(this.parent) && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.Detonate(this.parent.MapHeld, false);
						if (this.parent.Destroyed)
						{
							absorbed = true;
							return;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && this.Props.startWickOnDamageTaken.Contains(dinfo.Def))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.CanEverExplodeFromDamage)
			{
				return;
			}
			if (!this.CanExplodeFromDamageType(dinfo.Def))
			{
				return;
			}
			if (!this.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
					return;
				}
				if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold && dinfo.Def.ExternalViolenceFor(this.parent))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		public new void Detonate(Map map, bool ignoreUnspawned = false)
		{
			if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			CompProperties_SporeMine props = this.Props;
			float num = this.ExplosiveRadius();
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
			}
			if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
			{
				this.destroyedThroughDetonation = true;
				this.parent.Kill(null, null);
			}
			this.EndWickSustainer();
			this.wickStarted = false;
			if (map == null)
			{
				Log.Warning("Tried to detonate CompExplosive in a null map.");
				return;
			}
			/*
			if (props.explosionEffect != null)
			{
				Effecter effecter = props.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false));
				effecter.Cleanup();
			}
			Thing parent;
			if (this.instigator != null && !this.instigator.HostileTo(this.parent.Faction))
			{
				parent = this.instigator;
			}
			else
			{
				parent = this.parent;
			}
			GenExplosion.DoExplosion(this.parent.PositionHeld, map, num, props.explosiveDamageType, parent, props.damageAmountBase, props.armorPenetrationBase, props.explosionSound, null, null, null, props.postExplosionSpawnThingDef, props.postExplosionSpawnChance, props.postExplosionSpawnThingCount, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount, props.chanceToStartFire, props.damageFalloff, null, this.thingsIgnoredByExplosion);
			*/
		}

	}
}
