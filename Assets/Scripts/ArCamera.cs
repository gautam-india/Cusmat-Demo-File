using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArCamera : MonoBehaviour
{

    private string deviceName;
    private WebCamTexture wct;
    private WebCamDevice[] devices;
    public int deviceNumber;
    public Renderer arRenderer;

    // Start is called before the first frame update
    void Start()
    {
        PlayAr();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        StopAr();
    }

    public void StopAr()
    {
        wct.Stop();
    }

    public void PlayAr()
    {
        devices = WebCamTexture.devices;
        deviceName = devices[deviceNumber].name;
        wct = new WebCamTexture(deviceName, 600, 600, 30);
        arRenderer.material.mainTexture = wct;
        wct.Play();
    }

}
