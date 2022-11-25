using R2API;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace ClayMen
{
    public class Prefab
    {
        public static void Modify(GameObject clayObject)
        {
            clayObject.AddComponent<Interactor>().maxInteractionDistance = 3f;
            clayObject.AddComponent<InteractionDriver>();

            ModelLocator clayModelLocator = clayObject.GetComponent<ModelLocator>();
            clayModelLocator.modelTransform.gameObject.layer = LayerIndex.entityPrecise.intVal;
            clayModelLocator.modelTransform.localScale *= 1.4f;
            clayModelLocator.noCorpse = true;

            CharacterDeathBehavior clayCDB = clayObject.GetComponent<CharacterDeathBehavior>();
            clayCDB.deathState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.MoffeinClayMan.DeathState));

            CharacterBody clayCB = clayObject.GetComponent<CharacterBody>();
            clayCB.baseNameToken = "MOFFEIN_CLAY_BODY_NAME";
            clayCB.baseJumpPower = 22f;
            clayCB.baseMaxHealth = 140f;
            clayCB.levelMaxHealth = clayCB.baseMaxHealth * 0.3f;
            clayCB.baseArmor = 0f;
            clayCB.baseDamage = 12f;
            clayCB.levelDamage = clayCB.baseDamage * 0.2f;
            clayCB.baseMoveSpeed = 10f;
            clayCB.baseRegen = 0f;
            clayCB.levelRegen = 0f;
            clayCB.bodyFlags = CharacterBody.BodyFlags.ImmuneToGoo;

            SfxLocator claySFX = clayObject.GetComponent<SfxLocator>();
            claySFX.deathSound = "Play_clayboss_M1_explo";
            claySFX.barkSound = "";

            EntityLocator clayLocator = clayObject.AddComponent<EntityLocator>();
            clayLocator.entity = clayObject;

            FixHitbox(clayObject, clayModelLocator);
            AddSSoH(clayObject);

            DeathRewards dr = clayObject.GetComponent<DeathRewards>();
            ClayMenContent.ClayManLogbookUnlockable = ScriptableObject.CreateInstance<UnlockableDef>();
            ClayMenContent.ClayManLogbookUnlockable.nameToken = "UNLOCKABLE_LOG_MOFFEIN_CLAY_BODY";
            dr.logUnlockableDef = ClayMenContent.ClayManLogbookUnlockable;

            On.EntityStates.ClaymanMonster.SpawnState.OnEnter += (orig, self) =>
            {
                orig(self);
                Util.PlayAttackSpeedSound("Play_clayBruiser_attack2_shoot", self.outer.gameObject, 1f);
            };
        }

        public static void FixHitbox(GameObject enemyObject, ModelLocator modelLocator)
        {
            Component[] clayComponents = enemyObject.GetComponentsInChildren<Transform>();
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

            //Tweak Sword Hitbox
            HurtBoxGroup clayHurtBoxGroup = modelLocator.modelTransform.gameObject.AddComponent<HurtBoxGroup>();
            claySwordHitboxTransform.localScale = new Vector3(1f, 3.4f, 1.6f);  //X is vertical range

            //Move swing visual to match it
            ChildLocator cl = modelLocator.modelTransform.GetComponent<ChildLocator>();
            Transform swingCenter = cl.FindChild("SwingCenter");
            swingCenter.localPosition += new Vector3(0f, 0.7f, 1.8f);

            //When DisableHurtboxes disables chest, it disables a lot of other things (Sword + Shield), which breaks Ghosts.
            #region chest
            /*clayTransform.gameObject.layer = LayerIndex.entityPrecise.intVal;
            CapsuleCollider clayCollider = clayTransform.gameObject.AddComponent<CapsuleCollider>();
            clayCollider.center = new Vector3(0, -0.3f, 0);
            clayCollider.height = 1.5f;
            clayCollider.radius = 0.35f;


            HurtBox clayHurtBox = clayTransform.gameObject.AddComponent<HurtBox>();
            clayHurtBox.isBullseye = true;
            clayHurtBox.healthComponent = enemyObject.GetComponent<HealthComponent>();
            clayHurtBox.damageModifier = HurtBox.DamageModifier.Normal;
            clayHurtBox.hurtBoxGroup = clayHurtBoxGroup;
            clayHurtBox.indexInGroup = 0;
            //clayHurtBox.name = "ChestHurtbox";*/
            #endregion

            #region head
            clayHeadTransform.gameObject.layer = LayerIndex.entityPrecise.intVal;
            CapsuleCollider clayHeadCollider = clayHeadTransform.gameObject.AddComponent<CapsuleCollider>();

            //Dimensions for containing the head only
            /*clayHeadCollider.height = 0.7f;
            clayHeadCollider.radius = 0.25f;
            clayHeadCollider.center = new Vector3(0, 0.2f, 0);*/

            //Dimensions for containing the entire body
            clayHeadCollider.height = 2f;
            clayHeadCollider.radius = 0.4f;
            clayHeadCollider.center = new Vector3(0, -0.5f, 0);

            HurtBox clayHeadHurtBox = clayHeadTransform.gameObject.AddComponent<HurtBox>();
            clayHeadHurtBox.isBullseye = true;
            clayHeadHurtBox.healthComponent = enemyObject.GetComponent<HealthComponent>();
            clayHeadHurtBox.damageModifier = HurtBox.DamageModifier.Normal;
            clayHeadHurtBox.hurtBoxGroup = clayHurtBoxGroup;
            clayHeadHurtBox.indexInGroup = 0;
            clayHeadHurtBox.isSniperTarget = true;
            //clayHeadHurtBox.name = "HeadHurtbox";
            #endregion

            HurtBox[] clayHurtBoxArray = new HurtBox[]
            {
                clayHeadHurtBox
            };

            clayHurtBoxGroup.mainHurtBox = clayHeadHurtBox;
            clayHurtBoxGroup.hurtBoxes = clayHurtBoxArray;
        }

        private static void AddSSoH(GameObject enemyObject)
        {
            EntityStateMachine body = null;
            EntityStateMachine weapon = null;
            EntityStateMachine[] stateMachines = enemyObject.GetComponents<EntityStateMachine>();
            foreach (EntityStateMachine esm in stateMachines)
            {
                switch (esm.customName)
                {
                    case "Body":
                        body = esm;
                        break;
                    case "Weapon":
                        weapon = esm;
                        break;
                    default:
                        break;
                }
            }

            SetStateOnHurt ssoh = enemyObject.AddComponent<SetStateOnHurt>();
            ssoh.canBeFrozen = true;
            ssoh.canBeStunned = true;
            ssoh.canBeHitStunned = true;
            ssoh.hitThreshold = 0.35f;
            ssoh.targetStateMachine = body;
            ssoh.idleStateMachine = new EntityStateMachine[] { weapon };
            ssoh.hurtState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.HurtState));
        }
    }
}
