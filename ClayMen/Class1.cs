using BepInEx;
using RoR2;
using UnityEngine;
using RoR2.Navigation;
using R2API;
using System.Collections.Generic;
using RoR2.CharacterAI;
using R2API.Utils;
using BepInEx.Configuration;
using EntityStates.Treebot.UnlockInteractable;

namespace ClayMen
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Moffein.ClayMen", "Clay Men", "1.3.1")]
    [R2API.Utils.R2APISubmoduleDependency(nameof(DirectorAPI), nameof(LanguageAPI), nameof(PrefabAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class ClayMen : BaseUnityPlugin
    {
        private GameObject cm, clayObject;
        public static Transform headTransform;
        ContentPack content;

        public void Start()
        {
            EntityStates.ClaymanMonster.SwipeForward.attackString = "Play_merc_sword_swing";
            EntityStates.ClaymanMonster.SwipeForward.selfForceMagnitude = 1800f;
            EntityStates.ClaymanMonster.SwipeForward.baseDuration = 1f;
            EntityStates.ClaymanMonster.SwipeForward.damageCoefficient = 1.4f;
            EntityStates.ClaymanMonster.Leap.verticalJumpSpeed = 20f;
            EntityStates.ClaymanMonster.Leap.horizontalJumpSpeedCoefficient = 2.3f;
            EntityStates.ClaymanMonster.SpawnState.duration = 3.2f;
        }
        public void Awake()
        {
            content = new ContentPack();
            On.RoR2.ContentManager.SetContentPacks += AddContent;
            clayObject = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/characterbodies/ClayBody"), "MoffeinClayManBody", true);
            content.bodyPrefabs = new GameObject[] { clayObject };
            cm = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/charactermasters/ClaymanMaster"), "MoffeinClayManMaster", true);
            content.masterPrefabs = new GameObject[] { cm };

            CharacterSpawnCard beetleCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscBeetle");
            //CharacterSpawnCard clayBossCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscClayBoss");

            ModifyClayMan();
            ItemDisplays.DisplayRules(clayObject);

            CharacterSpawnCard clayManCSC = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            clayManCSC.name = "cscClayMan";
            clayManCSC.prefab = cm;
            clayManCSC.sendOverNetwork = true;
            clayManCSC.hullSize = HullClassification.Human;
            clayManCSC.nodeGraphType = MapNodeGroup.GraphType.Ground;
            clayManCSC.requiredFlags = NodeFlags.None;
            clayManCSC.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            clayManCSC.directorCreditCost = 16;
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
                spawnDistance = DirectorCore.MonsterSpawnDistance.Close
            };
            DirectorAPI.DirectorCardHolder clayManCard = new DirectorAPI.DirectorCardHolder
            {
                Card = clayManDC,
                MonsterCategory = DirectorAPI.MonsterCategory.BasicMonsters,
                InteractableCategory = DirectorAPI.InteractableCategory.None
            };

            //This causes errors
            /*DirectorAPI.FamilyActions += delegate (List<DirectorAPI.MonsterFamilyHolder> list, DirectorAPI.StageInfo stage)
            {
                foreach (DirectorAPI.MonsterFamilyHolder holder in list)
                {
                    foreach (DirectorCard dC in holder.FamilyChampions)
                    {
                        if (dC.spawnCard == clayBossCSC)
                        {
                            if (!holder.FamilyBasicMonsters.Contains(clayManDC))
                            {
                                holder.FamilyBasicMonsters.Add(clayManDC);
                            }
                        }
                    }
                }
            };*/

            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                bool addClayMan = false;
                switch (stage.stage)
                {
                    case DirectorAPI.Stage.ArtifactReliquary:
                    case DirectorAPI.Stage.VoidCell:
                    case DirectorAPI.Stage.AbandonedAqueduct:
                    case DirectorAPI.Stage.RallypointDelta:
                        foreach (DirectorAPI.DirectorCardHolder dc in list)
                        {
                            if (dc.Card.spawnCard == beetleCSC)
                            {
                                dc.Card.selectionWeight = 1;
                            }
                        }
                        addClayMan = true;
                        break;
                    case DirectorAPI.Stage.ScorchedAcres:
                        addClayMan = true;
                        foreach (DirectorAPI.DirectorCardHolder dc in list)
                        {
                            if (dc.Card.spawnCard == beetleCSC)
                            {
                                dc.Card.selectionWeight = 0;
                            }
                        }
                        break;
                    case DirectorAPI.Stage.Custom:
                        if (stage.CustomStageName == "rootjungle")
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
                }
            };
            On.EntityStates.ClaymanMonster.SpawnState.OnEnter += (orig, self) =>
            {
                orig(self);
                Util.PlayAttackSpeedSound("Play_clayBruiser_attack2_shoot", self.outer.gameObject, 1f);
            };
        }
        private void AddContent(On.RoR2.ContentManager.orig_SetContentPacks orig, List<ContentPack> newContentPacks)
        {
            newContentPacks.Add(content);
            orig(newContentPacks);
        }

        private void ModifyClayMan()
        { 
            
            AISkillDriver clayPrimary = cm.GetComponent<AISkillDriver>();
            clayPrimary.maxDistance = 16f;
            cm.GetComponent<CharacterMaster>().bodyPrefab = clayObject;

            LanguageAPI.Add("CLAY_BODY_NAME", "Clay Man");

            LanguageAPI.Add("CLAY_BODY_LORE", "<style=cMono>\r\n$ Transcribing image... done.\r\n$ Resolving... done.\r\n$ Outputting text strings... done.\r\nComplete!\r\n</style>\r\n\r\nQuick with his sword and quicker with his feet; the agility of these clay 'people' is unexpected with a form so roughly shaped.\n\nWhen faced with one of the few creatures here which I feel some humanity in, my aloneness closes in. Why do they have clay pots on their heads? Could it be protection from this cruel reality, or maybe just to hide the scars from this brutal planet.");
            clayObject.AddComponent<Interactor>().maxInteractionDistance = 3f;
            clayObject.AddComponent<InteractionDriver>();

            ModelLocator clayModelLocator = clayObject.GetComponent<ModelLocator>();
            clayModelLocator.modelTransform.gameObject.layer = LayerIndex.entityPrecise.intVal;
            clayModelLocator.modelTransform.localScale *= 1.2f;
            clayModelLocator.noCorpse = true;

            CharacterDeathBehavior clayCDB = clayObject.GetComponent<CharacterDeathBehavior>();
            clayCDB.deathState = Resources.Load<GameObject>("prefabs/characterbodies/WispBody").GetComponent<CharacterDeathBehavior>().deathState;

            CharacterBody clayCB = clayObject.GetComponent<CharacterBody>();
            clayCB.baseNameToken = "CLAY_BODY_NAME";
            clayCB.baseJumpPower = 22f;
            clayCB.baseMaxHealth = 160f;
            clayCB.levelMaxHealth = clayCB.baseMaxHealth*0.3f;
            clayCB.baseArmor = 0f;
            clayCB.baseDamage = 12f;
            clayCB.levelDamage = clayCB.baseDamage*0.2f;
            clayCB.baseMoveSpeed = 9f;
            clayCB.baseRegen = 0f;
            clayCB.levelRegen = 0f;
            clayCB.bodyFlags = CharacterBody.BodyFlags.ImmuneToGoo;

            //Debug.Log(clayCB.GetComponent<DeathRewards>().logUnlockableName);
            /*UnlockableDef clayLog = ScriptableObject.CreateInstance<UnlockableDef>();
            clayCB.GetComponent<DeathRewards>().logUnlockableDef = clayLog;*/

            SetStateOnHurt claySSoH = clayObject.AddComponent<SetStateOnHurt>();
            claySSoH.canBeFrozen = true;
            claySSoH.canBeStunned = true;
            claySSoH.canBeHitStunned = false;
            claySSoH.hitThreshold = 0.15f;

            SfxLocator claySFX = clayObject.GetComponent<SfxLocator>();
            claySFX.deathSound = "Play_clayboss_M1_explo";
            claySFX.barkSound = "";

            //Ice Fix Credits: SushiDev
            int i = 0;
            EntityStateMachine[] esmr = new EntityStateMachine[2];
            foreach (EntityStateMachine esm in clayObject.GetComponentsInChildren<EntityStateMachine>())
            {
                switch (esm.customName)
                {
                    case "Body":
                        claySSoH.targetStateMachine = esm;
                        break;
                    default:
                        if (i < 2)
                        {
                            esmr[i] = esm;
                        }
                        i++;
                        break;
                }
            }

            #region hitbox
            Component[] clayComponents = clayObject.GetComponentsInChildren<Transform>();
            Transform clayTransform = null;
            Transform clayHeadTransform = null;
            Transform claySwordHitboxTransform = null;
            foreach (Transform t in clayComponents)
            {
                if (t.name == "chest")
                {
                    clayTransform = t;
                }
                else if (t.name == "head")
                {
                    clayHeadTransform = t;
                }
                else if (t.name == "Hitbox")
                {
                    claySwordHitboxTransform = t;
                }
                if (clayTransform != null && clayHeadTransform != null && claySwordHitboxTransform != null)
                {
                    break;
                }
            }

            ItemDisplays.headTransform = clayHeadTransform;

            HurtBoxGroup clayHurtBoxGroup = clayModelLocator.modelTransform.gameObject.AddComponent<HurtBoxGroup>();
            claySwordHitboxTransform.localScale *= 2.4f; //2.8 -> 2.4

            #region chest
            clayTransform.gameObject.layer = LayerIndex.entityPrecise.intVal;
            CapsuleCollider clayCollider = clayTransform.gameObject.AddComponent<CapsuleCollider>();
            clayCollider.center -= new Vector3(0, 0.6f, 0);
            clayCollider.height *= 0.25f;
            clayCollider.radius *= 1.16f;
            HurtBox clayHurtBox = clayTransform.gameObject.AddComponent<HurtBox>();
            clayHurtBox.isBullseye = true;
            clayHurtBox.healthComponent = clayObject.GetComponent<HealthComponent>();
            clayHurtBox.damageModifier = HurtBox.DamageModifier.Normal;
            clayHurtBox.hurtBoxGroup = clayHurtBoxGroup;
            clayHurtBox.indexInGroup = 0;
            //clayHurtBox.name = "ChestHurtbox";
            #endregion

            #region head


            clayHeadTransform.gameObject.layer = LayerIndex.entityPrecise.intVal; 
            CapsuleCollider clayHeadCollider = clayHeadTransform.gameObject.AddComponent<CapsuleCollider>();
            clayHeadCollider.height *= 0.4f;
            clayHeadCollider.radius *= 0.3f;
            clayHeadCollider.center += new Vector3(0, 0.2f, 0);
            HurtBox clayHeadHurtBox = clayHeadTransform.gameObject.AddComponent<HurtBox>();
            clayHeadHurtBox.isBullseye = false;
            clayHeadHurtBox.healthComponent = clayObject.GetComponent<HealthComponent>();
            clayHeadHurtBox.damageModifier = HurtBox.DamageModifier.SniperTarget;
            clayHeadHurtBox.hurtBoxGroup = clayHurtBoxGroup;
            clayHeadHurtBox.indexInGroup = 1;
            //clayHeadHurtBox.name = "HeadHurtbox";

            #endregion

            HurtBox[] clayHurtBoxArray = new HurtBox[]
            {
                clayHurtBox, clayHeadHurtBox
            };

            clayHurtBoxGroup.bullseyeCount = 1;
            clayHurtBoxGroup.hurtBoxes = clayHurtBoxArray;
            clayHurtBoxGroup.mainHurtBox = clayHurtBox;

            #endregion

            EntityLocator clayLocator = clayObject.AddComponent<EntityLocator>();
            clayLocator.entity = clayObject;
        }
    }
}
