using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class LoginPlayerToPlayFab : MonoBehaviour
{

    public AssetReference assetReference;
    
    // Start is called before the first frame update
    void Start()
    {
        LoginToPlayFab();
        Debug.Log("ReadyPlayerOne log in");
        assetReference.InstantiateAsync();
    }
    

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator LoginToPlayFab()
    {
        var loginSuccessful = false;
        var request = new LoginWithCustomIDRequest { CustomId = "ReadyPlayerOne", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, result => loginSuccessful = true,
          error => error.GenerateErrorReport());
        return new WaitUntil(() => loginSuccessful);
    }

    private IEnumerator InitializeAddressables()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new AssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageHashProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageJsonAssetProvider());
        return Addressables.InitializeAsync();
    }

}
