
using System;
using RoR2.ContentManagement;
using System.Collections;
using UnityEngine;
using R2API;

namespace ClayMen
{
    public class Content : IContentPackProvider
    {
        public static ContentPack content = new ContentPack();
        public static GameObject ClayManObject;
        public static GameObject ClayManMaster;
        public static DamageAPI.ModdedDamageType ClayGooClayMan;

        public string identifier => "MoffeinClayMen.content";

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(content, args.output);
            yield break;
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            content.entityStateTypes.Add(new Type[] { typeof(EntityStates.MoffeinClayMan.SwipeForwardTar), typeof(EntityStates.MoffeinClayMan.DeathState) });
            content.bodyPrefabs.Add(new GameObject[] { Content.ClayManObject });
            content.masterPrefabs.Add(new GameObject[] { Content.ClayManMaster });
            yield break;
        }
    }
}
