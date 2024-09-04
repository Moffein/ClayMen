using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using UnityEngine;
using R2API.Utils;

namespace ClayMen
{
    class ItemDisplays
    {
        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();
        public static List<ItemDisplayRuleSet.KeyAssetRuleGroup> equipmentList;
        public static Transform headTransform;
        public static void DisplayRules(GameObject clayObject)
        {
            ChildLocator cl = clayObject.GetComponentInChildren<ChildLocator>();
            Array.Resize(ref cl.transformPairs, cl.transformPairs.Length + 1);
            cl.transformPairs[cl.transformPairs.Length - 1] = new ChildLocator.NameTransformPair
            {
                name = "Head",
                transform = headTransform
            };

            PopulateDisplays();

            ItemDisplayRuleSet idrsClay = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            equipmentList = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3(0f, 0.4f, 0f),
                            localAngles = new Vector3(255f, 0f, 0f),
                            localScale = 0.07f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(260f, 0f, 0f),
                            localScale = 0.07f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3(0f, 0.33f, -0.05f),
                            localAngles = new Vector3(260f, 0f, 0f),
                            localScale = 0.035f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0f, 0.28f, 0.1f),
                            localAngles = new Vector3(-20f, 0f, 0f),
                            localScale = 0.18f * Vector3.one,
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0f, 0.21f, 0.1f),
                            localAngles = new Vector3(-10f, 0f, 0f),
                            localScale = 0.25f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.1f, 0.25f,-0.1f),
                            localAngles = new Vector3(0f, 20f, 0f),
                            localScale =  new Vector3(-0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0.1f, 0.25f, -0.1f),
                            localAngles = new Vector3(0f, -20f, 0f),
                            localScale =  new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Elites.Earth.eliteEquipmentDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = LoadDisplay("DisplayEliteMendingAntlers"),
                                childName = "Head",
                                localPos = new Vector3(0f, 0.25f, 0f),
                                localAngles = Vector3.zero,
                                localScale = Vector3.one,
                                limbMask = LimbFlags.None
                            }
                        }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Elites.Void.eliteEquipmentDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = LoadDisplay("DisplayAffixVoid"),
                                childName = "Head",
                                localPos = new Vector3(0f, 0.14f, 0.15f),
                                localAngles = new Vector3(90f, 0f, 0f),
                                localScale = 0.2f * Vector3.one,
                                limbMask = LimbFlags.None
                            }
                        }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC2Content.Elites.Aurelionite.eliteEquipmentDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = LoadDisplay("DisplayEliteAurelioniteEquipment"),
                                childName = "Head",
                                localPos = new Vector3(0F, 0.28F, 0.14F),
                                localAngles = new Vector3(0F, 0F, 0F),
                                localScale = new Vector3(0.4F, 0.4F, 0.4F),
                                limbMask = LimbFlags.None
                            }
                        }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC2Content.Elites.Bead.eliteEquipmentDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = LoadDisplay("DisplayEliteBeadSpike"),
                                childName = "Head",
                                localPos = new Vector3(0F, 0.3F, -0.075F),
                                localAngles = new Vector3(326F, 0F, 0F),
                                localScale = new Vector3(0.025F, 0.025F, 0.025F),
                                limbMask = LimbFlags.None
                            }
                        }
                }
            });

            idrsClay.keyAssetRuleGroups = equipmentList.ToArray();
            CharacterModel characterModel = clayObject.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>();
            characterModel.itemDisplayRuleSet = idrsClay;
            characterModel.itemDisplayRuleSet.GenerateRuntimeValues();

            itemDisplayPrefabs.Clear();
        }

        internal static void PopulateDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;
                if (rules != null)
                {
                    for (int j = 0; j < rules.Length; j++)
                    {
                        GameObject followerPrefab = rules[j].followerPrefab;
                        if (followerPrefab)
                        {
                            string name = followerPrefab.name;
                            string key = (name != null) ? name.ToLower() : null;
                            if (!itemDisplayPrefabs.ContainsKey(key))
                            {
                                itemDisplayPrefabs[key] = followerPrefab;
                            }
                        }
                    }
                }
            }
        }

        public static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }
            return null;
        }
    }
}
