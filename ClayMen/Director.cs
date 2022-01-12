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
            //CharacterSpawnCard clayBossCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscClayBoss");

            CharacterSpawnCard clayManCSC = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            clayManCSC.name = "cscClayMan";
            clayManCSC.prefab = ClayMen.clayMaster;
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

            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                bool addClayMan = false;
                bool removeBeetles = false;
                bool removeImps = false;
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
                            removeBeetles = ClayMen.titanicBeetles;
                        }
                        break;
                    case DirectorAPI.Stage.DistantRoost:
                        if (ClayMen.roost)
                        {
                            addClayMan = true;
                            removeBeetles = ClayMen.roostBeetles;
                        }
                        break;
                    case DirectorAPI.Stage.WetlandAspect:
                        if (ClayMen.wetland)
                        {
                            addClayMan = true;
                            removeBeetles = ClayMen.wetlandBeetles;
                        }
                        break;
                    case DirectorAPI.Stage.AbandonedAqueduct:
                        if (ClayMen.aqueduct)
                        {
                            addClayMan = true;
                            removeBeetles = ClayMen.aqueductBeetles;
                        }
                        break;
                    case DirectorAPI.Stage.RallypointDelta:
                        if (ClayMen.rallypoint)
                        {
                            addClayMan = true;
                            removeImps = ClayMen.rallypointImps;
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
                            removeImps = ClayMen.abyssImps;
                        }
                        break;
                    case DirectorAPI.Stage.SirensCall:
                        if (ClayMen.sirens)
                        {
                            addClayMan = true;
                            removeBeetles = ClayMen.sirensBeetles;
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
                    if (!list.Contains(clayManCard))
                    {
                        list.Add(clayManCard);
                    }

                    foreach (DirectorAPI.DirectorCardHolder dc in list)
                    {
                        if (dc.Card.spawnCard == beetleCSC)
                        {
                            dc.Card.selectionWeight = removeBeetles ? 0 : 1;
                        }
                        else if (dc.Card.spawnCard == impCSC)
                        {
                            dc.Card.selectionWeight = removeImps ? 0 : 1;
                        }
                    }
                }
            };
        }
    }
}
