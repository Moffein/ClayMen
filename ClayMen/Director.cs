using UnityEngine;
using System.Collections.Generic;
using RoR2;
using R2API;
using RoR2.Navigation;
using UnityEngine.AddressableAssets;

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
            clayManCSC.prefab = ClayMenContent.ClayManMaster;
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
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
            };

            DirectorCard clayManLoopDC = new DirectorCard
            {
                spawnCard = clayManCSC,
                selectionWeight = 1,
                preventOverhead = false,
                minimumStageCompletions = 5,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };
            DirectorAPI.DirectorCardHolder clayManLoopCard = new DirectorAPI.DirectorCardHolder
            {
                Card = clayManLoopDC,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
            };

            ClayMenContent.ClayManCard = clayManCard;
            ClayMenContent.ClayManLoopCard = clayManLoopCard;

            DirectorCardCategorySelection dissonanceSpawns = Addressables.LoadAssetAsync<DirectorCardCategorySelection>("RoR2/Base/MixEnemy/dccsMixEnemy.asset").WaitForCompletion();
            int categoryIndex = FindCategoryIndexByName(dissonanceSpawns, "Basic Monsters");
            if (categoryIndex >= 0)
            {
                dissonanceSpawns.AddCard(categoryIndex, clayManDC);
            }

            foreach (StageSpawnInfo ssi in ClayMenPlugin.StageList)
            {
                DirectorAPI.DirectorCardHolder toAdd = ssi.GetMinStages() == 0 ? ClayMenContent.ClayManCard : ClayMenContent.ClayManLoopCard;

                SceneDef sd = ScriptableObject.CreateInstance<SceneDef>();
                sd.baseSceneNameOverride = ssi.GetStageName();

                DirectorAPI.Helpers.AddNewMonsterToStage(toAdd, false, DirectorAPI.GetStageEnumFromSceneDef(sd), ssi.GetStageName());
            }
        }

        //Minibosses
        //Basic Monsters
        //Champions
        public static int FindCategoryIndexByName(DirectorCardCategorySelection dcs, string categoryName)
        {
            for (int i = 0; i < dcs.categories.Length; i++)
            {
                if (string.CompareOrdinal(dcs.categories[i].name, categoryName) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
