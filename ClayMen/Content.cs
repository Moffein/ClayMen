
using RoR2.ContentManagement;
using System.Collections;
using UnityEngine;

namespace ClayMen
{
    public class Content : IContentPackProvider
    {
        public static ContentPack content = new ContentPack();

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
            content.bodyPrefabs.Add(new GameObject[] { ClayMen.clayObject });
            content.masterPrefabs.Add(new GameObject[] { ClayMen.clayMaster });
            yield break;
        }
    }
}
