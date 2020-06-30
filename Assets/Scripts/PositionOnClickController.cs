using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

/// <summary>
/// class based on DeployStageOnce, used in previous versions of Vuforia to 
/// instanciate only one new game object every time a user clicked
/// </summary>
public class PositionOnClickController : MonoBehaviour
{
    [SerializeField]
    GameObject anchorStage1;
    [SerializeField]
    GameObject anchorStage2;

    [SerializeField]
    Text txtUI;
    private PositionalDeviceTracker _deviceTracker;

    Vector3 initialPos;
    int hitCounts = 0;
    MeasurePoint startPoint;
    MeasurePoint endPoint;

    Vector3 tempPosition1;

    public void Start()
    {
        startPoint = anchorStage1.GetComponent<MeasurePoint>();
        endPoint = anchorStage1.GetComponent<MeasurePoint>();
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


    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    txtUI.text = "TOUCHING SCREEN on PositionOnCLickController";
                }
            }
            else if (touch.phase == TouchPhase.Moved) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray))
                {
                    // Create a particle if hit
                    txtUI.text = "MOVING ON SCREEN on PositionOnCLickController";
                }
            }
        }

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);            
        //    Debug.Log("touched "+touch.position);
        //    // Move the cube if the screen has the finger moving.
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        Debug.Log("moving"+ touch.position);
        //    }
        //}
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
            txtUI.text = "Hit test is invalid or AnchorStage not set";
            return;
        }

        //return if the points are being dragged
        if (startPoint.IsDragging || endPoint.IsDragging) {
            return;
        }

        switch (hitCounts)
        {
            case 0:
                //Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePointPlane(result, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                txtUI.text = "hitting starting point";
                hitCounts++;
                break;
            case 1:
                //Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePointPlane(result, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                txtUI.text = "hitting ending point";
                hitCounts++;
                break;
            default: //reset
                //Debug.Log("RESETING POINTS");
                txtUI.text = "RESETING POINTS";
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

        //return if the points are being dragged
        if (startPoint.IsDragging || endPoint.IsDragging)
        {
            txtUI.text = "dragging";
            return;
        }

        switch (hitCounts)
        {
            case 0:
                //Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePointMidAir(pose, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                hitCounts++;
                txtUI.text = "POSITIONING 1ST POINT, anchorStageMesh is "+ anchorStage1.GetComponent<MeshRenderer>().enabled;
                break;
            case 1:
                //Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePointMidAir(pose, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                txtUI.text = "POSITIONING 2ND POINT, anchorStageMesh is " + anchorStage1.GetComponent<MeshRenderer>().enabled;
                hitCounts++;
                break;
            default: //reset
                //Debug.Log("RESETING POINTS");
                ResetPoints();
                hitCounts = 0;
                txtUI.text = "RESETING POINTS";
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
        var anchor = _deviceTracker.CreateMidAirAnchor(point.name, inputPosition.position, point.transform.rotation); //inputPosition.rotation
        if (anchor != null)
        {
            point.transform.position = inputPosition.position;
            point.transform.rotation = Quaternion.identity;//inputPosition.rotation;

            point.SetActive(true);
            point.GetComponent<Collider>().enabled = true;
            point.GetComponent<MeshRenderer>().enabled = true;
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
