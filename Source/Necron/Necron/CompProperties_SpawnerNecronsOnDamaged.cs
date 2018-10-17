using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000257 RID: 599
	public class CompProperties_SpawnerNecronsOnDamaged : CompProperties
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x00055350 File Offset: 0x00053750
		public CompProperties_SpawnerNecronsOnDamaged()
		{
			this.compClass = typeof(CompSpawnerNecronsOnDamaged);
		}

        public Faction FactionDef = null;
	}
}
