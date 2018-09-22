using System;
using Verse;
using Verse.AI.Group;

namespace Necron
{
	public class LordJob_NecronsDefendShip : LordJob
	{
		private Thing shipPart;

		private RimWorld.Faction faction;

		private float defendRadius;

		private IntVec3 defSpot;

		public LordJob_NecronsDefendShip()
		{
		}

		public LordJob_NecronsDefendShip(Thing shipPart, RimWorld.Faction faction, float defendRadius, IntVec3 defSpot)
		{
			this.shipPart = shipPart;
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
		}

		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.");
				stateGraph.AttachSubgraph(new RimWorld.LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph());
				return stateGraph;
			}
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius);
			stateGraph.StartingToil = lordToil_DefendPoint;
			RimWorld.LordToil_AssaultColony lordToil_AssaultColony = new RimWorld.LordToil_AssaultColony();
			stateGraph.AddToil(lordToil_AssaultColony);
            RimWorld.LordToil_AssaultColony lordToil_AssaultColony2 = new RimWorld.LordToil_AssaultColony();
			stateGraph.AddToil(lordToil_AssaultColony2);
			Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true));
			transition2.AddTrigger(new Trigger_PawnLostViolently());
			transition2.AddTrigger(new Trigger_Memo(CompSpawnerNecronsOnDamaged.MemoDamaged));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2);
			Transition transition3 = new Transition(lordToil_AssaultColony, lordToil_DefendPoint);
			transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
			{
                CompSpawnerNecronsOnDamaged.MemoDamaged
			}));
			transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition3);
			Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2);
			transition4.AddSource(lordToil_AssaultColony);
			transition4.AddTrigger(new Trigger_ThingDamageTaken(this.shipPart, 0.5f));
			transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			stateGraph.AddTransition(transition4);
			return stateGraph;
		}

		public override void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
			Scribe_References.Look<RimWorld.Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
		}
	}
}
