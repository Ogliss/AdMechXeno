using System;
using RimWorld;
using AdeptusMechanicus;
using Verse;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.ScenPart_AMXBRelations
    public class ScenPart_AMXBRelations : ScenPart
    {

        public override void PostWorldGenerate()
        {
            if (AMAMod.settings.ForceRelations)
            {
                int i = 0;
                Faction faction = null;
                FactionDef factionDef = null;
                ThingDef factionRace = null;
                Faction player = Faction.OfPlayer;
                FactionDef playerDef = player.def;
                ThingDef playerRace = playerDef.basicMemberKind.race;
            //    Log.Message(string.Format("PWG Player: {0}\nDef: {1}\nRace: {2}", player, playerDef, playerRace));
                base.PostWorldGenerate();
                foreach (var f in Find.FactionManager.AllFactionsListForReading.FindAll(x => !x.IsPlayer && !x.def.permanentEnemy))
                {
                    faction = null;
                    factionDef = null;
                    factionRace = null;
                    faction = f;
                    factionDef = faction.def;
                    factionRace = factionDef.basicMemberKind?.race;
                    if (factionDef.basicMemberKind == null)
                    {
                    //    Log.Message(string.Format("basicMemberKind: Missing, checking PGM for {0} def: {1}", f, factionDef));
                        if (!factionDef.pawnGroupMakers.NullOrEmpty())
                        {
                            //    Log.Message(string.Format("PGM: Found, checking options for {0} def: {1}", f, factionDef));
                            PawnGroupMaker maker = factionDef.pawnGroupMakers.RandomElement();
                            if (!maker.options.NullOrEmpty())
                            {
                                //    Log.Message(string.Format("options: Found, checking for {0} def: {1}", f, factionDef));
                                PawnGenOption genOption = maker.options.RandomElement();
                                if (genOption != null)
                                {
                                    factionRace = genOption.kind.race;
                                    //    Log.Message(string.Format("Race: {2}, checking for {0} def: {1}", f, factionDef, factionRace));
                                }
                                else
                                {
                                //    Log.Message(string.Format("Race: Not Found for {0} def: {1}", f, factionDef));
                                }
                            }
                            else
                            {
                            //    Log.Message(string.Format("options: Not Found for {0} def: {1}", f, factionDef));
                            }
                        }
                        else
                        {
                        //    Log.Message(string.Format("PGM: Missing for {0}", factionDef));
                        }
                    }
                    if (factionRace != null)
                    {
                        //    Log.Message(string.Format("Race: {0}", factionRace));
                        if (factionRace != playerRace)
                        {
                            if ((factionDef.defName.Contains("OG_Mechanicus_") || factionDef.defName.Contains("OG_Militarum_") || factionDef.defName.Contains("OG_Astartes_") || factionDef.defName.Contains("OG_Sororitas_")) && (playerRace.defName.Contains("Human") || (playerRace.defName.Contains("OG_Mechanicus_") || playerRace.defName.Contains("OG_Militarum_") || playerRace.defName.Contains("OG_Astartes_") || playerRace.defName.Contains("OG_Sororitas_"))) && !playerRace.defName.Contains("OG_Chaos"))
                            {
                            //    Log.Message(string.Format("Imperial Faction {0}: {1} should be friendly to {2}", f, factionDef, player));
                                if (f.GoodwillWith(player) < 0)
                                {
                                    player.TrySetRelationKind(f, FactionRelationKind.Ally, false);
                                }

                            }
                            else if ((factionDef.defName.Contains("OG_Eldar_") && (playerRace.defName.Contains("Alien_Eldar") || playerRace.defName.Contains("OG_Eldar_"))))
                            {
                            //    Log.Message(string.Format("Eldar faction{0}: {1} should be friendly to {2}: {3}", f, factionDef, player, playerDef));
                                if (f.GoodwillWith(player) < 0)
                                {
                                    player.TrySetRelationKind(f, FactionRelationKind.Ally, false);
                                }
                            }
                            else if ((factionDef.defName.Contains("OG_Ork_")) && (playerRace.defName.Contains("Alien_Ork") || playerRace.defName.Contains("Alien_Grot") || playerRace.defName.Contains("OG_Ork_") || playerRace.defName.Contains("OG_Grot_")))
                            {
                            //    Log.Message(string.Format("Ork faction {0}: {1} should be friendly to {2}: {3}", f, f.def, player, playerDef));
                                if (f.GoodwillWith(player) < 0)
                                {
                                    player.TrySetRelationKind(f, FactionRelationKind.Neutral, false);
                                }
                            }
                            else if ((factionDef.defName.Contains("OG_Tau_") || factionDef.defName.Contains("OG_Kroot_") || factionDef.defName.Contains("OG_Vespid_")) && (playerRace.defName.Contains("Alien_Tau") || playerRace.defName.Contains("Alien_Kroot") || playerRace.defName.Contains("OG_Tau_") || playerRace.defName.Contains("OG_Kroot_")))
                            {
                            //    Log.Message(string.Format("Tau faction {0}: {1} should be friendly to {2}: {3}", f, factionDef, player, playerDef));
                                if (f.GoodwillWith(player) < 0)
                                {
                                    player.TrySetRelationKind(f, FactionRelationKind.Ally, false);
                                }
                            }
                            else if ((factionDef.defName.Contains("OG_Chaos_")) && (playerRace.defName.Contains("OG_Chaos")))
                            {
                            //    Log.Message(string.Format("Chaos faction {0}: {1} should be friendly to {2}: {3}", f, factionDef, player, playerDef));
                                if (f.GoodwillWith(player) < 0)
                                {
                                    player.TrySetRelationKind(f, FactionRelationKind.Neutral, false);
                                }
                            }
                            else
                            {
                            //    Log.Message(string.Format("{0}: {1} should be hostile to {2}: {3}", f, factionDef, player, playerDef));
                                player.TrySetRelationKind(f, FactionRelationKind.Hostile, false);
                            }
                        }
                    }

                }
            }
        }
        // Token: 0x060021DF RID: 8671 RVA: 0x001003C0 File Offset: 0x000FE7C0
        public override void PostGameStart()
        {

            base.PostGameStart();
        }

    }
}
