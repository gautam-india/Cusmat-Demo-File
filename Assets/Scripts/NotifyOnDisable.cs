using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

public class NotifyOnDisable : MonoBehaviour
{

    public event Action<AssetReference, NotifyOnDisable> Destroyed;
    public AssetReference AssetReference { get; set; }

    public void OnDestroy()
    {
        Destroyed?.Invoke(AssetReference, this);
    }


    
}
