using BepInEx;
using RoR2;
using UnityEngine;
using RoR2.Navigation;
using R2API;
using System.Collections.Generic;
using RoR2.CharacterAI;
using R2API.Utils;
using BepInEx.Configuration;
using System;
using RoR2.ContentManagement;
using System.Collections;
using EntityStates;
using System.Linq;

namespace ClayMen
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Moffein.ClayMen", "Clay Men", "1.5.0")]
    [R2API.Utils.R2APISubmoduleDependency(nameof(DirectorAPI), nameof(PrefabAPI))]//, nameof(DamageAPI)
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class ClayMenPlugin : BaseUnityPlugin
    {
        public static Transform headTransform;
        public static PluginInfo pluginInfo;
        public static List<StageSpawnInfo> StageList = new List<StageSpawnInfo>();

        public void LateSetup()
        {
            ItemDisplays.DisplayRules(ClayMenContent.ClayManObject);
        }

        public void ReadConfig()
        {
            string stages = base.Config.Bind<string>(new ConfigDefinition("Spawns", "Stage List"), "goolake, ancientloft, wispgraveyard, sulfurpools, arena, itgoolake, itancientloft", new ConfigDescription("What stages the monster will show up on. Add a '- loop' after the stagename to make it only spawn after looping. List of stage names can be found at https://github.com/risk-of-thunder/R2Wiki/wiki/List-of-scene-names")).Value;
            string impRemoveStages = base.Config.Bind<string>(new ConfigDefinition("Spawns", "Remove Imps"), "wispgraveyard", new ConfigDescription("Remove Imps from these stages to prevent role overlap.")).Value;

            //parse stage
            stages = new string(stages.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitStages = stages.Split(',');
            foreach (string str in splitStages)
            {
                string[] current = str.Split('-');

                string name = current[0];
                int minStages = 0;
                if (current.Length > 1)
                {
                    minStages = 5;
                }

                StageList.Add(new StageSpawnInfo(name, minStages));
            }

            //parse removeImps
            impRemoveStages = new string(impRemoveStages.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitimpRemoveStages = stages.Split(',');
            foreach (string str in splitimpRemoveStages)
            {
                string[] current = str.Split('-');  //in case people try to use the Stage List format

                string name = current[0];

                SceneDef sd = ScriptableObject.CreateInstance<SceneDef>();
                sd.baseSceneNameOverride = name;

                DirectorAPI.Helpers.RemoveExistingMonsterFromStage(DirectorAPI.Helpers.MonsterNames.Imp, DirectorAPI.GetStageEnumFromSceneDef(sd), name);
            }
        }

        private void SetEntityStateFieldValues()
        {
            SneedUtils.SneedUtils.SetEntityStateField("EntityStates.ClaymanMonster.Leap", "verticalJumpSpeed", "20");
            SneedUtils.SneedUtils.SetEntityStateField("EntityStates.ClaymanMonster.Leap", "horizontalJumpSpeedCoefficient", "2.3");

            SneedUtils.SneedUtils.SetEntityStateField("EntityStates.ClaymanMonster.SpawnState", "duration", "3.2");
        }

        public void Awake()
        {
            pluginInfo = Info;
            new LanguageTokens();
            ReadConfig();

            ClayMenContent.ClayManObject = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/ClayBody"), "MoffeinClayManBody", true);
            ClayMenContent.ClayManMaster = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("prefabs/charactermasters/ClaymanMaster"), "MoffeinClayManMaster", true);

            SetEntityStateFieldValues();
            Prefab.Modify(ClayMenContent.ClayManObject);
            ModifyAI();

            RoR2Application.onLoad += LateSetup;
            Director.Setup();
            ModifySkills(ClayMenContent.ClayManObject);
            //SetupDamageTypes();

            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
        }

        /*private void SetupDamageTypes()
        {
            Content.ClayGooClayMan = DamageAPI.ReserveDamageType();
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                orig(self, damageInfo);
                if (self.alive && !damageInfo.rejected)
                {
                    if (damageInfo.HasModdedDamageType(Content.ClayGooClayMan))
                    {
                        self.body.AddTimedBuff(RoR2Content.Buffs.ClayGoo.buffIndex, 2f * damageInfo.procCoefficient);
                    }
                }
            };
        }*/

        private void ModifySkills(GameObject bodyObject)
        {
            SkillLocator sk = bodyObject.GetComponent<SkillLocator>();
            sk.primary.skillFamily.variants[0].skillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.MoffeinClayMan.SwipeForwardTar));
            sk.primary.skillFamily.variants[0].skillDef.isCombatSkill = true;
            sk.primary.skillFamily.variants[0].skillDef.baseRechargeInterval = 1.4f;
        }

        private void ModifyAI()
        {
            AISkillDriver clayPrimary = ClayMenContent.ClayManMaster.GetComponent<AISkillDriver>();
            clayPrimary.maxDistance = 12f;
            ClayMenContent.ClayManMaster.GetComponent<CharacterMaster>().bodyPrefab = ClayMenContent.ClayManObject;
        }

        private void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(new ClayMenContent());
        }
    }
    public class StageSpawnInfo
    {
        private string stageName;
        private int minStages;

        public StageSpawnInfo(string stageName, int minStages)
        {
            this.stageName = stageName;
            this.minStages = minStages;
        }

        public string GetStageName() { return stageName; }
        public int GetMinStages() { return minStages; }
    }
}
