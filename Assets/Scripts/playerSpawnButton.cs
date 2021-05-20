using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerSpawnButton : MonoBehaviour
{

    [SerializeField] private int _index;


    

    private void OnValidate()
    {
        _index = transform.GetSiblingIndex();
        GetComponentInChildren<Text>().text = _index.ToString();
    }

   

    public void RequestSpawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            Destroy(player);


        FindObjectOfType<_Manager>().Spawn(_index);
    }

    
}
