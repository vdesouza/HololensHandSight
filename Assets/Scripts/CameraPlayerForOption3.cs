using Academy.HoloToolkit.Unity;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;

public class CameraPlayerForOption3 : MonoBehaviour
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
        //gestureRecognizer.StartCapturingGestures();

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
        webcamTexture = new WebCamTexture(camName);
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
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.x = 45;
            transform.rotation = Quaternion.Euler(rotationVector);
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            //var headPosition = Camera.main.transform.position;
            //var gazeDirection = Camera.main.transform.forward;

            //RaycastHit hitInfo;
            //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            //    30.0f, SpatialMapping.PhysicsRaycastMask))
            //{
            //    // Move this object's parent object to
            //    // where the raycast hit the Spatial Mapping mesh.
            //    this.transform.parent.position = hitInfo.point;

            //    // Rotate this object's parent object to face the user.
            //    Quaternion toQuat = Camera.main.transform.localRotation;
            //    toQuat.x = 0;
            //    toQuat.z = 0;
            //    this.transform.parent.rotation = toQuat;
            //}
        }
        else
        {
            tagalong.enabled = true;
            transform.eulerAngles = new Vector3(45, 0, 0);
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
