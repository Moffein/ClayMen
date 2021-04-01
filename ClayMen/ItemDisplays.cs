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
        public static List<ItemDisplayRuleSet.NamedRuleGroup> equipmentList;
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

            ItemDisplayRuleSet idrsCommando = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            ItemDisplayRuleSet.NamedRuleGroup[] ersCommando = idrsCommando.GetFieldValue<ItemDisplayRuleSet.NamedRuleGroup[]>("namedEquipmentRuleGroups");
            GameObject bluePrefab = null;
            GameObject redPrefab = null;
            GameObject whitePrefab = null;
            GameObject poisonPrefab = null;
            GameObject ghostPrefab = null;
            for (int i = 0; i < ersCommando.Length; i++)
            {
                if (ersCommando[i].name == "AffixPoison")
                {
                    poisonPrefab = ersCommando[i].displayRuleGroup.rules[0].followerPrefab;
                }
                else if (ersCommando[i].name == "AffixHaunted")
                {
                    ghostPrefab = ersCommando[i].displayRuleGroup.rules[0].followerPrefab;
                }
                else if (ersCommando[i].name == "AffixBlue")
                {
                    bluePrefab = ersCommando[i].displayRuleGroup.rules[0].followerPrefab;
                }
                else if (ersCommando[i].name == "AffixRed")
                {
                    redPrefab = ersCommando[i].displayRuleGroup.rules[0].followerPrefab;
                }
                else if (ersCommando[i].name == "AffixWhite")
                {
                    whitePrefab = ersCommando[i].displayRuleGroup.rules[0].followerPrefab;
                }
                if (false)
                {
                    Debug.Log("\nChildname: " + ersCommando[i].displayRuleGroup.rules[0].childName + "\nScale: " + ersCommando[i].displayRuleGroup.rules[0].localScale + "\nAngles: " + ersCommando[i].displayRuleGroup.rules[0].localAngles + "\nPos: " + ersCommando[i].displayRuleGroup.rules[0].localPos + "\n\n");
                }
            }

            ItemDisplayRuleSet idrsClay = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            equipmentList = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            equipmentList.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixPoison",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = poisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.4f, 0f),
                            localAngles = new Vector3(255f, 0f, 0f),
                            localScale = 0.07f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixHaunted",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ghostPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(260f, 0f, 0f),
                            localScale = 0.07f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixWhite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = whitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.33f, -0.05f),
                            localAngles = new Vector3(260f, 0f, 0f),
                            localScale = 0.035f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixBlue",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = bluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.28f, 0.1f),
                            localAngles = new Vector3(-20f, 0f, 0f),
                            localScale = 0.18f * Vector3.one,
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = bluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.21f, 0.1f),
                            localAngles = new Vector3(-10f, 0f, 0f),
                            localScale = 0.25f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentList.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixRed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = redPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.1f, 0.25f,-0.1f),
                            localAngles = new Vector3(0f, 20f, 0f),
                            localScale = 0.1f * Vector3.one,
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = redPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.1f, 0.25f, -0.1f),
                            localAngles = new Vector3(0f, -20f, 0f),
                            localScale = 0.1f * Vector3.one,
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            ItemDisplayRuleSet.NamedRuleGroup[] equipmentArray = equipmentList.ToArray();
            typeof(ItemDisplayRuleSet).GetField("namedEquipmentRuleGroups", bindingAttr).SetValue(idrsClay, equipmentArray);

            clayObject.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet = idrsClay;
        }
    }
}
