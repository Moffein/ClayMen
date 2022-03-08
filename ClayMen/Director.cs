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
        }
    }
}
