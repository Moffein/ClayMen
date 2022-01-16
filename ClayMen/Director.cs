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


            CharacterSpawnCard beetleCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscBeetle");
            CharacterSpawnCard impCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscImp");
            CharacterSpawnCard wispCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscLesserWisp");
            //CharacterSpawnCard clayBossCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscClayBoss");

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
                allowAmbushSpawn = true,
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
                            removeBeetles = ClayMen.aqueductBeetles;
                            removeWisps = ClayMen.aqueductWisps;
                        }
                        break;
                    case DirectorAPI.Stage.RallypointDelta:
                        if (ClayMen.rallypoint)
                        {
                            addClayMan = true;
                            removeImps = ClayMen.rallypointImps;
                            removeWisps = ClayMen.rallypointWisps;
                        }
                        break;
                    case DirectorAPI.Stage.ScorchedAcres:
                        if (ClayMen.scorched)
                        {
                            addClayMan = true;
                            removeImps = ClayMen.scorchedImps;
                            removeBeetles = ClayMen.scorchedBeetles;
                            removeWisps = ClayMen.scorchedWisps;
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
            };
        }
    }
}
