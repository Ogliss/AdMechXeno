using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_WarpSpawn
    public class DeathActionWorker_WarpSpawn : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            if (corpse != null)
            {
                corpse.Destroy();
            }
        }
    }
}
