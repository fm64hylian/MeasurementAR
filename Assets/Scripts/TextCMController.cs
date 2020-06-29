using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCMController : MonoBehaviour
{
    [SerializeField]
    GameObject point1;
    [SerializeField]
    GameObject point2;
    [SerializeField]
    Transform cam;
    [SerializeField]
    LineRenderer lineRenderer;
    Vector3 initialPos;

    static Vector3 posFrom;
    static Vector3 posTo;

    TextMesh txt;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        txt = gameObject.GetComponent<TextMesh>();

        lineRenderer.textureMode = LineTextureMode.Stretch;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = new Color32(119, 235, 52, 255);
        lineRenderer.endColor = new Color32(119, 235, 52, 255);
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        Debug.Log("line wid "+ lineRenderer.startWidth+", "+ lineRenderer.endWidth);
    }

    // Update is called once per frame
    void Update()
    {
        //no points active
        if (!point1.activeInHierarchy && !point2.activeInHierarchy) {
            txt.text = "";
            transform.position = initialPos;
            lineRenderer.enabled = false;
            return;
        }

        //first point active displays 0
        if (point1.activeInHierarchy && !point2.activeInHierarchy) {
            txt.text = "0 cm";
            transform.position = point1.transform.position; // + Vector3.up * 0.1f;
            //always looking at camera
            transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        }

        //both points will show the distance
        if (point1.activeInHierarchy && point2.activeInHierarchy) {
            txt.text = ConvertToCM();

            posFrom = point1.transform.position;
            posTo = point2.transform.position;

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, posFrom);
            lineRenderer.SetPosition(1, posTo);
            //always looking at camera
            transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        }
    }

    string ConvertToCM()
    {
        Vector3 from = point1.transform.position;
        Vector3 to = point2.transform.position;
        transform.position = new Vector3((from.x + to.x) / 2f, (from.y + to.y) / 2f, (from.z + to.z) / 2f);
        transform.rotation = Camera.main.transform.rotation;
        float unityU = (float)System.Math.Round(Vector3.Distance(from, to), 2);
        return unityU >= 1 ? unityU + " mt" : unityU * 100f +" cm";
    }
}
