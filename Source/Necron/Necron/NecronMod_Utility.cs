using System;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    // Token: 0x02000009 RID: 9
    [StaticConstructorOnStartup]
    public static class NecronMod_Utility
    {
        // Token: 0x0600002A RID: 42 RVA: 0x00002DEC File Offset: 0x00000FEC
        public static void ZombieFactory(Corpse corpse)
        {
            Thing thing = (NecronPawn)NecronMod_Utility.GenerateZombiePawnFromSource(corpse.innerPawn);
            IntVec3 position = corpse.Position;
            Building building = StoreUtility.StoringBuilding(corpse);
            if (building != null)
            {
                ((Building_Storage)building).Notify_LostThing(corpse);
            }
            GenSpawn.Spawn(thing, position);
            corpse.Destroy(0);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002E34 File Offset: 0x00001034
        public static Pawn GenerateZombiePawnFromSource(Pawn sourcePawn)
        {
            PawnKindDef pawnKindDef = PawnKindDef.Named("Zombie");
            Faction factionDirect = Find.FactionManager.FirstFactionOfDef(DefDatabase<FactionDef>.GetNamed("Zombies", true));
            Pawn pawn = (Pawn)ThingMaker.MakeThing(pawnKindDef.race, null);
            pawn.kindDef = pawnKindDef;
            pawn.SetFactionDirect(factionDirect);
            pawn.pather = new Pawn_PathFollower(pawn);
            pawn.ageTracker = new Pawn_AgeTracker(pawn);
            pawn.health = new Pawn_HealthTracker(pawn);
            pawn.jobs = new Pawn_JobTracker(pawn);
            pawn.mindState = new Pawn_MindState(pawn);
            pawn.filth = new Pawn_FilthTracker(pawn);
            pawn.needs = new Pawn_NeedsTracker(pawn);
            pawn.stances = new Pawn_StanceTracker(pawn);
            pawn.natives = new Pawn_NativeVerbs(pawn);
            PawnComponentsUtility.CreateInitialComponents(pawn);
            if (pawn.RaceProps.ToolUser)
            {
                pawn.equipment = new Pawn_EquipmentTracker(pawn);
                pawn.carrier = new Pawn_CarryTracker(pawn);
                pawn.apparel = new Pawn_ApparelTracker(pawn);
                pawn.inventory = new Pawn_InventoryTracker(pawn);
            }
            if (pawn.RaceProps.Humanlike)
            {
                pawn.ownership = new Pawn_Ownership(pawn);
                pawn.skills = new Pawn_SkillTracker(pawn);
                pawn.relations = new Pawn_RelationsTracker(pawn);
                pawn.story = new Pawn_StoryTracker(pawn);
                pawn.workSettings = new Pawn_WorkSettings(pawn);
            }
            if (pawn.RaceProps.intelligence <= 1)
            {
                pawn.caller = new Pawn_CallTracker(pawn);
            }
            pawn.gender = sourcePawn.gender;
            NecronMod_Utility.GenerateRandomAge(pawn);
            pawn.needs.SetInitialLevels();
            if (pawn.RaceProps.Humanlike)
            {
                string headGraphicPath = sourcePawn.story.HeadGraphicPath;
                typeof(Pawn_StoryTracker).GetField("headGraphicPath", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(pawn.story, headGraphicPath);
                pawn.story.skinWhiteness = sourcePawn.story.skinWhiteness;
                pawn.story.crownType = sourcePawn.story.crownType;
                pawn.story.hairColor = sourcePawn.story.hairColor;
                NecronMod_Utility.GenerateZombieBioFromSource(pawn, sourcePawn);
                pawn.story.hairDef = sourcePawn.story.hairDef;
                foreach (Trait trait in sourcePawn.story.traits.allTraits)
                {
                    pawn.story.traits.GainTrait(trait);
                }
                pawn.story.GenerateSkillsFromBackstory();
            }
            NecronMod_Utility.GenerateZombieApparelFromSource(pawn, sourcePawn);
            PawnInventoryGenerator.GenerateInventoryFor(pawn);
            ((NecronPawn)pawn).isRaiding = false;
            if (sourcePawn.IsColonist)
            {
                ((NecronPawn)pawn).wasColonist = true;
            }
            NecronMod_Utility.Zombify(pawn);
            NecronMod_Utility.SetZombieName(pawn);
            return pawn;
        }

        // Token: 0x0600002C RID: 44 RVA: 0x000030F4 File Offset: 0x000012F4
        private static void GenerateRandomAge(Pawn pawn)
        {
            int num = 0;
            int num2;
            do
            {
                if (pawn.RaceProps.ageGenerationCurve != null)
                {
                    num2 = Mathf.RoundToInt(Rand.ByCurve(pawn.RaceProps.ageGenerationCurve, 200));
                }
                else if (pawn.RaceProps.IsMechanoid)
                {
                    num2 = Rand.Range(0, 2500);
                }
                else
                {
                    if (!pawn.RaceProps.Animal)
                    {
                        goto IL_84;
                    }
                    num2 = Rand.Range(1, 10);
                }
                num++;
                if (num > 100)
                {
                    goto IL_95;
                }
            }
            while (num2 > pawn.kindDef.maxGenerationAge || num2 < pawn.kindDef.minGenerationAge);
            goto IL_A5;
            IL_84:
            Log.Warning("Didn't get age for " + pawn);
            return;
            IL_95:
            Log.Error("Tried 100 times to generate age for " + pawn);
            IL_A5:
            pawn.ageTracker.AgeBiologicalTicks = (long)((float)num2 * 3600000f) + (long)Rand.Range(0, 3600000);
            int num3;
            if (Rand.Value < pawn.kindDef.backstoryCryptosleepCommonality)
            {
                float value = Rand.Value;
                if (value < 0.7f)
                {
                    num3 = Rand.Range(0, 100);
                }
                else if (value < 0.95f)
                {
                    num3 = Rand.Range(100, 1000);
                }
                else
                {
                    int num4 = GenDate.CurrentYear - 2026 - pawn.ageTracker.AgeBiologicalYears;
                    num3 = Rand.Range(1000, num4);
                }
            }
            else
            {
                num3 = 0;
            }
            long num5 = (long)GenTicks.TicksAbs - pawn.ageTracker.AgeBiologicalTicks;
            num5 -= (long)num3 * 3600000L;
            pawn.ageTracker.BirthAbsTicks = num5;
            if (pawn.ageTracker.AgeBiologicalTicks > pawn.ageTracker.AgeChronologicalTicks)
            {
                pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x00003290 File Offset: 0x00001490
        public static void SetZombieName(Pawn pawn)
        {
            NameTriple nameTriple = pawn.Name as NameTriple;
            if (nameTriple.Nick.Contains(Translator.Translate("Zombie")))
            {
                return;
            }
            if (((NecronPawn)pawn).wasColonist)
            {
                pawn.Name = new NameTriple(nameTriple.First, string.Concat(new string[]
                {
                    "* ",
                    Translator.Translate("Zombie"),
                    " ",
                    nameTriple.Nick,
                    " *"
                }), nameTriple.Last);
                return;
            }
            if (!((NecronPawn)pawn).isRaiding && !((NecronPawn)pawn).wasRaiding)
            {
                pawn.Name = new NameTriple(nameTriple.First, Translator.Translate("Zombie") + " " + nameTriple.Nick, nameTriple.Last);
                return;
            }
            pawn.Name = new NameTriple(nameTriple.First, Translator.Translate("Zombie"), nameTriple.Last);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x0000338C File Offset: 0x0000158C
        private static NameTriple CurPawnName(Pawn pawn)
        {
            NameTriple nameTriple = pawn.Name as NameTriple;
            if (nameTriple != null)
            {
                return new NameTriple(nameTriple.First, nameTriple.Nick, nameTriple.Last);
            }
            throw new InvalidOperationException();
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000033C8 File Offset: 0x000015C8
        public static bool Zombify(Pawn pawn)
        {
            if (pawn.Drawer == null)
            {
                return false;
            }
            if (pawn.Drawer.renderer == null)
            {
                return false;
            }
            if (pawn.Drawer.renderer.graphics == null)
            {
                return false;
            }
            if (!pawn.Drawer.renderer.graphics.AllResolved)
            {
                pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            }
            if (pawn.Drawer.renderer.graphics.headGraphic == null)
            {
                return false;
            }
            if (pawn.Drawer.renderer.graphics.nakedGraphic == null)
            {
                return false;
            }
            if (pawn.Drawer.renderer.graphics.headGraphic.path == null)
            {
                return false;
            }
            if (pawn.Drawer.renderer.graphics.nakedGraphic.path == null)
            {
                return false;
            }
            Graphic nakedBodyGraphic = GraphicGetter_NakedHumanlike.GetNakedBodyGraphic(pawn.story.BodyType, ShaderDatabase.CutoutSkin, NecronMod_Utility.zombieSkinColor);
            Graphic headGraphic = GraphicDatabase.Get<Graphic_Multi>(pawn.story.HeadGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, NecronMod_Utility.zombieSkinColor);
            pawn.Drawer.renderer.graphics.headGraphic = headGraphic;
            pawn.Drawer.renderer.graphics.nakedGraphic = nakedBodyGraphic;
            if (((NecronPawn)pawn).isRaiding && UnityEngine.Random.Range(0f, 100f) < 33f)
            {
                ((NecronPawn)pawn).isRaiding = false;
                ((NecronPawn)pawn).wasRaiding = true;
            }
            return true;
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00003540 File Offset: 0x00001740
        public static void GenerateZombieBioFromSource(Pawn zombie, Pawn sourcePawn)
        {
            sourcePawn.Name = NecronMod_Utility.CurPawnName(sourcePawn);
            NameTriple name = sourcePawn.Name as NameTriple;
            zombie.Name = name;
            zombie.story.childhood = sourcePawn.story.childhood;
            zombie.story.adulthood = sourcePawn.story.adulthood;
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00003598 File Offset: 0x00001798
        public static bool SetCorpsesGreen()
        {
            if (Find.ListerThings.ThingsInGroup(8) != null)
            {
                foreach (Thing thing in Find.ListerThings.ThingsInGroup(8))
                {
                    Corpse corpse = thing as Corpse;
                    if (corpse != null && corpse.innerPawn != null && corpse.Spawned && corpse.innerPawn is NecronPawn && !NecronMod_Utility.Zombify(corpse.innerPawn as NecronPawn))
                    {
                        return false;
                    }
                }
                return true;
            }
            return true;
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00003638 File Offset: 0x00001838
        public static void GenerateZombieApparelFromSource(Pawn zombie, Pawn sourcePawn)
        {
            if (sourcePawn.apparel == null || sourcePawn.apparel.WornApparelCount == 0)
            {
                return;
            }
            foreach (Apparel apparel in sourcePawn.apparel.WornApparel)
            {
                Apparel apparel2;
                if (apparel.def.MadeFromStuff)
                {
                    apparel2 = (Apparel)ThingMaker.MakeThing(apparel.def, apparel.Stuff);
                }
                else
                {
                    apparel2 = (Apparel)ThingMaker.MakeThing(apparel.def, null);
                }
                apparel2.DrawColor = new Color(apparel.DrawColor.r, apparel.DrawColor.g, apparel.DrawColor.b, apparel.DrawColor.a);
                zombie.apparel.Wear(apparel2, true);
            }
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00003724 File Offset: 0x00001924
        public static void GenerateZombieInjuriesFromSource(Pawn zombie, Pawn sourcePawn)
        {
        }

        // Token: 0x04000010 RID: 16
        public const float biteChance = 0.1f;

        // Token: 0x04000011 RID: 17
        public const int bittenDamage = 1;

        // Token: 0x04000012 RID: 18
        public const int bittenDamageTicks = 3000;

        // Token: 0x04000013 RID: 19
        public static Color zombieSkinColor = new Color(0.37f, 0.48f, 0.35f, 1f);

        // Token: 0x04000014 RID: 20
        public static bool addedPawnComps = false;

        // Token: 0x04000015 RID: 21
        public static HediffDef Z_Virus = DefDatabase<HediffDef>.GetNamed("ZombieInfection", true);
    }
}
