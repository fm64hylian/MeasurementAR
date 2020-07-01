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
    GameObject MidAirPositioner;
    [SerializeField]
    GameObject anchorStage1;
    [SerializeField]
    GameObject anchorStage2;
    [SerializeField]
    GameObject SaveMenu;
    [SerializeField]
    ScrollRect BodyPartMenu;

    [SerializeField]
    Text txtUI;
    [SerializeField]
    TextCMController measurementController;
    [SerializeField]
    Text txtSavedValue;
    private PositionalDeviceTracker _deviceTracker;

    //AR 
    Vector3 initialPos;
    int hitCounts = 0;
    MeasurePoint startPoint;
    MeasurePoint endPoint;

    Vector3 tempPosition1;


    /// body parts
    public List<BodyPartMeasure> BodyParts = new List<BodyPartMeasure>();
    BodyPartUI SelectedPartUI;

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
        SaveMenu.SetActive(false);

        MidAirPositioner.SetActive(false);
        SetGridValues();
    }

    void SetGridValues()
    {

        //BodyPartMenu.GetComponent <>
        GameObject gridContent = BodyPartMenu.content.gameObject;
        for (int i = 1; i < System.Enum.GetValues(typeof(BodyPart)).Length; i++)
        {
            BodyParts.Add(new BodyPartMeasure((BodyPart)i, 0f));

            GameObject bodyPrefab = Instantiate(Resources.Load("Prefabs/BodyPartUI")) as GameObject;
            BodyPartUI bodyUI = bodyPrefab.GetComponent<BodyPartUI>();
            bodyUI.SetData(BodyParts[i - 1]);
            bodyUI.OnSelected = OnBodyPartSelected;
            bodyUI.OnSaved = OnBodyPartSaved;

            bodyUI.gameObject.transform.SetParent(gridContent.transform);
            //bodyUI.gameObject.transform.parent = gridContent.transform;
            bodyUI.gameObject.transform.localScale = Vector3.one;
        }

        BodyPartMenu.gameObject.SetActive(false);
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
    }


    /// <summary>
    /// on input touch, declare what to do here (plane version)
    /// </summary>
    /// <param name="result"></param>
    public void OnInteractivePlaneHit(HitTestResult result)
    {
        if (!MidAirPositioner.activeInHierarchy)
        {
            return;
        }

        if (result == null || anchorStage1 == null)
        {
            Debug.LogWarning("Hit test is invalid or AnchorStage not set");
            txtUI.text = "Hit test is invalid or AnchorStage not set";
            return;
        }

        //return if the points are being dragged
        if (startPoint.IsDragging || endPoint.IsDragging || SaveMenu.activeInHierarchy)
        {
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
                hitCounts++;
                SaveMenu.SetActive(true);
                txtUI.text = "hitting ending point, menu " + SaveMenu.activeInHierarchy;
                break;
            default: //reset
                //Debug.Log("RESETING POINTS");
                //txtUI.text = "RESETING POINTS";
                //ResetPoints();
                //hitCounts = 0;
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
        if (startPoint.IsDragging || endPoint.IsDragging || SaveMenu.activeInHierarchy)
        {
            txtUI.text = "returning";
            return;
        }

        switch (hitCounts)
        {
            case 0:
                //Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePointMidAir(pose, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                hitCounts++;
                //testing if its still false, not working on device
                if (!anchorStage1.GetComponent<MeshRenderer>().enabled)
                {
                    anchorStage1.SetActive(true);
                    anchorStage1.GetComponent<Collider>().enabled = true;
                    anchorStage1.GetComponent<MeshRenderer>().enabled = true;
                }
                //
                txtUI.text = "POSITIONING 1ST POINT, anchorStageMesh is " + anchorStage1.GetComponent<MeshRenderer>().enabled;
                break;
            case 1:
                //Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePointMidAir(pose, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                SaveMenu.SetActive(true);
                txtUI.text = "hitting ending point, menu " + SaveMenu.activeInHierarchy;
                break;
                //default: //reset
                //    CancelSave();
                //    break;
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

    void OnBodyPartSelected(BodyPartUI partUI)
    {
        SelectedPartUI = partUI;

        //enter AR mode
        BodyPartMenu.gameObject.SetActive(!BodyPartMenu.gameObject.activeInHierarchy);
        MidAirPositioner.SetActive(true);
        //ToggleBodyMenu();
    }

    void OnBodyPartSaved(BodyPartUI partUI)
    {
        BodyPartMeasure modelPart = BodyParts.Find(x => x.Part.Equals(SelectedPartUI.BodyPart.Part));
        if (modelPart != null)
        {
            Debug.Log("saving bodypart value");
            //save on model
            modelPart.Cm = measurementController.GetUnityValue() * 100f;

            //update UI
            partUI.UpdateMeasure(System.Math.Round(modelPart.Cm, 2).ToString());
            partUI.IsSet = true;

            txtSavedValue.text = measurementController.GetTextValue();
            ResetPoints();
            hitCounts = 0;
            SaveMenu.SetActive(false);
            MidAirPositioner.SetActive(false);
        }
    }


    /////  UI 

    public void SaveMeasurement()
    {
        Debug.Log("on save measurement button");
        SelectedPartUI.OnPartSaved();
    }

    public void CancelSave()
    {
        ResetPoints();
        hitCounts = 0;
        txtUI.text = "RESETING POINTS";
        SaveMenu.SetActive(false);
    }


    public void ToggleBodyMenu()
    {
        BodyPartMenu.gameObject.SetActive(!BodyPartMenu.gameObject.activeInHierarchy);
        if (MidAirPositioner.activeInHierarchy)
        {
            MidAirPositioner.SetActive(false);
        }
    }
}
