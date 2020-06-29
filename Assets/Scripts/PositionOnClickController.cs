using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Vuforia;

public class PositionOnClickController : MonoBehaviour
{
    [SerializeField]
    GameObject anchorStage1;
    [SerializeField]
    GameObject anchorStage2;
    private PositionalDeviceTracker _deviceTracker;

    Vector3 initialPos;
    int hitCounts = 0;

    Vector3 tempPosition1;

    public void Start()
    {
        Debug.Log("STARTING DEPLOY STAGE ONCE");
        if (anchorStage1 == null)
        {
            Debug.Log("AnchorStage must be specified");
            return;
        }
        initialPos = anchorStage1.transform.position;
        anchorStage1.SetActive(false);
        anchorStage2.SetActive(false);
    }
    public void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }
    public void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }
    private void OnVuforiaStarted()
    {
        _deviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
    }

    /// <summary>
    /// on input touch, declare what to do here (plane version)
    /// </summary>
    /// <param name="result"></param>
    public void OnInteractivePlaneHit(HitTestResult result)
    {
        if (result == null || anchorStage1 == null)
        {
            Debug.LogWarning("Hit test is invalid or AnchorStage not set");
            return;
        }


        switch (hitCounts)
        {
            case 0:
                Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePointPlane(result, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                hitCounts++;
                break;
            case 1:
                Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePointPlane(result, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                hitCounts++;
                break;
            default: //reset
                Debug.Log("RESETING POINTS");
                ResetPoints();
                hitCounts = 0;
                break;
        }
    }
    
    /// <summary>
    /// on input touch, declare what to do here (mid air version)
    /// </summary>
    /// <param name="pose"></param>
    public void OnInteractiveMidAirHit(Transform pose)
    {
        if (pose == null || anchorStage1 == null)
        {
            Debug.LogWarning("Hit test is invalid or AnchorStage not set");
            return;
        }


        switch (hitCounts)
        {
            case 0:
                Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePointMidAir(pose, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                hitCounts++;
                break;
            case 1:
                Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePointMidAir(pose, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                hitCounts++;
                break;
            default: //reset
                Debug.Log("RESETING POINTS");
                ResetPoints();
                hitCounts = 0;
                break;
        }
    }

    /// <summary>
    /// point is the  Measurepoint 1 or 2
    /// </summary>
    /// <param name="result"></param>
    /// <param name="point"></param>
    void PutMeasurePointPlane(HitTestResult result, GameObject point)
    {
        var anchor = _deviceTracker.CreatePlaneAnchor(Guid.NewGuid().ToString(), result);
        if (anchor != null)
        {
            point.transform.position = result.Position;
            point.transform.rotation = result.Rotation;

            point.SetActive(true);
        }
    }
    
    /// <summary>
    /// point is the  Measurepoint 1 or 2
    /// </summary>
    /// <param name="result"></param>
    /// <param name="point"></param>
    void PutMeasurePointMidAir(Transform inputPosition, GameObject point)
    {
        var anchor = _deviceTracker.CreateMidAirAnchor(point.name, inputPosition.position, inputPosition.rotation);
        if (anchor != null)
        {
            point.transform.position = inputPosition.position;
            point.transform.rotation = inputPosition.rotation;

            point.SetActive(true);
        }
    }

    void ResetPoints()
    {
        anchorStage1.transform.position = initialPos;
        anchorStage2.transform.position = initialPos;

        anchorStage1.transform.rotation = Quaternion.identity;
        anchorStage2.transform.rotation = Quaternion.identity;

        anchorStage1.SetActive(false);
        anchorStage2.SetActive(false);
    }
}
