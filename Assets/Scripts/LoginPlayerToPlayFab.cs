using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class LoginPlayerToPlayFab : MonoBehaviour
{

    public AssetReference assetReference;
    public TextMeshPro textMesh;



    // Start is called before the first frame update
    void Start()
    {
        LoginToPlayFab();
        Debug.Log("ReadyPlayerOne logged in");
        textMesh = GameObject.FindObjectOfType<TextMeshPro>();
        textMesh.text = "ReadyPlayerOne Logged In";
        assetReference.InstantiateAsync();
        textMesh = GameObject.FindObjectOfType<TextMeshPro>();
        textMesh.text = "Asset loaded from Github";

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
