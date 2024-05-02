using System;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    public class DeathActionProperties_Necron : DeathActionProperties
    {
        public DeathActionProperties_Necron()
        {
            this.workerClass = typeof(DeathActionWorker_Necron);
        }

        public IntRange baseRegenerationDelay = new IntRange(150, 500);
    }

    public class DeathActionWorker_Necron : DeathActionWorker
    {
        public DeathActionProperties_Necron Props => (DeathActionProperties_Necron)this.props;
        float maxtime = 30f;
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            Comp_NecronOG _Necron = corpse.InnerPawn.TryGetCompFast<Comp_NecronOG>();
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
            int delay = Props.baseRegenerationDelay.RandomInRange;
            foreach (Hediff item in _Necron.healableHediffs)
            {
                delay += (int)(10*item.Severity);
            }
            _Necron.reviveIntervalTicks = Math.Min(delay, maxtime.SecondsToTicks());
        //    Log.Message(string.Format("{0} tries revive in {1} seconds ({2} ticks)",corpse.InnerPawn.Label, _Necron.reviveIntervalTicks.TicksToSeconds(), _Necron.reviveIntervalTicks));
        }
        
    }
}
