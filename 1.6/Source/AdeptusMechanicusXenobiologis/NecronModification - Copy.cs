using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class NecronModification
    {
        static NecronModification()
        {

            DefDatabase<ThingDef>.AllDefsListForReading.ForEach(action: td =>
            {
                if (td.defName.StartsWith("Corpse_Necron_") && td.IsCorpse)
                {
                    td.comps.Add(new CompProperties_Necron());
                }
            });
        }
    }

    // Token: 0x0200024E RID: 590
    public class CompProperties_Necron : CompProperties
    {
        // Token: 0x06000AB1 RID: 2737 RVA: 0x0005598C File Offset: 0x00053D8C
        public CompProperties_Necron()
        {
            this.compClass = typeof(Comp_Necron);
        }

        // Token: 0x040004AB RID: 1195
        public float overlightRadius;

        // Token: 0x040004AC RID: 1196
        public float glowRadius = 14f;

        // Token: 0x040004AD RID: 1197
        public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
        public DamageDef fatalDamageDef;
        public bool reviveFlag = true;
        public bool repairFlag = false;
    }
    // Token: 0x02000C66 RID: 3174
    public class Comp_Necron : ThingComp
    {
        // Token: 0x17000AD2 RID: 2770
        // (get) Token: 0x060045EE RID: 17902 RVA: 0x0020D7F1 File Offset: 0x0020BBF1
        public CompProperties_Necron Props
        {
            get
            {
                return (CompProperties_Necron)this.props;
            }
        }

        public DamageInfo dinfo;
        public DamageDef damageDef;

        public DamageDef fatalDamageDef = null;
        public bool reviveFlag = false;
        public Pawn pawn = null;
        public Corpse pawnCorpse = null;
        public bool reviveTried = false;
        public List<Hediff> HDlist;
        

        public override void CompTick()
        {
            base.CompTick();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (base.parent != null)
            {
                Log.Message(string.Format("{0} is base.parent", base.parent.Label));
                if (base.parent is Pawn)
                {
                    Log.Message(string.Format("{0} is a pawn", base.parent.Label));
                    this.pawn = (Pawn)base.parent;
                    Log.Message(string.Format("{0} is pawn", this.pawn.Label));
                    this.pawnCorpse = pawn.Corpse;
                    Log.Message(string.Format("{0} is pawnCorpse", this.pawnCorpse.Label));
                }
                else if (base.parent is Corpse)
                {
                    Log.Message(string.Format("{0} is a Corpse", base.parent.Label));
                    this.pawnCorpse = (Corpse)base.parent;
                    Log.Message(string.Format("{0} is pawnCorpse", this.pawnCorpse.Label));
                    this.pawn = pawnCorpse.InnerPawn;
                    Log.Message(string.Format("{0} is pawn", this.pawn.Label));
                }
                Log.Message(string.Format("checking for fatalDamageDef", base.parent.Label));
                if (fatalDamageDef!=null)
                {
                    Log.Message(string.Format("fatalDamageDef is {0}", fatalDamageDef.label));
                    if (fatalDamageDef?.defaultArmorPenetration == null)
                    {
                        fatalDamageDef.defaultArmorPenetration = 0f;
                        Log.Message(string.Format("defaultArmorPenetration for fatalDamageDef is {0}", fatalDamageDef.defaultArmorPenetration));
                    }
                    if (fatalDamageDef.defName.StartsWith("Power") || fatalDamageDef.defName.StartsWith("Force") || fatalDamageDef.defaultArmorPenetration >= 1f)
                    {
                        Props.reviveFlag = false;
                        Log.Message(string.Format("Props.reviveFlag is {0}", Props.reviveFlag));
                    }
                    else
                    {
                        Props.reviveFlag = true;
                        Log.Message(string.Format("Props.reviveFlag is {0}", Props.reviveFlag));
                    }
                }
                else
                {
                    if (base.parent is Pawn)
                    {
                        HDlist = pawn.health.hediffSet.hediffs;

                    }
                    else if (base.parent is Corpse)
                    {
                        HDlist = pawnCorpse.InnerPawn.health.hediffSet.hediffs;
                        if (HDlist?.Count > 0)
                        {
                            Log.Message(string.Format("{0} is a Corpse with {1} hediffs in HDlist first: {2} last: {3}", base.parent.Label, HDlist.Count, HDlist[0].source.label, HDlist[HDlist.Count - 1].source.label));
                            float age = 0;
                            foreach (var hd in HDlist)
                            {
                                if (hd.ageTicks<age||age==0)
                                {
                                    Log.Message(string.Format("{0} is {1} ticks old", hd.Label, hd.ageTicks));
                                    age = hd.ageTicks;
                                    Log.Message(string.Format("{0} WeaponUsingProjectiles {1}", hd.Label, hd.source.IsWeaponUsingProjectiles));
                                    Log.Message(string.Format("{0} MeleeWeapon {1}", hd.Label, hd.source.IsMeleeWeapon));
                                    if (hd.source.IsWeaponUsingProjectiles)
                                    {
                                        Log.Message(string.Format("{0} WeaponUsingProjectiles is {1} : {2} on {3}", hd.source.label, hd.source.IsWeaponUsingProjectiles, hd.source.Verbs[0].defaultProjectile.label, hd.Part.Label));
                                        fatalDamageDef = hd.source.Verbs[0].defaultProjectile.projectile.damageDef;
                                        Log.Message(string.Format("fatalDamageDef is {0}", fatalDamageDef));
                                    }
                                    else if (hd.source.IsMeleeWeapon)
                                    {
                                        Log.Message(string.Format("hd.source.IsMeleeWeapon is {0}", hd.source.IsMeleeWeapon));
                                        foreach (var v in hd.source.Verbs)
                                        {
                                            if (v.IsMeleeAttack && v.meleeDamageDef.hediff == hd.def)
                                            {
                                                fatalDamageDef = v.meleeDamageDef;
                                                Log.Message(string.Format("fatalDamageDef is {0}", fatalDamageDef));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fatalDamageDef = null;
                                        Log.Message(string.Format("fatalDamageDef is null", fatalDamageDef));
                                    }
                                }
                            }

                            Log.Message(string.Format("fatalDamageDef is {0}", fatalDamageDef));
                            if (fatalDamageDef?.defaultArmorPenetration == null)
                            {
                                fatalDamageDef.defaultArmorPenetration = 0f;
                                Log.Message(string.Format("defaultArmorPenetration for fatalDamageDef is {0}", fatalDamageDef.defaultArmorPenetration));
                            }
                            if (fatalDamageDef.defName.StartsWith("Power") || fatalDamageDef.defName.StartsWith("Force") || fatalDamageDef.defaultArmorPenetration >= 1f)
                            {
                                Props.reviveFlag = false;
                                Log.Message(string.Format("Props.reviveFlag is {0}", Props.reviveFlag));
                            }
                            else
                            {
                                Props.reviveFlag = true;
                                Log.Message(string.Format("Props.reviveFlag is {0}", Props.reviveFlag));
                            }
                        }
                    }
                }
            }
        }
/*
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            if (base.parent != null)
            {
                Log.Message(string.Format("{0} took damage", base.parent.Label));
                if (base.parent is Pawn)
                {
                    Log.Message(string.Format("{0} is a pawn", base.parent.Label));
                    this.pawn = (Pawn)base.parent;
                    this.pawnCorpse = pawn.Corpse;
                    if (this.pawn.Dead)
                    {
                        Log.Message(string.Format("{0} is dead", base.parent.Label)); 

                        this.fatalDamageDef = dinfo.Def;
                        Props.fatalDamageDef = fatalDamageDef;
                        if (fatalDamageDef?.defaultArmorPenetration!=null)
                        {
                            fatalDamageDef.defaultArmorPenetration = 0f;
                        }
                        if (fatalDamageDef.defName.StartsWith("Power") || fatalDamageDef.defaultArmorPenetration >= 1f)
                        {
                            Props.reviveFlag = false;
                        }
                        else
                        {
                            Props.reviveFlag = true;
                        }
                    }
                }
                else if (base.parent is Corpse)
                {
                    this.pawnCorpse = (Corpse)base.parent;
                    this.pawn = pawnCorpse.InnerPawn;

                    if (this.pawnCorpse != null && this.pawn != null)
                    {
                        fatalDamageDef = dinfo.Def;
                        Props.fatalDamageDef = fatalDamageDef;
                        if (fatalDamageDef.defName.StartsWith("Power") || fatalDamageDef.defaultArmorPenetration >= 1f)
                        {
                            Props.reviveFlag = false;
                        }
                        else
                        {
                            Props.reviveFlag = true;
                        }
                    }
                }
            }
        }
*/
        public override void CompTickRare()
        {
            base.CompTickRare();
            if (base.parent is Corpse && base.parent != null && Props.reviveFlag)
            {
                pawnCorpse = (Corpse)base.parent;
                pawn = pawnCorpse.InnerPawn;
                if (pawn.Dead && Props.reviveFlag && pawn.Dead && !this.reviveTried)
                {
                    if (Rand.Chance(pawn.BodySize))
                    {
                        ResurrectionUtility.Resurrect(pawn);
                        MoteMaker.ThrowLightningGlow(base.parent.TrueCenter(), base.parent.Map, 13f);
                       // MoteMaker.MakeStaticMote(base.parent.TrueCenter(), base.parent.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
                        //   pawn.jobs.StopAll();
                        //   pawn.mindState.

                    }
                    this.reviveTried = true;
                }
            }
            else if (base.parent is Pawn && this.pawn != null && pawn.Dead == false && Props.reviveFlag && !pawn.Dead)
            {
                this.pawn = (Pawn)base.parent;
                this.pawnCorpse = pawn.Corpse;

                CompSelfRepair();
            }

        }

        public void CompSelfRepair()
        {
            Hediff item = pawn.health.hediffSet.hediffs.RandomElement();
            // damageDef = dinfo.Def;
            if (!item.sourceHediffDef.defName.StartsWith("Power") || !item.sourceHediffDef.defName.StartsWith("Force") || ((item.source.IsWeaponUsingProjectiles || item.source.IsRangedWeapon) && item.source.projectile.damageDef.defaultArmorPenetration <= 1))
            {
                //item.source.Verbs[0].;
                if (Rand.Chance(pawn.BodySize) &&item.def.isBad)
                {
                    //item.def;
                        item.Heal(item.Severity);
                        MoteMaker.ThrowLightningGlow(base.parent.TrueCenter(), base.parent.Map, 3f);
                }
            }

        }
    }
}