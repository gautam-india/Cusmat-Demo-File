using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSwitch : MonoBehaviour
{

    [SerializeField] GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var player in players)
        {
            player.SetActive(false);
        }
        players[0].SetActive(true);
    }


    public void switchPlayer(int index)
    {
        foreach (var player in players)
        {
            player.SetActive(false);
        }
        players[index].SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
