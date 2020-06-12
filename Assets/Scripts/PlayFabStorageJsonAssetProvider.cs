using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class PlayFabStorageJsonAssetProvider : JsonAssetProvider
{
    public override string ProviderId => typeof(JsonAssetProvider).FullName;

    public override void Provide(ProvideHandle provideHandle)
    {
        if (provideHandle.Location.InternalId.StartsWith("playfab://") == false)
        {
            base.Provide(provideHandle);
            return;
        }

        var addressableId = provideHandle.Location.InternalId.Replace("playfab://", "");
        PlayFabClientAPI.GetContentDownloadUrl(
            new GetContentDownloadUrlRequest() { Key = addressableId, ThruCDN = false },
            result =>
            {
                Assert.IsTrue(provideHandle.Location.ResourceType == typeof(ContentCatalogData), "Only catalogs supported");
                var resourceLocation = new ResourceLocationBase(result.URL, result.URL, typeof(JsonAssetProvider).FullName, typeof(string));
                provideHandle.ResourceManager.ProvideResource<ContentCatalogData>(resourceLocation).Completed += handle =>
                {
                    var contents = handle.Result;
                    provideHandle.Complete(contents, true, handle.OperationException);
                };
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}