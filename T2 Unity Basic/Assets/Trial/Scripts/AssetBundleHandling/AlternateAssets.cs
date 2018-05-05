using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AlternateAssets : MonoBehaviour {

    public static AlternateAssets instance;

    AssetBundle myLoadedAssetBundle;

    private void Awake()
    {
        instance = this;
        myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "maincharacters"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
    }

    public static GameObject GetPrefab(string _obj)
    {
        return instance.myLoadedAssetBundle.LoadAsset(_obj) as GameObject;

    }

    GameObject GetGameObject (string _obj)
    {
        return myLoadedAssetBundle.LoadAsset(_obj) as GameObject;
    }
}

