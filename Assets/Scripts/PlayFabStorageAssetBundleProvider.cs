using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class PlayFabStorageAssetBundleProvider : AssetBundleProvider
{
    public override void Provide(ProvideHandle provideHandle)
    {
        var addressableId = provideHandle.Location.InternalId.Replace("playfab://", "");
        PlayFabClientAPI.GetContentDownloadUrl(
            new GetContentDownloadUrlRequest() { Key = addressableId, ThruCDN = false },
            result =>
            {
                var dependenciesList = provideHandle.Location.Dependencies;
                var dependenciesArray = provideHandle.Location.Dependencies == null ? new IResourceLocation[0] : new IResourceLocation[dependenciesList.Count];
                dependenciesList?.CopyTo(dependenciesArray, 0);
                var resourceLocation = new ResourceLocationBase(result.URL, result.URL, typeof(AssetBundleProvider).FullName, typeof(IResourceLocator), dependenciesArray)
                {
                    Data = provideHandle.Location.Data,
                    PrimaryKey = provideHandle.Location.PrimaryKey
                };
                provideHandle.ResourceManager.ProvideResource<IAssetBundleResource>(resourceLocation).Completed += handle =>
                {
                    var contents = handle.Result;
                    provideHandle.Complete(contents, true, handle.OperationException);
                };
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}