using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    // Token: 0x0200000A RID: 10
    internal class NecronPawn : Pawn
    {
        // Token: 0x06000035 RID: 53 RVA: 0x0000375C File Offset: 0x0000195C
        public NecronPawn()
        {
            this.Init();
        }

        // Token: 0x06000036 RID: 54 RVA: 0x0000377C File Offset: 0x0000197C
        private void Init()
        {
            this.setZombie = false;
            this.pather = new Pawn_PathFollower(this);
            this.stances = new Pawn_StanceTracker(this);
            this.health = new Pawn_HealthTracker(this);
            this.jobs = new Pawn_JobTracker(this);
            this.filth = new Pawn_FilthTracker(this);
            this.notRaidingAttackRange = UnityEngine.Random.Range(15f, 250f);
        }

        // Token: 0x06000037 RID: 55 RVA: 0x000037E4 File Offset: 0x000019E4
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<bool>(ref this.isRaiding, "isRaiding", true, false);
            Scribe_Values.LookValue<bool>(ref this.wasColonist, "wasColonist", false, false);
            Scribe_Values.LookValue<float>(ref this.notRaidingAttackRange, "notRaidingAttackRange", 100f, false);
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00003834 File Offset: 0x00001A34
        public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            this.health.PreApplyDamage(dinfo, ref absorbed);
            if (!base.Destroyed && (dinfo.Def == DamageDefOf.Cut || dinfo.Def == DamageDefOf.Stab))
            {
                float num = 0f;
                float num2 = 0f;
                if (dinfo.Instigator != null && dinfo.Instigator is Pawn && ((Pawn)dinfo.Instigator).IsColonist)
                {
                    Pawn pawn = dinfo.Instigator as Pawn;
                    if (pawn.skills != null)
                    {
                        SkillRecord skill = pawn.skills.GetSkill(SkillDefOf.Melee);
                        num = (float)(skill.level * 2);
                        num2 = (float)skill.level / 20f * 3f;
                    }
                    if (UnityEngine.Random.Range(0f, 100f) < 20f + num)
                    {
                        dinfo.SetAmount(999);
                        dinfo.SetPart(new BodyPartDamageInfo(this.health.hediffSet.GetBrain(), false, HediffDefOf.Cut));
                        dinfo.Def.Worker.Apply(dinfo, this);
                        return;
                    }
                    dinfo.SetAmount((int)((float)dinfo.Amount * (1f + num2)));
                }
            }
        }

        // Token: 0x06000039 RID: 57 RVA: 0x0000396C File Offset: 0x00001B6C
        public override void Tick()
        {
            try
            {
                if (DebugSettings.noAnimals && base.RaceProps.Animal)
                {
                    this.Destroy(0);
                }
                else if (!base.Downed)
                {
                    if (Find.TickManager.TicksGame % 250 == 0)
                    {
                        this.TickRare();
                    }
                    if (base.Spawned)
                    {
                        this.pather.PatherTick();
                    }
                    base.Drawer.DrawTrackerTick();
                    this.health.HealthTick();
                    this.records.RecordsTick();
                    if (base.Spawned)
                    {
                        this.stances.StanceTrackerTick();
                    }
                    if (base.Spawned)
                    {
                        this.verbTracker.VerbsTick();
                    }
                    if (base.Spawned)
                    {
                        this.natives.NativeVerbsTick();
                    }
                    if (this.equipment != null)
                    {
                        this.equipment.EquipmentTrackerTick();
                    }
                    if (this.apparel != null)
                    {
                        this.apparel.ApparelTrackerTick();
                    }
                    if (base.Spawned)
                    {
                        this.jobs.JobTrackerTick();
                    }
                    if (!base.Dead)
                    {
                        this.carrier.CarryHandsTick();
                    }
                    if (this.skills != null)
                    {
                        this.skills.SkillsTick();
                    }
                    if (this.inventory != null)
                    {
                        this.inventory.InventoryTrackerTick();
                    }
                }
                if (this.needs != null && this.needs.food != null && this.needs.food.CurLevel <= 0.95f)
                {
                    this.needs.food.CurLevel = 1f;
                }
                if (this.needs != null && this.needs.joy != null && this.needs.joy.CurLevel <= 0.95f)
                {
                    this.needs.joy.CurLevel = 1f;
                }
                if (this.needs != null && this.needs.beauty != null && this.needs.beauty.CurLevel <= 0.95f)
                {
                    this.needs.beauty.CurLevel = 1f;
                }
                if (this.needs != null && this.needs.comfort != null && this.needs.comfort.CurLevel <= 0.95f)
                {
                    this.needs.comfort.CurLevel = 1f;
                }
                if (this.needs != null && this.needs.rest != null && this.needs.rest.CurLevel <= 0.95f)
                {
                    this.needs.rest.CurLevel = 1f;
                }
                if (this.needs != null && this.needs.mood != null && this.needs.mood.CurLevel <= 0.45f)
                {
                    this.needs.mood.CurLevel = 0.5f;
                }
                if (!this.setZombie)
                {
                    this.mindState.mentalStateHandler.neverFleeIndividual = true;
                    this.setZombie = NecronMod_Utility.Zombify(this);
                    NecronMod_Utility.SetZombieName(this);
                }
                if (base.Downed || this.health.Downed || this.health.InPainShock)
                {
                    DamageInfo damageInfo;
                    damageInfo..ctor(DamageDefOf.Blunt, 9999, this, null, null);
                    damageInfo.SetPart(new BodyPartDamageInfo(this.health.hediffSet.GetBrain(), false, HediffDefOf.Cut));
                    base.TakeDamage(damageInfo);
                }
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x04000016 RID: 22
        private bool setZombie;

        // Token: 0x04000017 RID: 23
        public bool isRaiding = true;

        // Token: 0x04000018 RID: 24
        public bool wasRaiding;

        // Token: 0x04000019 RID: 25
        public bool wasColonist;

        // Token: 0x0400001A RID: 26
        public float notRaidingAttackRange = 15f;
    }
}
