
using System;
using RoR2.ContentManagement;
using System.Collections;
using UnityEngine;
using R2API;
using RoR2;

namespace ClayMen
{
    public class ClayMenContent : IContentPackProvider
    {
        public static ContentPack content = new ContentPack();
        public static GameObject ClayManObject;
        public static GameObject ClayManMaster;
        public static UnlockableDef ClayManLogbookUnlockable;
        //public static DamageAPI.ModdedDamageType ClayGooClayMan;
        public static DirectorAPI.DirectorCardHolder ClayManCard;
        public static DirectorAPI.DirectorCardHolder ClayManLoopCard;

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
            content.bodyPrefabs.Add(new GameObject[] { ClayMenContent.ClayManObject });
            content.masterPrefabs.Add(new GameObject[] { ClayMenContent.ClayManMaster });
            content.unlockableDefs.Add(new UnlockableDef[] { ClayManLogbookUnlockable });
            yield break;
        }
    }
}
