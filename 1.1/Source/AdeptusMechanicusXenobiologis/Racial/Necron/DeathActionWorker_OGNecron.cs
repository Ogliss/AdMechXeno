using System;
using AdeptusMechanicus;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
    // Token: 0x02000B58 RID: 2904
    public class DeathActionWorker_OGNecron : DeathActionWorker
    {
        float maxtime = 30f;
        // Token: 0x0600403D RID: 16445 RVA: 0x001E1AD3 File Offset: 0x001DFED3
        public override void PawnDied(Corpse corpse)
        {
            Comp_NecronOG _Necron = corpse.InnerPawn.TryGetComp<Comp_NecronOG>();
            if (_Necron==null)
            {
                return;
            }
            if (_Necron.originalWeapon!=null)
            {
                if (_Necron.originalWeapon.Spawned)
                {
                    _Necron.originalWeapon.DeSpawn();
                }
                corpse.InnerPawn.inventory.innerContainer.TryAddOrTransfer(_Necron.originalWeapon);
            }
            if (_Necron.secondryWeapon!=null)
            {
                if (_Necron.secondryWeapon.Spawned)
                {
                    _Necron.secondryWeapon.DeSpawn();
                }
                corpse.InnerPawn.inventory.innerContainer.TryAddOrTransfer(_Necron.secondryWeapon);
            }
            int delay = 500;
            foreach (Hediff item in _Necron.healableHediffs)
            {
                delay += (int)(10*item.Severity);
            }
            _Necron.reviveIntervalTicks = Math.Min(delay, maxtime.SecondsToTicks());
        //    Log.Message(string.Format("{0} tries revive in {1} seconds ({2} ticks)",corpse.InnerPawn.Label, _Necron.reviveIntervalTicks.TicksToSeconds(), _Necron.reviveIntervalTicks));
        }
        
    }
}
