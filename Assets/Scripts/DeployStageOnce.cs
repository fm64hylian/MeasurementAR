using System;
using UnityEngine;
using Vuforia;
public class DeployStageOnce : MonoBehaviour
{
    [SerializeField]
     GameObject anchorStage1;
    [SerializeField]
     GameObject anchorStage2;
    private PositionalDeviceTracker _deviceTracker;
    private GameObject _previousAnchor;

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
    public void OnInteractiveHitTest(HitTestResult result)
    {
        if (result == null || anchorStage1 == null)
        {
            Debug.LogWarning("Hit test is invalid or AnchorStage not set");
            return;
        }

        switch (hitCounts) {
            case 0:
                Debug.Log("POSITIONING 1ST POINT");
                PutMeasurePoint(result, anchorStage1);
                tempPosition1 = anchorStage1.transform.position;
                hitCounts++;
                break;
            case 1:
                Debug.Log("POSITIONING 2ND POINT");
                PutMeasurePoint(result, anchorStage2);
                anchorStage1.transform.position = tempPosition1;
                hitCounts++;
                break;
            default: //reset
                Debug.Log("RESETING POINTS");
                ResetPoints();
                hitCounts = 0;
                break;
        }



        //var anchor = _deviceTracker.CreatePlaneAnchor(Guid.NewGuid().ToString(), result);

        //GameObject anchorGO = new GameObject();
        //anchorGO.transform.position = result.Position;
        //anchorGO.transform.rotation = result.Rotation;
        //if (anchor != null)
        //{
        //    ///
        //    AnchorStage1.transform.parent = anchorGO.transform;
        //    AnchorStage1.transform.localPosition = Vector3.zero;
        //    AnchorStage1.transform.localRotation = Quaternion.identity;

        //    AnchorStage1.SetActive(true);
        //    ///
        //    //AnchorStage.transform.parent = anchor.transform;
        //    //AnchorStage.transform.localPosition = Vector3.zero;
        //    //AnchorStage.transform.localRotation = Quaternion.identity;
        //    //AnchorStage.SetActive(true);
        //}
        //if (_previousAnchor != null)
        //{
        //    Destroy(_previousAnchor);
        //}
        //_previousAnchor = anchorGO;//anchor;
    }

    /// <summary>
    /// point is the  Measurepoint 1 or 2
    /// </summary>
    /// <param name="result"></param>
    /// <param name="point"></param>
    void PutMeasurePoint(HitTestResult result, GameObject point) {
        var anchor = _deviceTracker.CreatePlaneAnchor(Guid.NewGuid().ToString(), result);

        //GameObject anchorGO = new GameObject();
        //anchorGO.transform.position = result.Position;
        //anchorGO.transform.rotation = result.Rotation;
        if (anchor != null)
        {
            ///
            //point.transform.parent = anchorGO.transform;
            point.transform.position = result.Position;
            point.transform.rotation = result.Rotation;
            //point.transform.localPosition = Vector3.zero;
            //point.transform.localRotation = Quaternion.identity;

            point.SetActive(true);
            point.GetComponent<Collider>().enabled = true;
            point.GetComponent<MeshRenderer>().enabled = true;
            ///
            //AnchorStage.transform.parent = anchor.transform;
            //AnchorStage.transform.localPosition = Vector3.zero;
            //AnchorStage.transform.localRotation = Quaternion.identity;
            //AnchorStage.SetActive(true);
        }
    }

    void ResetPoints() {
        anchorStage1.transform.position = initialPos;
        anchorStage2.transform.position = initialPos;

        anchorStage1.transform.rotation = Quaternion.identity;
        anchorStage2.transform.rotation = Quaternion.identity;

        anchorStage1.SetActive(false);
        anchorStage2.SetActive(false);
    }
}