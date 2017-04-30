using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;

public class CameraPlayerForOption2 : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private Tagalong tagalong;
    private GestureRecognizer gestureRecognizer;

    void Start()
    {

        tagalong = GetComponent<Tagalong>();

        // Set up gestureRecognizer to listen for tapped events and what to do when event is recognized.
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
        gestureRecognizer.TappedEvent += gestureRecognizer_TappedEvent;
        gestureRecognizer.HoldStartedEvent += gestureRecognizer_HoldStartedEvent;
        gestureRecognizer.HoldCompletedEvent += gestureRecognizer_HoldCompletedEvent;
        gestureRecognizer.HoldCanceledEvent += gestureRecognizer_HoldCanceledEvent;


        var devices = WebCamTexture.devices;
        var camName = "";
        if (devices.Length > 0) camName = devices[0].name;
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camName = devices[i].name;
            }
        }
        webcamTexture = new WebCamTexture(camName, 640, 360, 30);
    }

    void OnEnable()
    {
        gestureRecognizer.StartCapturingGestures();

        webcamTexture.Play();
        var renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;

    }

    void OnDisable()
    {
        gestureRecognizer.StopCapturingGestures();
        webcamTexture.Stop();
    }

    void Update()
    {

    }

    private void gestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (tagalong.isActiveAndEnabled)
        {
            tagalong.enabled = false;
        }
        else
        {
            tagalong.enabled = true;
        }
    }

    private void gestureRecognizer_HoldCanceledEvent(InteractionSourceKind source, Ray headRay)
    {
        throw new System.NotImplementedException();
    }

    private void gestureRecognizer_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {
        throw new System.NotImplementedException();
    }

    private void gestureRecognizer_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {
        throw new System.NotImplementedException();
    }
}
