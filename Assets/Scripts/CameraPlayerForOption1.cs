using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;

public class CameraPlayerForOption1 : MonoBehaviour {

    public RawImage rawimage;

    WebCamTexture tex;

    // Use this for initialization
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        // assuming the first available WebCam is desired
        tex = new WebCamTexture(devices[0].name);
    }

    public void turnCameraOn()
    {
        
        // Sets values that help sharpen the feed from the camera
        //tex.anisoLevel = 9;
        //tex.mipMapBias = -0.5F;
        tex.Play();
        rawimage.color = Color.white;
        rawimage.texture = tex;
        rawimage.material.mainTexture = tex;
    }

    public void turnCameraOff()
    {
        if (tex.isPlaying)
        {
            tex.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        turnCameraOn();
    }

    void OnDisable()
    {
        turnCameraOff();
    }
}
