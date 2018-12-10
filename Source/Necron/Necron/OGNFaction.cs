using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000566 RID: 1382
	public class OGNFaction : Faction
    {

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001A00 RID: 6656 RVA: 0x000C8BDD File Offset: 0x000C6FDD
		public static Faction OfNecrons
		{
			get
			{
				return OGFind.OGNFactionManager.OfNecrons;
			}
		}
        
	}
}
