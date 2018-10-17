using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
    // Token: 0x02000173 RID: 371
    public class LordJob_NecronsDefendShip : LordJob
    {
        // Token: 0x060007AE RID: 1966 RVA: 0x00043393 File Offset: 0x00041793
        public LordJob_NecronsDefendShip()
        {
        }

        // Token: 0x060007AF RID: 1967 RVA: 0x0004339B File Offset: 0x0004179B
        public LordJob_NecronsDefendShip(Thing shipPart, Faction faction, float defendRadius, IntVec3 defSpot)
        {
            this.shipPart = shipPart;
            this.faction = faction;
            this.defendRadius = defendRadius;
            this.defSpot = defSpot;
        }

        // Token: 0x17000136 RID: 310
        // (get) Token: 0x060007B0 RID: 1968 RVA: 0x000433C0 File Offset: 0x000417C0
        public override bool CanBlockHostileVisitors
        {
            get
            {
                return false;
            }
        }

        // Token: 0x17000137 RID: 311
        // (get) Token: 0x060007B1 RID: 1969 RVA: 0x000433C3 File Offset: 0x000417C3
        public override bool AddFleeToil
        {
            get
            {
                return false;
            }
        }

        // Token: 0x060007B2 RID: 1970 RVA: 0x000433C8 File Offset: 0x000417C8
        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            if (!this.defSpot.IsValid)
            {
                Log.Warning("LordJob_NecronsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.", false);
                stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph());
                return stateGraph;
            }
            LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, this.defendRadius);
            stateGraph.StartingToil = lordToil_DefendPoint;
            LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false);
            stateGraph.AddToil(lordToil_AssaultColony);
            LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false);
            stateGraph.AddToil(lordToil_AssaultColony2);
            Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
            transition.AddSource(lordToil_AssaultColony);
            transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
            stateGraph.AddTransition(transition, false);
            Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
            transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
            transition2.AddTrigger(new Trigger_PawnLostViolently(true));
            transition2.AddTrigger(new Trigger_Memo(CompSpawnerMechanoidsOnDamaged.MemoDamaged));
            transition2.AddPostAction(new TransitionAction_EndAllJobs());
            stateGraph.AddTransition(transition2, false);
            Transition transition3 = new Transition(lordToil_AssaultColony, lordToil_DefendPoint, false, true);
            transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
            {
                CompSpawnerNecronsOnDamaged.MemoDamaged
            }));
            transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
            stateGraph.AddTransition(transition3, false);
            Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
            transition4.AddSource(lordToil_AssaultColony);
            transition4.AddTrigger(new Trigger_ThingDamageTaken(this.shipPart, 0.5f));
            transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
            stateGraph.AddTransition(transition4, false);
            return stateGraph;
        }

        // Token: 0x060007B3 RID: 1971 RVA: 0x00043548 File Offset: 0x00041948
        public override void ExposeData()
        {
            Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
            Scribe_References.Look<Faction>(ref this.faction, "faction", false);
            Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
            Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
        }

        // Token: 0x0400035A RID: 858
        private Thing shipPart;

        // Token: 0x0400035B RID: 859
        private Faction faction;

        // Token: 0x0400035C RID: 860
        private float defendRadius;

        // Token: 0x0400035D RID: 861
        private IntVec3 defSpot;
    }
}
