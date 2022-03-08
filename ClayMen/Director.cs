using UnityEngine;
using System.Collections.Generic;
using RoR2;
using R2API;
using RoR2.Navigation;

namespace ClayMen
{
    public class Director
    {
        public static void Setup()
        {


            CharacterSpawnCard beetleCSC = LegacyResourcesAPI.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscBeetle");
            CharacterSpawnCard impCSC = LegacyResourcesAPI.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscImp");
            CharacterSpawnCard wispCSC = LegacyResourcesAPI.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscLesserWisp");
            //CharacterSpawnCard clayBossCSC = LegacyResourcesAPI.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscClayBoss");

            CharacterSpawnCard clayManCSC = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            clayManCSC.name = "cscClayMan";
            clayManCSC.prefab = Content.ClayManMaster;
            clayManCSC.sendOverNetwork = true;
            clayManCSC.hullSize = HullClassification.Human;
            clayManCSC.nodeGraphType = MapNodeGroup.GraphType.Ground;
            clayManCSC.requiredFlags = NodeFlags.None;
            clayManCSC.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            clayManCSC.directorCreditCost = 28;
            clayManCSC.occupyPosition = false;
            clayManCSC.loadout = new SerializableLoadout();
            clayManCSC.noElites = false;
            clayManCSC.forbiddenAsBoss = false;

            DirectorCard clayManDC = new DirectorCard
            {
                spawnCard = clayManCSC,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = 0,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };
            DirectorAPI.DirectorCardHolder clayManCard = new DirectorAPI.DirectorCardHolder
            {
                Card = clayManDC,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
                InteractableCategory = DirectorAPI.InteractableCategory.None
            };
            Content.ClayManCard = clayManCard;

            #region addcard


            if (ClayMen.artifact)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.ArtifactReliquary);
            }
            if (ClayMen.voidfields)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.VoidCell);
            }

            if (ClayMen.titanic)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.TitanicPlains);
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itgolemplains");
            }
            if (ClayMen.roost)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.DistantRoost);
            }
            if (ClayMen.wetland)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.WetlandAspect);
            }
            if (ClayMen.aqueduct)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.AbandonedAqueduct);
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itgoolake");
            }
            if (ClayMen.rallypoint)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.RallypointDelta);
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itfrozenwall");
            }
            if (ClayMen.scorched)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.ScorchedAcres);
                if (ClayMen.scorchedImps)
                {
                    DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscImp", DirectorAPI.Stage.ScorchedAcres);
                }
                if (ClayMen.scorchedBeetles)
                {
                    DirectorAPI.Helpers.RemoveExistingMonsterFromStage("cscBeetle", DirectorAPI.Stage.ScorchedAcres);
                }
            }
            if (ClayMen.stadia)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.SunderedGrove);
            }
            if (ClayMen.abyss)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.AbyssalDepths);
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itdampcave");
            }
            if (ClayMen.sirens)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.SirensCall);
            }
            if (ClayMen.meadow)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.SkyMeadow);
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itskymeadow");
            }
            if (ClayMen.snowyForest)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "snowyforest");
            }
            if (ClayMen.aphSanct)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "ancientloft");
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "itancientloft");
            }
            if (ClayMen.sulfur)
            {
                DirectorAPI.Helpers.AddNewMonsterToStage(clayManCard.Card, clayManCard.MonsterCategory, DirectorAPI.Stage.Custom, "sulfurpools");
            }
            #endregion

            #region old
            /*
            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                bool addClayMan = false;
                bool removeBeetles = false;
                bool removeImps = false;
                bool removeWisps = false;
                switch (stage.stage)
                {
                    case DirectorAPI.Stage.ArtifactReliquary:
                        if (ClayMen.artifact)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.VoidCell:
                        if (ClayMen.voidfields)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.TitanicPlains:
                        if (ClayMen.titanic)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.DistantRoost:
                        if (ClayMen.roost)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.WetlandAspect:
                        if (ClayMen.wetland)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.AbandonedAqueduct:
                        if (ClayMen.aqueduct)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.RallypointDelta:
                        if (ClayMen.rallypoint)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.ScorchedAcres:
                        if (ClayMen.scorched)
                        {
                            addClayMan = true;
                            removeImps = ClayMen.scorchedImps;
                            removeBeetles = ClayMen.scorchedBeetles;
                        }
                        break;
                    case DirectorAPI.Stage.SunderedGrove:
                        if (ClayMen.stadia)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.AbyssalDepths:
                        if (ClayMen.abyss)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.SirensCall:
                        if (ClayMen.sirens)
                        {
                            addClayMan = true;
                        }
                        break;
                    case DirectorAPI.Stage.SkyMeadow:
                        if (ClayMen.meadow)
                        {
                            addClayMan = true;
                        }
                        break;
                    default:
                        break;
                }

                //Todo: Replace when R2API updates
                if (!addClayMan)
                {
                    switch (stage.CustomStageName)
                    {
                        //Simulacrum
                        case "itgolemplains":
                            addClayMan = ClayMen.titanic;
                            break;
                        case "itdampcave":
                            addClayMan = ClayMen.abyss;
                            break;
                        case "itancientloft":
                            addClayMan = ClayMen.aphSanct;
                            break;
                        case "itfrozenwall":
                            addClayMan = ClayMen.rallypoint;
                            break;
                        case "itgoolake":
                            addClayMan = ClayMen.aqueduct;
                            break;
                        case "itskymeadow":
                            addClayMan = ClayMen.meadow;
                            break;

                        //DLC1
                        case "ancientloft":
                            addClayMan = ClayMen.aphSanct;
                            break;
                        case "sulfurpools":
                            addClayMan = ClayMen.sulfur;
                            break;
                        case "snowyforest":
                            addClayMan = ClayMen.snowyForest;
                            break;
                    }
                }

                if (addClayMan)
                {
                    if (!list.Contains(clayManCard)) list.Add(clayManCard);

                    List<DirectorAPI.DirectorCardHolder> toRemove = new List<DirectorAPI.DirectorCardHolder>();

                    foreach (DirectorAPI.DirectorCardHolder dc in list)
                    {
                        if ((removeBeetles && dc.Card.spawnCard == beetleCSC)
                        || (removeImps && dc.Card.spawnCard == impCSC)
                        || (removeWisps && dc.Card.spawnCard == wispCSC))
                        {
                            toRemove.Add(dc);
                        }
                    }

                    foreach(DirectorAPI.DirectorCardHolder dc in toRemove)
                    {
                        list.Remove(dc);
                    }
                    toRemove.Clear();
                }
            };*/
            #endregion
        }
    }
}
