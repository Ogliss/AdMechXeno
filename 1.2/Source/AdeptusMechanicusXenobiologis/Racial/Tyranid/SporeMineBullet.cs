using System;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000003 RID: 3
    public class SporeMine : ArcingBullet
    {

        public override Graphic Graphic
        {
            get
            {
               return base.DefaultGraphic.GetColoredVersion(base.DefaultGraphic.Shader, Launcher.DrawColor, DrawColorTwo);
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x0000208C File Offset: 0x0000028C
        public override void Impact(Thing hitThing)
		{
			Faction faction = this.launcher.Faction;
			ThingDef raceDef = this.def.projectile.postExplosionSpawnThingDef;
			Pawn pawn = GenSpawn.Spawn(PawnGenerator.GeneratePawn(SporeMine.mineDef, faction), base.Position, base.Map, WipeMode.Vanish) as Pawn;
			pawn.def = raceDef;
			pawn.health.AddHediff(HediffDefOf.Scaria, null, null, null);
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
			GenClamor.DoClamor(this, 2.1f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04000001 RID: 1
		public static readonly PawnKindDef mineDef = PawnKindDef.Named("OG_Tyranid_SporeMine");
	}
}
