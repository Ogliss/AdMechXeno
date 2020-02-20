using AdeptusMechanicus;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
    // Token: 0x02000082 RID: 130
    public class JobDriver_TombSpyderRepair : JobDriver
    {
        // Token: 0x170000B6 RID: 182
        // (get) Token: 0x06000371 RID: 881 RVA: 0x0001EEFC File Offset: 0x0001D2FC
        protected Pawn DamagedNecron
        {
            get
            {
                if (this.job.GetTarget(TargetIndex.A).Thing is Corpse corpse)
                {
                    return corpse.InnerPawn;
                }
                return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
            }
        }
        
        protected Comp_NecronOG _Necron
        {
            get
            {
                return DamagedNecron.TryGetComp<Comp_NecronOG>();
            }
        }

        // Token: 0x06000391 RID: 913 RVA: 0x00024554 File Offset: 0x00022954
        public override void Notify_Starting()
        {
            base.Notify_Starting();
        }

        // Token: 0x06000372 RID: 882 RVA: 0x0001EF20 File Offset: 0x0001D320
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.targetA;
            Job job = this.job;
            return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
        }

        // Token: 0x06000373 RID: 883 RVA: 0x0001EF58 File Offset: 0x0001D358
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            if (_Necron == null)
            {
                Log.Warning("ERROR!!!! _Necron null");
                yield break;
            }
            if (!DamagedNecron.Spawned && !DamagedNecron.Corpse.Spawned)
            {
                Log.Warning("ERROR!!!! DamagedNecron despawned");
                yield break;

            }
            if (DamagedNecron == null)
            {
                Log.Warning("ERROR!!!! DamagedNecron null");
                yield break;

            }
            if (!_Necron.PawnHediffs.NullOrEmpty())
            {
            //    Log.Message("CHECKING!!!! _Necron Damage");
                foreach (Hediff hd in _Necron.PawnHediffs)
                {
                    if (hd.def.hediffClass == typeof(Hediff_Injury))
                    {
                    //    Log.Message("INJURY!!!!");
                        this.useDuration += (int)(((hd.Severity * DamagedNecron.BodySize))* (_Necron.UnhealableHediffs.Contains(hd) ? 3 : 1) );
                    }
                    else if (hd.def.hediffClass == typeof(Hediff_MissingPart) && !DamagedNecron.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hd.Part))
                    {
                    //    Log.Message("MISSING PART!!!!");
                        this.useDuration += (int)(((300 * DamagedNecron.BodySize) * hd.Severity) * (_Necron.UnhealableHediffs.Contains(hd) ? 3 : 1));
                    }
                    else
                    {
                    //    Log.Message("STILL CHECKING!!!!");
                        if (hd.def.hediffClass != typeof(Hediff_Implant) && hd.def.hediffClass!=typeof(Hediff_MissingPart))
                        {
                        //    Log.Message("NOT IMPLANT OR MISSINGPART!!!!");

                        }
                    //    Log.Message(string.Format("Missed {0} {1} on {2}", hd.LabelCap, hd.def.hediffClass, hd.Part.LabelCap));
                    }
                }
            }
            else
            {
             //   Log.Warning("ERROR!!!! DamagedNecron null");
            }
            if (DamagedNecron.Dead)
            {
                this.useDuration = (int)(500 * DamagedNecron.BodySize);
            }
            Toil prepare = Toils_General.Wait(this.useDuration, TargetIndex.A);
            prepare.NPCWithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            prepare.WithEffect(EffecterDefOf.ConstructMetal, TargetIndex.A);
            prepare.PlaySustainerOrSound(() => SoundDefOf.Crunch);
            if (DamagedNecron.Dead)
            {
                yield return prepare;
                Toil use = new Toil();
                use.initAction = delegate ()
                {
                    Pawn Spyder = use.actor;
                    _Necron.TryRevive(true);
                };
                use.defaultCompleteMode = ToilCompleteMode.Instant;
                yield return use;
            }
            else
            {
                if (DamagedNecron.Downed)
                {
                    yield return prepare;
                    Toil use = new Toil();
                    use.initAction = delegate ()
                    {
                        Pawn Spyder = use.actor;
                        foreach (Hediff hd in _Necron.PawnHediffs)
                        {
                            DamagedNecron.health.RemoveHediff(hd);
                        }
                    };
                    use.defaultCompleteMode = ToilCompleteMode.Instant;
                    yield return use;
                    if (_Necron.originalWeapon != null)
                    {
                        ThingWithComps thing = _Necron.originalWeapon;
                        if (thing.Spawned)
                        {
                            thing.DeSpawn();
                        }
                        if (DamagedNecron.inventory.innerContainer.Contains(thing))
                        {
                            DamagedNecron.inventory.innerContainer.Remove(thing);
                        }
                        DamagedNecron.equipment.AddEquipment(thing);
                    }
                    if (_Necron.secondryWeapon != null)
                    {
                        ThingWithComps thing = _Necron.secondryWeapon;
                        if (thing.Spawned)
                        {
                            thing.DeSpawn();
                        }
                        if (DamagedNecron.inventory.innerContainer.Contains(thing))
                        {
                            DamagedNecron.inventory.innerContainer.Remove(thing);
                        }
                        DamagedNecron.equipment.AdMechAddOffHandEquipment(thing);
                    }
                }
            }
            yield break;
        }

        private int useDuration = -1;
    }
}
