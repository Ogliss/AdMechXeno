using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Necron
{
	public class Faction : IExposable, ILoadReferenceable, RimWorld.ICommunicable
	{
		public RimWorld.FactionDef def;

		private string name;

		public int loadID = -1;

		public int randomKey;

		public float colorFromSpectrum;

		public float centralMelanin = 0.5f;

		private List<RimWorld.FactionRelation> relations = new List<RimWorld.FactionRelation>();

		public Pawn leader;

		private RimWorld.FactionTacticalMemory tacticalMemoryInt = new RimWorld.FactionTacticalMemory();

		public RimWorld.KidnappedPawnsTracker kidnapped;

		private List<RimWorld.PredatorThreat> predatorThreats = new List<RimWorld.PredatorThreat>();

		public Dictionary<Map, ByteGrid> avoidGridsBasic = new Dictionary<Map, ByteGrid>();

		public Dictionary<Map, ByteGrid> avoidGridsSmart = new Dictionary<Map, ByteGrid>();

		public bool defeated;

		public int lastTraderRequestTick = -9999999;

		private List<Map> avoidGridsBasicKeysWorkingList;

		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		private List<Map> avoidGridsSmartKeysWorkingList;

		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		private const float InitialHostileThreshold = -40f;

		private const float BecomeHostileThreshold = -80f;

		private const float BecomeNeutralThreshold = 0f;

		public const float GoodwillScale = 100f;

		private const float GoodwillLossPerDamage = 1.3f;

		private const float GoodwillLossFractionOnCrushed = 0.5f;

		private const float GoodwillLossFractionOnNeutralDeath = 0.1f;

		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		[CompilerGenerated]
		private static Predicate<FactionBase> <>f__mg$cache0;

		[CompilerGenerated]
		private static Predicate<FactionBase> <>f__mg$cache1;

		public string Name
		{
			get
			{
				if (this.HasName)
				{
					return this.name;
				}
				return this.def.LabelCap;
			}
			set
			{
				this.name = value;
			}
		}

		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		public float PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		public RimWorld.FactionTacticalMemory TacticalMemory
		{
			get
			{
				if (this.tacticalMemoryInt == null)
				{
					this.tacticalMemoryInt = new RimWorld.FactionTacticalMemory();
				}
				return this.tacticalMemoryInt;
			}
		}

		public Color Color
		{
			get
			{
				if (this.def.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return ColorsFromSpectrum.Get(this.def.colorSpectrum, this.colorFromSpectrum);
			}
		}

		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.");
				}
				return ofPlayerSilentFail;
			}
		}
        
        public static Faction OfNecrons
        {
            get
            {
                return Find.FactionManager.FirstFactionOfDef(FactionDefOf.Necron);
            }
        }
        
		public static Faction OfPlayerSilentFail
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					GameInitData gameInitData = Find.GameInitData;
					if (gameInitData != null && gameInitData.playerFaction != null)
					{
						return gameInitData.playerFaction;
					}
				}
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int i = 0; i < allFactionsListForReading.Count; i++)
				{
					if (allFactionsListForReading[i].def.isPlayer)
					{
						return allFactionsListForReading[i];
					}
				}
				return null;
			}
		}

		public Faction()
		{
			this.randomKey = Rand.Range(0, 2147483647);
			this.kidnapped = new RimWorld.KidnappedPawnsTracker(this);
		}

		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.leader, "leader", false);
			Scribe_Collections.Look<Map, ByteGrid>(ref this.avoidGridsBasic, "avoidGridsBasic", LookMode.Reference, LookMode.Deep, ref this.avoidGridsBasicKeysWorkingList, ref this.avoidGridsBasicValuesWorkingList);
			Scribe_Collections.Look<Map, ByteGrid>(ref this.avoidGridsSmart, "avoidGridsSmart", LookMode.Reference, LookMode.Deep, ref this.avoidGridsSmartKeysWorkingList, ref this.avoidGridsSmartValuesWorkingList);
			Scribe_Defs.Look<RimWorld.FactionDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.randomKey, "randomKey", 0, false);
			Scribe_Values.Look<float>(ref this.colorFromSpectrum, "colorFromSpectrum", 0f, false);
			Scribe_Values.Look<float>(ref this.centralMelanin, "centralMelanin", 0f, false);
			Scribe_Collections.Look<RimWorld.FactionRelation>(ref this.relations, "relations", LookMode.Deep, new object[0]);
			Scribe_Deep.Look<RimWorld.FactionTacticalMemory>(ref this.tacticalMemoryInt, "tacticalMemory", new object[0]);
			Scribe_Deep.Look<RimWorld.KidnappedPawnsTracker>(ref this.kidnapped, "kidnapped", new object[]
			{
				this
			});
			Scribe_Collections.Look<RimWorld.PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, new object[0]);
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((RimWorld.PredatorThreat x) => x.predator == null);
			}
		}

		public void FactionTick()
		{
			if (Find.TickManager.TicksGame % 1001 == 0 && this != Faction.OfPlayer)
			{
				if (this.PlayerGoodwill < this.def.naturalColonyGoodwill.min)
				{
					this.AffectGoodwillWith(Faction.OfPlayer, this.def.goodwillDailyGain * 0f);
				}
				else if (this.PlayerGoodwill > this.def.naturalColonyGoodwill.max)
				{
					this.AffectGoodwillWith(Faction.OfPlayer, -this.def.goodwillDailyFall * 0f);
				}
			}
			this.kidnapped.KidnappedPawnsTrackerTick();
			for (int i = this.predatorThreats.Count - 1; i >= 0; i--)
			{
				RimWorld.PredatorThreat predatorThreat = this.predatorThreats[i];
				if (predatorThreat.Expired)
				{
					this.predatorThreats.RemoveAt(i);
					if (predatorThreat.predator.Spawned)
					{
						predatorThreat.predator.Map.attackTargetsCache.UpdateTarget(predatorThreat.predator);
					}
				}
			}
			if (Find.TickManager.TicksGame % 1000 == 200 && this.IsPlayer)
			{
				if (RimWorld.NamePlayerFactionAndBaseUtility.CanNameFactionNow())
				{
					List<FactionBase> arg_164_0 = Find.WorldObjects.FactionBases;
					if (Faction.<>f__mg$cache0 == null)
					{
						Faction.<>f__mg$cache0 = new Predicate<FactionBase>(RimWorld.NamePlayerFactionAndBaseUtility.CanNameFactionBaseSoon);
					}
					FactionBase factionBase = arg_164_0.Find(Faction.<>f__mg$cache0);
					if (factionBase != null)
					{
						Find.WindowStack.Add(new RimWorld.Dialog_NamePlayerFactionAndBase(factionBase));
					}
					else
					{
						Find.WindowStack.Add(new RimWorld.Dialog_NamePlayerFaction());
					}
				}
				else
				{
					List<FactionBase> arg_1C0_0 = Find.WorldObjects.FactionBases;
					if (Faction.<>f__mg$cache1 == null)
					{
						Faction.<>f__mg$cache1 = new Predicate<FactionBase>(RimWorld.NamePlayerFactionAndBaseUtility.CanNameFactionBaseNow);
					}
					FactionBase factionBase2 = arg_1C0_0.Find(Faction.<>f__mg$cache1);
					if (factionBase2 != null)
					{
						if (RimWorld.NamePlayerFactionAndBaseUtility.CanNameFactionSoon())
						{
							Find.WindowStack.Add(new RimWorld.Dialog_NamePlayerFactionAndBase(factionBase2));
						}
						else
						{
							Find.WindowStack.Add(new RimWorld.Dialog_NamePlayerFactionBase(factionBase2));
						}
					}
				}
			}
		}

		public ByteGrid GetAvoidGridBasic(Map map)
		{
			ByteGrid result;
			if (this.avoidGridsBasic.TryGetValue(map, out result))
			{
				return result;
			}
			return null;
		}

		public ByteGrid GetAvoidGridSmart(Map map)
		{
			ByteGrid result;
			if (this.avoidGridsSmart.TryGetValue(map, out result))
			{
				return result;
			}
			return null;
		}

		public void Notify_MapRemoved(Map map)
		{
			this.avoidGridsBasic.Remove(map);
			this.avoidGridsSmart.Remove(map);
			this.tacticalMemoryInt.Notify_MapRemoved(map);
		}

		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			float a = this.def.startingGoodwill.RandomInRange;
			if (this.IsPlayer)
			{
				a = 100f;
			}
			float b = other.def.startingGoodwill.RandomInRange;
			if (other.IsPlayer)
			{
				b = 100f;
			}
			float num = Mathf.Min(a, b);
			RimWorld.FactionRelation factionRelation = new RimWorld.FactionRelation();
			factionRelation.other = other;
			factionRelation.goodwill = num;
			factionRelation.hostile = (num < -40f);
			this.relations.Add(factionRelation);
			RimWorld.FactionRelation factionRelation2 = new RimWorld.FactionRelation();
			factionRelation2.other = this;
			factionRelation2.goodwill = num;
			factionRelation2.hostile = (num < -40f);
			other.relations.Add(factionRelation2);
		}

		public PawnKindDef RandomPawnKind()
		{
			Faction.allPawnKinds.Clear();
			if (this.def.pawnGroupMakers != null)
			{
				for (int i = 0; i < this.def.pawnGroupMakers.Count; i++)
				{
					List<RimWorld.PawnGenOption> options = this.def.pawnGroupMakers[i].options;
					for (int j = 0; j < options.Count; j++)
					{
						Faction.allPawnKinds.Add(options[j].kind);
					}
				}
			}
			if (!Faction.allPawnKinds.Any<PawnKindDef>())
			{
				return this.def.basicMemberKind;
			}
			PawnKindDef result = Faction.allPawnKinds.RandomElement<PawnKindDef>();
			Faction.allPawnKinds.Clear();
			return result;
		}

		public RimWorld.FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + this + " and itself.");
				return new RimWorld.FactionRelation();
			}
			for (int i = 0; i < this.relations.Count; i++)
			{
				if (this.relations[i].other == other)
				{
					return this.relations[i];
				}
			}
			if (!allowNull)
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					this.name,
					" has null relation with ",
					other,
					". Returning dummy relation."
				}));
				return new RimWorld.FactionRelation();
			}
			return null;
		}

		public float GoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).goodwill;
		}

		public bool AffectGoodwillWith(Faction other, float goodwillChange)
		{
			if (this.def.hidden || other.def.hidden)
			{
				return false;
			}
			if (goodwillChange > 0f && !this.def.appreciative)
			{
				return false;
			}
			if (goodwillChange > 0f && ((this.IsPlayer && SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) || (other.IsPlayer && SettlementUtility.IsPlayerAttackingAnySettlementOf(this))))
			{
				return false;
			}
			float num = this.GoodwillWith(other);
			float value = num + goodwillChange;
			RimWorld.FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.goodwill = Mathf.Clamp(value, -100f, 100f);
			if (!this.HostileTo(other) && this.GoodwillWith(other) < -80f)
			{
				this.SetHostileTo(other, true);
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.TicksGame > 100 && other == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChangeBad".Translate(), "RelationsBrokenDown".Translate(new object[]
					{
						this.name
					}),RimWorld.LetterDefOf.PositiveEvent.NegativeEvent, null);
				}
			}
			if (this.HostileTo(other) && this.GoodwillWith(other) > 0f)
			{
				this.SetHostileTo(other, false);
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.TicksGame > 100 && other == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChangeGood".Translate(), "RelationsWarmed".Translate(new object[]
					{
						this.name
					}), RimWorld.LetterDefOf.PositiveEvent, null);
				}
			}
			bool flag = (this.IsPlayer || other.IsPlayer) && !this.HostileTo(other);
			return flag && this.def.appreciative && (goodwillChange > 0f || factionRelation.goodwill != num);
		}

		public void SetHostileTo(Faction other, bool hostile)
		{
			if (this.def.hidden || other.def.hidden)
			{
				return;
			}
			RimWorld.FactionRelation factionRelation = this.RelationWith(other, false);
			if (hostile)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					foreach (Pawn current in RimWorld.PawnsFinder.AllMapsWorldAndTemporary_Alive.ToList<Pawn>())
					{
						if ((current.Faction == this && current.HostFaction == other) || (current.Faction == other && current.HostFaction == this))
						{
							current.guest.SetGuestStatus(current.HostFaction, true);
						}
					}
				}
				if (!factionRelation.hostile)
				{
					other.RelationWith(this, false).hostile = true;
					factionRelation.hostile = true;
					if (factionRelation.goodwill > -80f)
					{
						factionRelation.goodwill = -80f;
					}
				}
			}
			else if (factionRelation.hostile)
			{
				other.RelationWith(this, false).hostile = false;
				factionRelation.hostile = false;
				if (factionRelation.goodwill < 0f)
				{
					factionRelation.goodwill = 0f;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].attackTargetsCache.Notify_FactionHostilityChanged(this, other);
					LordManager lordManager = maps[i].lordManager;
					for (int j = 0; j < lordManager.lords.Count; j++)
					{
						Lord lord = lordManager.lords[j];
						if (lord.faction == other)
						{
							lord.Notify_RimWorld.FactionRelationsChanged(this);
						}
						else if (lord.faction == this)
						{
							lord.Notify_RimWorld.FactionRelationsChanged(other);
						}
					}
				}
			}
		}

		public void RemoveAllRelations()
		{
			foreach (Faction current in Find.FactionManager.AllFactionsListForReading)
			{
				if (current != this)
				{
					current.relations.RemoveAll((RimWorld.FactionRelation x) => x.other == this);
				}
			}
			this.relations.Clear();
		}

		public void Notify_MemberTookDamage(Pawn member, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == RimWorld.JobDefOf.PredatorHunt)
			{
				this.TookDamageFromPredator(pawn);
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.externalViolence || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (member.InMentalState && member.MentalStateDef.IsAggro)
			{
				return;
			}
			Pawn pawn2 = dinfo.Instigator as Pawn;
			if (pawn2 != null && pawn2.InMentalState && pawn2.MentalStateDef == RimWorld.MentalStateDefOf.Berserk)
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && RimWorld.PrisonBreakUtility.IsPrisonBreaking(member))
			{
				return;
			}
			float num = (float)Mathf.Min(100, dinfo.Amount);
			float goodwillChange = -1.3f * num;
			if (dinfo.Instigator.Faction == Faction.OfPlayer && this != Faction.OfPlayer)
			{
				this.AffectGoodwillWith(dinfo.Instigator.Faction, goodwillChange);
			}
		}

		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (violator == this || this.HostileTo(violator) || member.Faction.def.hidden || violator.def.hidden)
			{
				return;
			}
			this.SetHostileTo(violator, true);
			Find.LetterStack.ReceiveLetter("LetterLabelRelationsChangeBad".Translate().CapitalizeFirst(), "RelationsBrokenCapture".Translate(new object[]
			{
				member,
				this.name
			}),RimWorld.LetterDefOf.PositiveEvent.NegativeEvent, member, null);
		}

		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, Map map)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo.HasValue && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					float num = member.RaceProps.body.corePart.def.GetMaxHealth(member) * 1.3f * 0.5f * -1f;
					bool flag = this.AffectGoodwillWith(Faction.OfPlayer, num);
					if (flag && MessagesRepeatAvoider.MessageShowAllowed("RimWorld.FactionRelationAdjustmentCrushed-" + this.Name, 5f))
					{
						Messages.Message("MessageFactionPawnCrushed".Translate(new object[]
						{
							this.Name,
							Mathf.RoundToInt(num)
						}), member, RimWorld.MessageTypeDefOf.NegativeEvent);
					}
				}
				else if (dinfo.HasValue && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					float num2 = member.RaceProps.body.corePart.def.GetMaxHealth(member) * 1.3f * 0.1f * -1f;
					if (this.AffectGoodwillWith(Faction.OfPlayer, num2))
					{
						Messages.Message("MessageFactionPawnLost".Translate(new object[]
						{
							this.Name,
							member.NameStringShort,
							Mathf.RoundToInt(num2)
						}), member, RimWorld.MessageTypeDefOf.NegativeEvent);
					}
				}
			}
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		public void Notify_LeaderDied()
		{
			Pawn pawn = this.leader;
			this.GenerateNewLeader();
			Find.LetterStack.ReceiveLetter("LetterLeadersDeathLabel".Translate(new object[]
			{
				this.name,
				this.def.leaderTitle
			}).CapitalizeFirst(), "LetterLeadersDeath".Translate(new object[]
			{
				pawn.Name.ToStringFull,
				this.name,
				this.leader.Name.ToStringFull,
				this.def.leaderTitle
			}).CapitalizeFirst(),RimWorld.LetterDefOf.PositiveEvent.NeutralEvent, null);
		}

		public void Notify_LeaderLost()
		{
			Pawn pawn = this.leader;
			this.GenerateNewLeader();
			Find.LetterStack.ReceiveLetter("LetterLeaderChangedLabel".Translate(new object[]
			{
				this.name,
				this.def.leaderTitle
			}).CapitalizeFirst(), "LetterLeaderChanged".Translate(new object[]
			{
				pawn.Name.ToStringFull,
				this.name,
				this.leader.Name.ToStringFull,
				this.def.leaderTitle
			}).CapitalizeFirst(),RimWorld.LetterDefOf.PositiveEvent.NeutralEvent, null);
		}

		public void GenerateNewLeader()
		{
			this.leader = null;
			if (this.def.pawnGroupMakers != null)
			{
				List<PawnKindDef> list = new List<PawnKindDef>();
				foreach (RimWorld.PawnGroupMaker current in from x in this.def.pawnGroupMakers
				where x.kindDef == RimWorld.PawnGroupKindDefOf.Normal
				select x)
				{
					foreach (RimWorld.PawnGenOption current2 in current.options)
					{
						if (current2.kind.factionLeader)
						{
							list.Add(current2.kind);
						}
					}
				}
				PawnKindDef kindDef;
				if (list.TryRandomElement(out kindDef))
				{
					this.leader = PawnGenerator.GeneratePawn(kindDef, this);
					if (this.leader.RaceProps.IsFlesh)
					{
						this.leader.relations.everSeenByPlayer = true;
					}
					if (!Find.WorldPawns.Contains(this.leader))
					{
						Find.WorldPawns.PassToWorld(this.leader, PawnDiscardDecideMode.KeepForever);
					}
				}
			}
		}

		public string GetCallLabel()
		{
			return this.name;
		}

		public string GetInfoText()
		{
			string text = this.def.LabelCap;
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"\n",
				"ColonyGoodwill".Translate(),
				": ",
				this.PlayerGoodwill.ToString("###0")
			});
			if (this.HostileTo(Faction.OfPlayer))
			{
				text = text + "\n" + "Hostile".Translate();
			}
			else
			{
				text = text + "\n" + "Neutral".Translate();
			}
			return text;
		}

		public void TryOpenComms(Pawn negotiator)
		{
            RimWorld.Dialog_Negotiation dialog_Negotiation = new RimWorld.Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = RimWorld.SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		private void TookDamageFromPredator(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					this.predatorThreats[i].lastAttackTicks = Find.TickManager.TicksGame;
					return;
				}
			}
			RimWorld.PredatorThreat predatorThreat = new RimWorld.PredatorThreat();
			predatorThreat.predator = predator;
			predatorThreat.lastAttackTicks = Find.TickManager.TicksGame;
			this.predatorThreats.Add(predatorThreat);
		}

		public bool HasPredatorRecentlyAttackedAnyone(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					return true;
				}
			}
			return false;
		}

		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID;
		}

		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			if (this.def != null)
			{
				return this.def.defName;
			}
			return "[faction of no def]";
		}

		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			ByteGrid avoidGridSmart = this.GetAvoidGridSmart(Find.VisibleMap);
			if (avoidGridSmart != null)
			{
				stringBuilder.Append("Avoidgrid val at mouse: ");
				if (UI.MouseCell().InBounds(Find.VisibleMap))
				{
					stringBuilder.AppendLine(avoidGridSmart[UI.MouseCell()].ToString());
				}
				stringBuilder.Append("Avoidgrid pathcost at mouse: ");
				if (UI.MouseCell().InBounds(Find.VisibleMap))
				{
					stringBuilder.AppendLine(((int)(avoidGridSmart[UI.MouseCell()] * 8)).ToString());
				}
			}
			return stringBuilder.ToString();
		}

		public void DebugDrawOnMap()
		{
			ByteGrid avoidGridSmart = this.GetAvoidGridSmart(Find.VisibleMap);
			if (avoidGridSmart != null)
			{
				avoidGridSmart.DebugDraw();
			}
		}
	}
}
