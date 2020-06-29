using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PositionOnClickController : MonoBehaviour
{
    [SerializeField]
    ContentPositioningBehaviour content;

    [SerializeField]
    MeasurePoint pointFrom;
    MeasurePoint pointTo;

    [SerializeField]
    TextMesh textCM;
    Vector3 touchFromPos;
    Vector3 touchToPos;

    int times = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting input");
    }

    // Update is called once per frame
    void Update()
    {
        if (times ==2) {
            textCM.text = ConvertToCM();
        }

        if (Input.touchCount > 0) //Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
            {
                if (times == 0)
                { //first time
                    touchFromPos = Input.mousePosition;
                    times++;
                    Debug.Log("clicked once");
                    return;
                }

                touchToPos = Input.mousePosition;
                Debug.Log("clicked twice");
                times = 0;

            }

        }
    }


    string ConvertToCM()
    {
        textCM.transform.position = new Vector3((touchFromPos.x + touchToPos.x) / 2f, (touchFromPos.y + touchToPos.y) / 2f, (touchFromPos.z + touchToPos.z) / 2f);
        textCM.transform.rotation = Camera.main.transform.rotation;
        float unityU = Vector3.Distance(touchFromPos, touchFromPos);
        return unityU >= 1 ? "MT " + unityU : "CM " + unityU * 100f;
    }
}
