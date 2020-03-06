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

        public bool reviveFlag => (pawn.Dead ? reviveIntervalTicks - corpse.Age <= 0 : false) && !reviveTried;
        public Pawn pawn => (Pawn) this.parent;
        public Corpse corpse => pawn.Corpse;
        public List<Hediff> PawnHediffs => pawn.health.hediffSet.hediffs.FindAll(x=> x.def.hediffClass == typeof(Hediff_Injury) || (x.def.hediffClass == typeof(Hediff_MissingPart) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)) || x.def.isBad && x.def.hediffClass != typeof(Hediff_AddedPart) && x.def.hediffClass != typeof(Hediff_Implant));
        public List<Hediff> HealableHediffs => PawnHediffs.FindAll(x => !unhealableHediffs.Contains(x) && x.def.hediffClass==typeof(Hediff_Injury));
        public List<Hediff> UnhealableHediffs => unhealableHediffs;
        public bool HasHealableWounds => !HealableHediffs.NullOrEmpty();
        public bool HasRegeneragtingLimbs => pawn.health.hediffSet.hediffs.Any(x => x.def == AMXenoBiologisHediffDefOf.OG_Regenerating);
        public Map map => pawn.Dead ? pawn.Corpse.Map : pawn.Map;
        public IntVec3 pos => pawn.Dead ? pawn.Corpse.Position : pawn.Position;
        bool Necrodermis => (pawn.Dead ? false : this.ticksSinceHeal > this.healIntervalTicks) && Props.Necrodermis && !HealableHediffs.NullOrEmpty();
        bool Reanimation => (pawn.Dead ? this.corpse.Age > this.reviveIntervalTicks : false) && Props.ReanimationProtocol && !reviveTried;
        bool Phasic => (pawn.Dead ? reviveTried : false) && Props.PhaseOut && !phaseTried && !map.mapPawns.AllPawns.Any(x=> (x.def == OGNecronDefOf.Necron_TombSpyder || (x.def == OGNecronDefOf.Necron_Lord && x.health.hediffSet.HasHediff(OGNecronDefOf.OG_Necron_Upgrade_RessurectionOrb))) && x.Position.InHorDistOf(pos, 20f));

        BodyPartRecord NecrodermisRegulator => pawn.RaceProps.body.AllParts.Find(x => x.def == OGNecronDefOf.OG_Necron_NecrodermisRegulator);
        BodyPartRecord ReanimationMatrix => pawn.RaceProps.body.AllParts.Find(x => x.def == OGNecronDefOf.OG_Necron_ReanimationMatrix);
        BodyPartRecord PhasicCapacitor => pawn.RaceProps.body.AllParts.Find(x => x.def == OGNecronDefOf.OG_Necron_PhasicCapacitor);
        
        float reanimateChance => Props.ReanimationProtocolChance * (pawn.health.hediffSet.PartIsMissing(ReanimationMatrix) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, ReanimationMatrix, false, null));
        float phaseChance => Props.PhaseOutChance * (pawn.health.hediffSet.PartIsMissing(PhasicCapacitor) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, PhasicCapacitor, false, null));
        float regenChance => Props.NecrodermisChance * (pawn.health.hediffSet.PartIsMissing(NecrodermisRegulator) ? 0f : PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, NecrodermisRegulator, false, null));

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                if (pawn.equipment!=null)
                {
                    if (pawn.equipment.Primary!=null)
                    {
                        originalWeapon = pawn.equipment.Primary;
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
            if (base.parent != null && (base.parent.Spawned || pawn.Dead) && reviveFlag && SettingsHelper.latest.AllowNecronWellBeBack)
            {
                if (base.parent is Pawn && pawn.kindDef != OGNecronDefOf.OG_Necron_Scarab_Swarm)
                {
                    if (Reanimation)
                    {
                        Dead(pawn);
                    }
                    if (Necrodermis)
                    {
                        if (Rand.Chance(regenChance))
                        {
                            if (pawn.Downed)
                            {
                                Downed();
                            }
                            else
                            {
                                TryHeal();
                            }
                        }
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
            if (Rand.Chance(phaseChance))
            {
                if (pawn.Corpse.Spawned)
                {
                    ThrowNecronGlow(pos.ToVector3(), map, 0.5f);
                    pawn.Corpse.DeSpawn();
                }
                MoteMaker.MakeStaticMote(pos, map, ThingDefOf.Mote_ExplosionFlash, 0.5f);
            }
            else
            {
                phaseTried = true;
            }
        }

        public void Dead(Pawn pawn)
        {
            if (pawn!=null && pawn.health is Pawn_HealthTracker healthTracker)
            {
                bool healable = HasHealableWounds || HasRegeneragtingLimbs;
                if (logging == true) Log.Message(string.Format("Dead {0}, healable: {1}, HasRegeneragtingLimbs: {2}, SummaryHealthPercent: {3}, LethalDamageThreshold: {4}, InPainShock: {5}, State: {6}, reviveTried {7}, reviveFlag {8}, HealableHediffs {9}, UnhealableHediffs {10}", pawn.Label, HasHealableWounds, HasRegeneragtingLimbs, healthTracker.summaryHealth.SummaryHealthPercent, healthTracker.LethalDamageThreshold, healthTracker.InPainShock, healthTracker.State, reviveTried, reviveFlag, HealableHediffs.Count, UnhealableHediffs.Count));
                if (reviveFlag)
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
            string str = "sucessful";
            reviveIntervalTicks = -1;
            reviveTried = true;
            if (Rand.Chance(reanimateChance) || ForcedRevive)
            {
                List<Hediff> hediffs = unhealableHediffs;
                ResurrectionUtility.Resurrect(pawn);
                if (originalWeapon==null && pawn.kindDef.weaponTags.Count>0)
                {
                    ThingDef thingDef = ThingDef.Named(pawn.kindDef.weaponTags[0]);
                    Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(thingDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false));
                    this.originalWeapon = (ThingWithComps)thing2;
                }
                if (originalWeapon != null)
                {
                    ThingWithComps thing = originalWeapon;
                    if (thing.Spawned)
                    {
                        thing.DeSpawn();
                    }
                    if (pawn.inventory.innerContainer.Contains(thing))
                    {
                        pawn.inventory.innerContainer.Remove(thing);
                    }
                    pawn.equipment.AddEquipment(thing);
                }
                if (secondryWeapon != null)
                {
                    ThingWithComps thing = secondryWeapon;
                    if (thing.Spawned)
                    {
                        thing.DeSpawn();
                    }
                    if (pawn.inventory.innerContainer.Contains(thing))
                    {
                        pawn.inventory.innerContainer.Remove(thing);
                    }
                //    pawn.equipment.AdMechAddOffHandEquipment(thing);
                }
                if (!ForcedRevive)
                {
                    bool revives = true;
                    foreach (Hediff item in hediffs)
                    {
                        if (!pawn.health.hediffSet.PartIsMissing(item.Part))
                        {
                            if (pawn.health.WouldDieAfterAddingHediff(item))
                            {
                                revives = false;
                                str = "failiure";
                            }
                            if (pawn.health.WouldBeDownedAfterAddingHediff(item))
                            {
                                revives = false;
                                str = "failiure: still downed";
                            }
                            pawn.health.AddHediff(item);
                        }
                    }
                }
                ThrowNecronGlow(pos.ToVector3(), map, 5f);
                MoteMaker.MakeStaticMote(pos, map, ThingDefOf.Mote_ExplosionFlash, 3f);
                Log.Message(string.Format("{0} revive {1}",pawn, str));
            }
            else
            {

                str = "failiure: roll";
                Log.Message(string.Format("{0} revive {1}", pawn, str));
            }
        }

        public void TryHeal(bool Forced = false)
        {

            bool flag2 = HealableHediffs.Any(x => !pawn.health.hediffSet.PartIsMissing(x.Part) && x is Hediff_Injury);
            bool flag3 = HealableHediffs.Any(x => pawn.health.hediffSet.PartIsMissing(x.Part));
            bool flag4 = HealableHediffs.Any(x => x.def == AMXenoBiologisHediffDefOf.OG_Regenerating);
            float num = Rand.RangeInclusive(1, 100);
            Hediff hediff;
            if (flag2)
            {
                if (logging == true) Log.Message(string.Format("flag2"));
                hediff = GenCollection.RandomElement<Hediff_Injury>(from x in pawn.health.hediffSet.GetHediffs<Hediff_Injury>()
                                                                    where HediffUtility.CanHealNaturally(x) && (HealableHediffs.Contains(x) || Forced)
                                                                    select x);
                num = num * pawn.HealthScale * 0.1f;
                hediff.Heal(num);
                string text = string.Format("flag2 the {1} on {0}'s {2} healed by {3}.", pawn.LabelCap, hediff.Label, hediff.Part.customLabel, num);
                if (hediff.Severity == 0f)
                {
                    text = text + " and was removed";
                    pawn.health.RemoveHediff(hediff);
                }
                if (logging == true) Log.Message(string.Format(text));
            }
            else if (flag4)
            {
                if (logging == true) Log.Message(string.Format("flag4"));
                hediff = pawn.health.hediffSet.GetHediffs<Hediff_RegeneratingPart>().RandomElement();
                num = num * pawn.HealthScale * 0.001f;
                hediff.Heal(num);
                string text = string.Format("flag5 the {1} on {0}'s {2} regenerated by {3}.", pawn.LabelCap, hediff.Label, hediff.Part.customLabel, num);
                if (hediff.Severity == 0f)
                {
                    text = text + " and was removed";
                    pawn.health.RemoveHediff(hediff);
                }
                if (logging == true) Log.Message(string.Format(text));
            }
            else if (flag3)
            {
                if (logging == true) Log.Message(string.Format("flag3"));
                bool flag3B = pawn.health.hediffSet.hediffs.Any(x => pawn.health.hediffSet.PartIsMissing(x.Part) && HealableHediffs.Contains(x));
                if (flag3B)
                {
                    if (logging == true) Log.Message(string.Format("flag3B"));
                    TryRegrowBodyparts();
                }
                else
                {
                    if (logging == true) Log.Message(string.Format("flag3 {0}'s hediff_Injury flag3B: {1}", pawn.Label, flag3B));
                }
            }
            else
            {
                hediff = null;
            }
        }

        public void TryRegrowBodyparts(bool Forced = false)
        {
            using (IEnumerator<BodyPartRecord> enumerator = this.pawn.GetFirstMatchingBodyparts(this.pawn.RaceProps.body.corePart, HediffDefOf.MissingBodyPart, AMXenoBiologisHediffDefOf.OG_Regenerating, (Hediff hediff) => hediff is Hediff_AddedPart).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord part = enumerator.Current;
                    bool any = this.pawn.health.hediffSet.hediffs.Any((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart && (HealableHediffs.Contains(hediff) || Forced));
                    if (any)
                    {
                        Hediff hediff2 = this.pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.Part == part && hediff.def == HediffDefOf.MissingBodyPart && (HealableHediffs.Contains(hediff) || Forced));
                        bool flag = hediff2 != null;
                        if (flag)
                        {
                            this.pawn.health.RemoveHediff(hediff2);
                            this.pawn.health.AddHediff(AMXenoBiologisHediffDefOf.OG_Regenerating, part, null, null);
                            this.pawn.health.hediffSet.DirtyCache();
                        }
                    }

                }
            }
        }

        public void TryRemoveHediff(bool Forced = false)
        {
            bool any = this.pawn.health.hediffSet.hediffs.Any((Hediff hediff) => hediff.def != HediffDefOf.MissingBodyPart && (Forced || HealableHediffs.Contains(hediff)));
            if (any)
            {
                Hediff hediff2 = this.pawn.health.hediffSet.hediffs.First((Hediff hediff) => hediff.def != HediffDefOf.MissingBodyPart && (Forced || HealableHediffs.Contains(hediff)));
                bool flag = hediff2 != null;
                if (flag)
                {
                    this.pawn.health.RemoveHediff(hediff2);
                    this.pawn.health.hediffSet.DirtyCache();
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
            moteThrown.Scale = Rand.Range(4f, 6f) * size;
            moteThrown.rotationRate = Rand.Range(-3f, 3f);
            moteThrown.exactPosition = loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
            moteThrown.SetVelocity((float)Rand.Range(0, 360), 1.2f);
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
            bool selected = (Find.Selector.SingleSelectedThing == pawn || Find.Selector.SingleSelectedThing == pawn.Corpse) && Prefs.DevMode && DebugSettings.godMode;
            bool missing =  pawn.health.hediffSet.hediffs.Any(x => x.def.hediffClass == typeof(Hediff_MissingPart) && x.def.isBad);
            if (selected)
            {
                //    Log.Message("Comp_Necron CompGetGizmosExtra selected");
                /*
                yield return new Command_Action
                {
                    action = Detonate,
                    defaultLabel = "WearableExplosives_Detonate_Label".Translate(),
                    defaultDesc = "WearableExplosives_Detonate_Desc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Ui/Commands/CommandButton_BOOM", true),
                    activateSound = SoundDef.Named("Click"),
                    groupKey = num + 1
                };
                */
                if (pawn.Dead)
                {
                    Command_Action command_Action = new Command_Action();
                    command_Action.defaultLabel = "Activate Reanimation Protocols";
                    command_Action.defaultDesc = parent.def.label;
                    //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                    command_Action.icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true);
                    command_Action.action = doTryRevive;
                    yield return command_Action;
                }
                else
                if (!HealableHediffs.NullOrEmpty())
                {
                    Command_Action command_Action = new Command_Action();
                    command_Action.defaultLabel = "Activate Necrodermis repair";
                    command_Action.defaultDesc = parent.def.label;
                    //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                    command_Action.icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true);
                    command_Action.action = doTryHeal;
                    yield return command_Action;
                }
                if (missing)
                {
                    Command_Action command_Action = new Command_Action();
                    command_Action.defaultLabel = "Activate Necrodermis regeneration";
                    command_Action.defaultDesc = parent.def.label;
                    //    command_Action.hotKey = KeyBindingDefOf.Misc2;
                    command_Action.icon = ContentFinder<Texture2D>.Get("Icons/Icon_Necron", true);
                    command_Action.action = doTryRegrowBodyparts;
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

        public void doTryRevive()
        {
            if (pawn.Dead)
            {
                TryRevive(true);
            }
        }
        public void doTryHeal()
        {
            if (pawn.Dead)
            {
                TryHeal(true);
            }
        }
        public void doTryRegrowBodyparts()
        {
            if (pawn.Dead)
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
            Scribe_Collections.Look(ref this.healableHediffs, "healableHediffs");
            Scribe_Collections.Look(ref this.unhealableHediffs, "unhealableHediffs");
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
        public int healIntervalTicks => Props.NecrodermisInterval.SecondsToTicks();
        public int ticksSinceHeal;

    }
}