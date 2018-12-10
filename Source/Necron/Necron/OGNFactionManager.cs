using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200056A RID: 1386
	public class OGNFactionManager : FactionManager
    {
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x000CAADB File Offset: 0x000C8EDB
		public Faction OfNecrons
        {
			get
			{
				return this.ofNecrons;
			}
		}
        

		// Token: 0x04000FA5 RID: 4005
		private Faction ofNecrons;
        
	}
}
