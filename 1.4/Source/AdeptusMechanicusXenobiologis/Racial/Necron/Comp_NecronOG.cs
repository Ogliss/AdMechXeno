using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.settings;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    public class CompProperties_NecronOG : CompProperties
    {
        // Token: 0x06000AB1 RID: 2737 RVA: 0x0005598C File Offset: 0x00053D8C
        public CompProperties_NecronOG()
        {
            this.compClass = typeof(Comp_NecronOG);
        }

        // Token: 0x040004AB RID: 1195
        public float overlightRadius;

        // Token: 0x040004AC RID: 1196
        public float glowRadius = 14f;

        // Token: 0x040004AD RID: 1197
        public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;

        public bool Necrodermis = true;
        public float NecrodermisChance = 0.5f;
        public float NecrodermisInterval = 2.5f;

        public bool ReanimationProtocol = true;
        public float ReanimationProtocolChance = 0.5f;

        public bool PhaseOut = true;
        public float PhaseOutChance = 1f;

        public bool PhaseShifter = false;
        public float PhaseShifterMin = 0f;
    }
    // Token: 0x02000C66 RID: 3174
    public class Comp_NecronOG : ThingComp
    {
        // Token: 0x17000AD2 RID: 2770
        // (get) Token: 0x060045EE RID: 17902 RVA: 0x0020D7F1 File Offset: 0x0020BBF1
        public CompProperties_NecronOG Props
        {
            get
            {
                return (CompProperties_NecronOG)this.props;
            }
        }

        public bool ReviveFlag => (Pawn.Dead && reviveIntervalTicks - Corpse.Age <= 0) && !reviveTried;
        public Pawn Pawn => (Pawn) this.parent;
        public Corpse Corpse => Pawn.Corpse;
        public List<Hediff> PawnHediffs => Pawn.health.hediffSet.hediffs.FindAll(x=> x.def.hediffClass == typeof(Hediff_Injury) || (x.def.hediffClass == typeof(Hediff_MissingPart) && !Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)) || x.def.isBad && x.def.hediffClass != typeof(Hediff_AddedPart) && x.def.hediffClass != typeof(Hediff_Implant));
        public List<Hediff> HealableHediffs => PawnHediffs.FindAll(x => !unhealableHediffs.Contains(x) && x.def.hediffClass==typeof(Hediff_Injury));
        public List<Hediff> UnhealableHediffs => unhealableHediffs;
        public bool HasHealableWounds => !HealableHediffs.NullOrEmpty();
        public bool HasRegeneragtingLimbs => Pawn.health.hediffSet.hediffs.Any(x => x.def == AdeptusHediffDefOf.OG_Regenerating);
        public Map Map => Pawn.Dead ? Pawn.Corpse.Map : Pawn.Map;
        public IntVec3 Pos => Pawn.Dead ? Pawn.Corpse.Position : Pawn.Position;
        public bool Necrodermis => (!Pawn.Dead && this.ticksSinceHeal > this.HealIntervalTicks) && Props.Necrodermis && !HealableHediffs.NullOrEmpty();
        public bool Reanimation => (Pawn.Dead && this.Corpse.Age > this.reviveIntervalTicks) && Props.ReanimationProtocol && !reviveTried;
        public bool Phasic => (Pawn.Dead && reviveTried) && Props.PhaseOut && !phaseTried && !Map.mapPawns.AllPawns.Any(x=> (x.def == AdeptusThingDefOf.OG_Necron_TombSpyder || (x.def == AdeptusThingDefOf.OG_Necron_Lord && x.health.hediffSet.HasHediff(AdeptusHediffDefOf.OG_Necron_Upgrade_RessurectionOrb))) && x.Position.InHorDistOf(Pos, 20f));

        public BodyPartRecord NecrodermisRegulator => Pawn.RaceProps.body.AllParts.Find(x => x.def == AdeptusBodyPartDefOf.OG_Necron_NecrodermisRegulator);
        public BodyPartRecord ReanimationMatrix => Pawn.RaceProps.body.AllParts.Find(x => x.def == AdeptusBodyPartDefOf.OG_Necron_ReanimationMatrix);
        public BodyPartRecord PhasicCapacitor => Pawn.RaceProps.body.AllParts.Find(x => x.def == AdeptusBodyPartDefOf.OG_Necron_PhasicCapacitor);

        public float ReanimateChance => Props.ReanimationProtocolChance * (Pawn.health.hediffSet.PartIsMissing(ReanimationMatrix) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(Pawn.health.hediffSet, ReanimationMatrix, false, null));
        public float PhaseChance => Props.PhaseOutChance * (Pawn.health.hediffSet.PartIsMissing(PhasicCapacitor) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(Pawn.health.hediffSet, PhasicCapacitor, false, null));
        public float RegenChance => Props.NecrodermisChance * (Pawn.health.hediffSet.PartIsMissing(NecrodermisRegulator) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(Pawn.health.hediffSet, NecrodermisRegulator, false, null));

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                if (Pawn.equipment!=null)
                {
                    if (Pawn.equipment.Primary!=null)
                    {
                        originalWeapon = Pawn.equipment.Primary;
                        //    Log.Message(string.Format("{0} spawned with {1}", pawn.Label, originalWeapon.Label));
                        /*
                        if (AdeptusIntergrationUtil.enabled_rooloDualWield)
                        {
                            if (pawn.equipment.AdMechTryGetOffHandEquipment(out ThingWithComps thing))
                            {
                                secondryWeapon = thing;
                            }
                        }
                        */
                    }
                }
            }
        }

        public override void CompTickRare()
        {
            if (phaseTried)
            {
                return;
            }
            if (base.parent != null && (base.parent.Spawned || Pawn.Dead) && ReviveFlag && AMAMod.settings.AllowNecronWellBeBack)
            {
                if (base.parent is Pawn && Pawn.kindDef != AdeptusPawnKindDefOf.OG_Necron_Scarab_Swarm)
                {
                    if (Reanimation)
                    {
                        Dead(Pawn);
                    }
                    if (Necrodermis)
                    {
                        Rand.PushState();
                        if (Rand.Chance(RegenChance))
                        {
                            if (Pawn.Downed)
                            {
                                Downed();
                            }
                            else
                            {
                                TryHeal();
                            }
                        }
                        Rand.PopState();
                    }
                    if (Phasic)
                    {
                        TryPhase();
                    }
                    else
                    {

                    }
                }
            }
            base.CompTickRare();
        }

        public void Downed()
        {
            if (base.parent!=null && base.parent is Pawn pawn)
            {
                if (pawn.Downed)
                {
                    downed = true;
                    if (originalWeapon != null)
                    {
                        if (originalWeapon.Spawned)
                        {
                            originalWeapon.DeSpawn();
                        }
                        pawn.inventory.innerContainer.TryAddOrTransfer(originalWeapon);
                    }
                    if (secondryWeapon != null)
                    {
                        if (secondryWeapon.Spawned)
                        {
                            secondryWeapon.DeSpawn();
                        }
                        pawn.inventory.innerContainer.TryAddOrTransfer(secondryWeapon);
                    }
                    if (healableHediffs.NullOrEmpty() && !phaseTried)
                    {
                        if (Phasic)
                        {
                            TryPhase();
                        }
                    }
                }
                if (downed && !pawn.Downed)
                {
                }
            }
        }

        public void TryPhase()
        {
            Rand.PushState();
            if (Rand.Chance(PhaseChance))
            {
                if (Pawn.Corpse.Spawned)
                {
                    ThrowNecronGlow(Pos.ToVector3(), Map, 0.5f);
                    Pawn.Corpse.DeSpawn();
                }
                FleckMaker.Static(Pos, Map, FleckDefOf.PsycastSkipInnerExit, 0.5f);
            }
            else
            {
                phaseTried = true;
            }
            Rand.PopState();
        }

        public void Dead(Pawn pawn)
        {
            if (pawn!=null && pawn.health is Pawn_HealthTracker healthTracker)
            {
            //    bool healable = HasHealableWounds || HasRegeneragtingLimbs;
                if (logging == true) Log.Message(string.Format("Dead {0}, healable: {1}, HasRegeneragtingLimbs: {2}, SummaryHealthPercent: {3}, LethalDamageThreshold: {4}, InPainShock: {5}, State: {6}, reviveTried {7}, reviveFlag {8}, HealableHediffs {9}, UnhealableHediffs {10}", pawn.Label, HasHealableWounds, HasRegeneragtingLimbs, healthTracker.summaryHealth.SummaryHealthPercent, healthTracker.LethalDamageThreshold, healthTracker.InPainShock, healthTracker.State, reviveTried, ReviveFlag, HealableHediffs.Count, UnhealableHediffs.Count));
                if (ReviveFlag)
                {
                    /*
                    if (healable)
                    {
                        foreach (Hediff item in HealableHediffs)
                        {
                            TryHeal();
                        }
                    }
                    */
                    TryRevive();
                }
            }
        }

        public void TryRevive(bool ForcedRevive = false)
        {
            reviveIntervalTicks = -1;
            reviveTried = true;
            Rand.PushState();
            if (Rand.Chance(ReanimateChance) || ForcedRevive)
            {
                List<Hediff> hediffs = unhealableHediffs;
                ResurrectionUtility.Resurrect(Pawn);
                if (originalWeapon==null && Pawn.kindDef.weaponTags.Count>0)
                {
                    ThingDef thingDef = ThingDef.Named(Pawn.kindDef.weaponTags[0]);
                    Thing thing2 = GenClosest.ClosestThingReachable(Pawn.Position, Pawn.Map, ThingRequest.ForDef(thingDef), PathEndMode.InteractionCell, TraverseParms.For(Pawn, Danger.Deadly, TraverseMode.ByPawn, false));
                    this.originalWeapon = (ThingWithComps)thing2;
                }
                if (originalWeapon != null)
                {
                    ThingWithComps thing = originalWeapon;
                    if (thing.Spawned)
                    {
                        thing.DeSpawn();
                    }
                    if (Pawn.inventory.innerContainer.Contains(thing))
                    {
                        Pawn.inventory.innerContainer.Remove(thing);
                    }
                    Pawn.equipment.AddEquipment(thing);
                }
                if (secondryWeapon != null)
                {
                    ThingWithComps thing = secondryWeapon;
                    if (thing.Spawned)
                    {
                        thing.DeSpawn();
                    }
                    if (Pawn.inventory.innerContainer.Contains(thing))
                    {
                        Pawn.inventory.innerContainer.Remove(thing);
                    }
                //    pawn.equipment.AdMechAddOffHandEquipment(thing);
                }
                if (!ForcedRevive)
                {
                //    bool revives = true;
                    foreach (Hediff item in hediffs)
                    {
                        if (!Pawn.health.hediffSet.PartIsMissing(item.Part))
                        {
                            if (Pawn.health.WouldDieAfterAddingHediff(item))
                            {
                            //    revives = false;
                            }
                            if (Pawn.health.WouldBeDownedAfterAddingHediff(item))
                            {
                            //    revives = false;
                            }
                            Pawn.health.AddHediff(item);
                        }
                    }
                }
                ThrowNecronGlow(Pos.ToVector3(), Map, 5f);
                FleckMaker.Static(Pos, Map, FleckDefOf.ExplosionFlash, 3f);
                //    log.message(string.Format("{0} revive {1}",pawn, str));
            }
            else
            {

                //    log.message(string.Format("{0} revive {1}", pawn, str));
            }
            Rand.PopState();
        }

        public void TryHeal(bool Forced = false)
        {

            bool flag2 = HealableHediffs.Any(x => !Pawn.health.hediffSet.PartIsMissing(x.Part) && x is Hediff_Injury);
            bool flag3 = HealableHediffs.Any(x => Pawn.health.hediffSet.PartIsMissing(x.Part));
            bool flag4 = HealableHediffs.Any(x => x.def == AdeptusHediffDefOf.OG_Regenerating);
            Rand.PushState();
            float num = Rand.RangeInclusive(1, 100);
            Rand.PopState();
            Hediff hediff;
            if (flag2)
            {
                if (logging == true) Log.Message(string.Format("flag2"));
                List<Hediff_Injury> list = new List<Hediff_Injury>();
                Pawn.health.hediffSet.GetHediffs<Hediff_Injury>(ref list, x => HediffUtility.CanHealNaturally(x) && (HealableHediffs.Contains(x) || Forced));
                hediff = GenCollection.RandomElement<Hediff_Injury>(list);
                num = num * Pawn.HealthScale * 0.1f;
                hediff.Heal(num);
                string text = string.Format("flag2 the {1} on {0}'s {2} healed by {3}.", Pawn.LabelCap, hediff.Label, hediff.Part.customLabel, num);
                if (hediff.Severity == 0f)
                {
                    text += " and was removed";
                    Pawn.health.RemoveHediff(hediff);
                }
                if (logging == true) Log.Message(string.Format(text));
            }
            else if (flag4)
            {
                if (logging == true) Log.Message(string.Format("flag4"));
#pragma warning disable CS0436 // Type conflicts with imported type
                List<Hediff_RegeneratingPart> list = new List<Hediff_RegeneratingPart>();
                Pawn.health.hediffSet.GetHediffs<Hediff_RegeneratingPart>(ref list);
                hediff = GenCollection.RandomElement<Hediff_RegeneratingPart>(list);
#pragma warning restore CS0436 // Type conflicts with imported type
                num = num * Pawn.HealthScale * 0.001f;
                hediff.Heal(num);
                string text = string.Format("flag5 the {1} on {0}'s {2} regenerated by {3}.", Pawn.LabelCap, hediff.Label, hediff.Part.customLabel, num);
                if (hediff.Severity == 0f)
                {
                    text += " and was removed";
                    Pawn.health.RemoveHediff(hediff);
                }
                if (logging == true) Log.Message(string.Format(text));
            }
            else if (flag3)
            {
                if (logging == true) Log.Message(string.Format("flag3"));
                bool flag3B = Pawn.health.hediffSet.hediffs.Any(x => Pawn.health.hediffSet.PartIsMissing(x.Part) && HealableHediffs.Contains(x));
                if (flag3B)
                {
                    if (logging == true) Log.Message(string.Format("flag3B"));
                    TryRegrowBodyparts();
                }
                else
                {
                    if (logging == true) Log.Message(string.Format("flag3 {0}'s hediff_Injury flag3B: {1}", Pawn.Label, flag3B));
                }
            }
            else
            {
                hediff = null;
            }
        }

        public void TryRegrowBodyparts(bool Forced = false)
        {
            using IEnumerator<BodyPartRecord> enumerator = this.Pawn.GetFirstMatchingBodyparts(this.Pawn.RaceProps.body.corePart, HediffDefOf.MissingBodyPart, AdeptusHediffDefOf.OG_Regenerating, (Hediff hediff) => hediff is Hediff_AddedPart).GetEnumerator();
            while (enumerator.MoveNext())
            {
                BodyPartRecord part = enumerator.Current;
                bool any = this.Pawn.health.hediffSet.hediffs.Any((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart && (HealableHediffs.Contains(hediff) || Forced));
                if (any)
                {
                    Hediff hediff2 = this.Pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart && (HealableHediffs.Contains(hediff) || Forced));
                    bool flag = hediff2 != null;
                    if (flag)
                    {
                        this.Pawn.health.RemoveHediff(hediff2);
                        this.Pawn.health.AddHediff(AdeptusHediffDefOf.OG_Regenerating, part, null, null);
                        this.Pawn.health.hediffSet.DirtyCache();
                    }
                }

            }
        }

        public void TryRemoveHediff(bool Forced = false)
        {
            bool any = this.Pawn.health.hediffSet.hediffs.Any((Hediff hediff) => hediff.def != HediffDefOf.MissingBodyPart && (Forced || HealableHediffs.Contains(hediff)));
            if (any)
            {
                Hediff hediff2 = this.Pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.def != HediffDefOf.MissingBodyPart && (Forced || HealableHediffs.Contains(hediff)));
                bool flag = hediff2 != null;
                if (flag)
                {
                    this.Pawn.health.RemoveHediff(hediff2);
                    this.Pawn.health.hediffSet.DirtyCache();
                }
            }
        }

        // Token: 0x060026C2 RID: 9922 RVA: 0x00126668 File Offset: 0x00124A68
        public static void ThrowNecronGlow(Vector3 loc, Map map, float size)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamedSilentFail("Mote_OG_NecronLightningGlow"), null);
            Rand.PushState();
            moteThrown.Scale = Rand.Range(4f, 6f) * size;
            moteThrown.rotationRate = Rand.Range(-3f, 3f);
            moteThrown.exactPosition = loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
            moteThrown.SetVelocity((float)Rand.Range(0, 360), 1.2f);
            Rand.PopState();
            GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
        }


        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
        //    Log.Message("Comp_Necron CompGetGizmosExtra");
            /*
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            */
            bool selected = (Find.Selector.SingleSelectedThing == Pawn || Find.Selector.SingleSelectedThing == Pawn.Corpse) && Prefs.DevMode && DebugSettings.godMode;
            bool missing =  Pawn.health.hediffSet.hediffs.Any(x => x.def.hediffClass == typeof(Hediff_MissingPart) && x.def.isBad);
            if (selected)
            {
                if (Pawn.Dead)
                {
                    Command_Action command_Action = new Command_Action
                    {
                        defaultLabel = "Activate Reanimation Protocols",
                        defaultDesc = parent.def.label,
                        //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                        icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true),
                        action = DoTryRevive
                    };
                    yield return command_Action;
                }
                else
                if (!HealableHediffs.NullOrEmpty())
                {
                    Command_Action command_Action = new Command_Action
                    {
                        defaultLabel = "Activate Necrodermis repair",
                        defaultDesc = parent.def.label,
                        //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                        icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true),
                        action = DoTryHeal
                    };
                    yield return command_Action;
                }
                if (missing)
                {
                    Command_Action command_Action = new Command_Action
                    {
                        defaultLabel = "Activate Necrodermis regeneration",
                        defaultDesc = parent.def.label,
                        //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                        icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true),
                        action = DoTryRegrowBodyparts
                    };
                    yield return command_Action;
                }
            }
            yield break;
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            if (dinfo.Def == null)
            {
                absorbed = false;
                return;
            }
            if (base.parent is Pawn && dinfo.Def.defName.Contains("InEyes"))
            {
                absorbed = true;
            }
            /*
            else if (base.parent is Pawn pawn && dinfo.ArmorPenetrationInt < pawn.kindDef.race.statBases.GetStatValueFromList(dinfo.Def.armorCategory.armorRatingStat, 0f))
            {
                absorbed = Rand.Chance(0.5f) ? true : false;
            }
            */
            else
            {
                absorbed = false;
            }
            base.PostPreApplyDamage(dinfo, out absorbed);

        }

        public override void CompTick()
        {
            base.CompTick();
            this.ticksSinceHeal++;
        }

        public void DoTryRevive()
        {
            if (Pawn.Dead)
            {
                TryRevive(true);
            }
        }
        public void DoTryHeal()
        {
            if (Pawn.Dead)
            {
                TryHeal(true);
            }
        }
        public void DoTryRegrowBodyparts()
        {
            if (Pawn.Dead)
            {
                TryRegrowBodyparts(true);
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0);
            Scribe_Values.Look<int>(ref this.reviveIntervalTicks, "reviveIntervalTicks", 0);
            Scribe_Values.Look<bool>(ref this.reviveTried, "reviveTried", false);
            Scribe_Values.Look<bool>(ref this.phaseTried, "phaseTried", false);
            Scribe_References.Look<ThingWithComps>(ref this.originalWeapon, "originalWeapon");
            Scribe_References.Look<ThingWithComps>(ref this.secondryWeapon, "secondryWeapon");
            Scribe_Collections.Look(ref this.healableHediffs, "healableHediffs", LookMode.Reference);
            Scribe_Collections.Look(ref this.unhealableHediffs, "unhealableHediffs", LookMode.Reference);
        }

        public ThingWithComps originalWeapon;
        public ThingWithComps secondryWeapon;
        public bool reviveTried = false;
        public bool phaseTried = false;
        public List<Hediff> HDlist = null;
        public List<Hediff> healableHediffs = new List<Hediff>();
        public List<Hediff> unhealableHediffs = new List<Hediff>();
        bool downed;
        bool logging = false;
        public int reviveIntervalTicks;
        public int HealIntervalTicks => Props.NecrodermisInterval.SecondsToTicks();
        public int ticksSinceHeal;

    }
}