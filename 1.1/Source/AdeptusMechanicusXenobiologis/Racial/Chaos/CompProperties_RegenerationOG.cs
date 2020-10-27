using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace AdeptusMechanicus
{
    public class CompProperties_RegenerationOG : CompProperties
    {
        public CompProperties_RegenerationOG()
        {
            this.compClass = typeof(CompRegenerationOG);
        }
        public List<DamageDef> BypassingDamageDefs = new List<DamageDef>();
        public string ShieldTexPath;
        public bool CanRevive = false;
        public bool CanRestoreParts = false;
        public bool CanStopBleeding = false;
        public bool CanReplaceBlood = false;
        public bool ArmourReliant = false;
        public float BaseChance = 0.25f;

    }

    // Token: 0x02000002 RID: 2
    public class CompRegenerationOG : ThingComp
    {
        public CompProperties_RegenerationOG Props => (CompProperties_RegenerationOG)props;
        public Pawn pawn => base.parent as Pawn;
        public List<DamageDef> BypassingDamageDefs => Props.BypassingDamageDefs as List<DamageDef>;
        public bool CanRevive => Props.CanRevive;
        public bool CanRestoreParts => Props.CanRestoreParts;
        public bool CanStopBleeding => Props.CanStopBleeding;
        public bool CanReplaceBlood => Props.CanReplaceBlood;
        public bool ArmourReliant => Props.ArmourReliant;
        public float BaseChance => Props.BaseChance;
        public List<Hediff> healable = new List<Hediff>();

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            if (dinfo.Def != null && base.parent is Pawn pawn && pawn != null && !Props.BypassingDamageDefs.Any(def => dinfo.Def == def))
            {
                absorbed = true;
                SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.parent.Position, base.parent.Map, false));
                Vector3 impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
                Vector3 loc = base.parent.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
                float num = Mathf.Min(10f, 2f + (float)dinfo.Amount / 10f);
                MoteMaker.MakeStaticMote(loc, base.parent.Map, ThingDefOf.Mote_ExplosionFlash, num);
                int num2 = (int)num;
                for (int i = 0; i < num2; i++)
                {
                    Rand.PushState();
                    MoteMaker.ThrowDustPuff(loc, base.parent.Map, Rand.Range(0.8f, 1.2f));
                    float angle = (float)Rand.Range(0, 360);
                    Rand.PopState();


                    float num3 = Mathf.Lerp(1.2f, 1.55f, 2f);
                    Vector3 vector = pawn.Drawer.DrawPos;
                    vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                    int num4 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
                    if (num4 < 8)
                    {
                        float num5 = (float)(8 - num4) / 8f * 0.05f;
                        vector += impactAngleVect * num5;
                        num3 -= num5;
                    }
                    Vector3 s = new Vector3(num3, 1f, num3);
                    Matrix4x4 matrix = default(Matrix4x4);
                    matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
#pragma warning disable CS0436 // Type conflicts with imported type
                    Graphics.DrawMesh(MeshPool.plane10, matrix, CompInvunerableSaveOGStatic.BubbleMat, 0);
#pragma warning restore CS0436 // Type conflicts with imported type
                }
                this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
            }
            else base.PostPreApplyDamage(dinfo, out absorbed);
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            if (dinfo.Def != null && base.parent is Pawn pawn && pawn != null && !Props.BypassingDamageDefs.Any(def => dinfo.Def == def))
            {
                //healable.Add(dinfo.Def.);
            }
        }

        private int lastAbsorbDamageTick = -9999;
    }
    [StaticConstructorOnStartup]
    public static class CompInvunerableSaveOGStatic
    {
        // Token: 0x0400157E RID: 5502
        public static readonly Material BubbleMat = MaterialPool.MatFrom("Other/InvSaveBubble", ShaderDatabase.Transparent);
    }
}
