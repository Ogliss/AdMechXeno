using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AdeptusMechanicus
{
	// Token: 0x0200086C RID: 2156
	public class CompProperties_SporeMine : CompProperties
	{
		// Token: 0x0600351E RID: 13598 RVA: 0x00122CA4 File Offset: 0x00120EA4
		public CompProperties_SporeMine()
		{
			this.compClass = typeof(CompSporeMine);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x00122D1D File Offset: 0x00120F1D
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00122D39 File Offset: 0x00120F39
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompExplosive requires Normal ticker type";
			}
			yield break;
		}

		// Token: 0x04001C4F RID: 7247
		public float explosiveRadius = 1.9f;

		// Token: 0x04001C50 RID: 7248
		public DamageDef explosiveDamageType;

		// Token: 0x04001C51 RID: 7249
		public int damageAmountBase = -1;

		// Token: 0x04001C52 RID: 7250
		public float armorPenetrationBase = -1f;

		// Token: 0x04001C53 RID: 7251
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04001C54 RID: 7252
		public float postExplosionSpawnChance;

		// Token: 0x04001C55 RID: 7253
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04001C56 RID: 7254
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04001C57 RID: 7255
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04001C58 RID: 7256
		public float preExplosionSpawnChance;

		// Token: 0x04001C59 RID: 7257
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04001C5A RID: 7258
		public float chanceToStartFire;

		// Token: 0x04001C5B RID: 7259
		public bool damageFalloff;

		// Token: 0x04001C5C RID: 7260
		public bool explodeOnKilled;

		// Token: 0x04001C5D RID: 7261
		public float explosiveExpandPerStackcount;

		// Token: 0x04001C5E RID: 7262
		public float explosiveExpandPerFuel;

		// Token: 0x04001C5F RID: 7263
		public EffecterDef explosionEffect;

		// Token: 0x04001C60 RID: 7264
		public SoundDef explosionSound;

		// Token: 0x04001C61 RID: 7265
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x04001C62 RID: 7266
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x04001C63 RID: 7267
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x04001C64 RID: 7268
		public float wickScale = 1f;

		// Token: 0x04001C65 RID: 7269
		public float chanceNeverExplodeFromDamage;

		// Token: 0x04001C66 RID: 7270
		public float destroyThingOnExplosionSize;

		// Token: 0x04001C67 RID: 7271
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x04001C68 RID: 7272
		public IntRange? countdownTicks;

		// Token: 0x04001C69 RID: 7273
		public string extraInspectStringKey;
	}
	// Token: 0x02000D06 RID: 3334
	public class CompSporeMine : ThingComp
	{
		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x001B3182 File Offset: 0x001B1382
		public CompProperties_SporeMine Props
		{
			get
			{
				return (CompProperties_SporeMine)this.props;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x001B318F File Offset: 0x001B138F
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x001B31B0 File Offset: 0x001B13B0
		private bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = this.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value < this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001B3202 File Offset: 0x001B1402
		public void AddThingsIgnoredByExplosion(List<Thing> things)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			this.thingsIgnoredByExplosion.AddRange(things);
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001B3224 File Offset: 0x001B1424
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
			Scribe_Values.Look<int>(ref this.countdownTicksLeft, "countdownTicksLeft", 0, false);
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x001B32A8 File Offset: 0x001B14A8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.Props.countdownTicks != null)
			{
				this.countdownTicksLeft = this.Props.countdownTicks.Value.RandomInRange;
			}
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001B32E8 File Offset: 0x001B14E8
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
			else
			{
				Pawn wearer = parent as Pawn;
				if (wearer!=null)
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
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001B3370 File Offset: 0x001B1570
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001B33CB File Offset: 0x001B15CB
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001B33E7 File Offset: 0x001B15E7
		public override void PostDraw()
		{
			if (this.wickStarted)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001B340D File Offset: 0x001B160D
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.KillFinalize && this.Props.explodeOnKilled)
			{
				this.Detonate(previousMap, true);
			}
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001B3428 File Offset: 0x001B1628
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
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

		// Token: 0x06005115 RID: 20757 RVA: 0x001B34E8 File Offset: 0x001B16E8
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

		// Token: 0x06005116 RID: 20758 RVA: 0x001B3574 File Offset: 0x001B1774
		public void StartWick(Thing instigator = null)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (this.ExplosiveRadius() <= 0f)
			{
				return;
			}
			this.instigator = instigator;
			this.wickStarted = true;
			this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
			this.StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null);
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001B35D9 File Offset: 0x001B17D9
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001B35EC File Offset: 0x001B17EC
		public float ExplosiveRadius()
		{
			CompProperties_SporeMine props = this.Props;
			float num = props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001B367C File Offset: 0x001B187C
		public void Detonate(Map map, bool ignoreUnspawned = false)
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
				Log.Warning("Tried to detonate CompExplosive in a null map.", false);
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

		// Token: 0x0600511A RID: 20762 RVA: 0x001B3816 File Offset: 0x001B1A16
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001B3838 File Offset: 0x001B1A38
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (this.countdownTicksLeft != -1)
			{
				text += "DetonationCountdown".Translate(this.countdownTicksLeft.TicksToDays().ToString("0.0"));
			}
			if (this.Props.extraInspectStringKey != null)
			{
				text += ((text != "") ? "\n" : "") + this.Props.extraInspectStringKey.Translate();
			}
			return text;
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001B38CF File Offset: 0x001B1ACF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.countdownTicksLeft > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Trigger countdown",
					action = delegate ()
					{
						this.countdownTicksLeft = 1;
					}
				};
			}
			yield break;
		}

		// Token: 0x04002CF2 RID: 11506
		public bool wickStarted;

		// Token: 0x04002CF3 RID: 11507
		protected int wickTicksLeft;

		// Token: 0x04002CF4 RID: 11508
		private Thing instigator;

		// Token: 0x04002CF5 RID: 11509
		private int countdownTicksLeft = -1;

		// Token: 0x04002CF6 RID: 11510
		public bool destroyedThroughDetonation;

		// Token: 0x04002CF7 RID: 11511
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x04002CF8 RID: 11512
		protected Sustainer wickSoundSustainer;
	}
}
