using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkPose : MonoBehaviour
{
    [SerializeField]
    GameObject GamePose1;
    [SerializeField]
    GameObject GamePose2;
    [SerializeField]
    TextMesh textCM;
    Vector3 initialPos;
    Vector3 position2, position1;
    Vector3 pos_temp;

    Transform anchor1;
    Transform anchor2;

    Vector3 position_difference, size_temp;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        textCM.transform.position = transform.position;
        textCM.text = "";

        anchor1 = GamePose1.transform.parent;
        anchor2 = GamePose2.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GamePose1.activeInHierarchy && !GamePose2.activeInHierarchy)
        {
            Vector3 behindCam = Camera.main.transform.position + Vector3.back * 2f;
            transform.position = behindCam;
            textCM.transform.position = behindCam;
            return;
        }


        //position_difference = transform.position;
        size_temp = transform.localScale;
        position1 = anchor1.position;//GetComponent<Pose>().pos;
        position2 = anchor2.position;//.GetComponent<Pose2>().pos2;

        float distanceX = position1.x - position2.x;
        //float distanceY = position1.y - position2.y;
        //float distanceZ = position1.z - position2.z;
        size_temp.x = distanceX / 2;
        //size_temp.y = distanceY / 2;
        //size_temp.z = distanceZ / 2;

        pos_temp = position1 - position2;

        transform.position = pos_temp;
        transform.localScale = size_temp;


        //adjust text to middle
        textCM.transform.position = new Vector3((position1.x + position2.x) / 2f, (position1.y + position2.y) / 2f, (position1.z + position2.z) / 2f);
        textCM.transform.rotation = Camera.main.transform.rotation;
        textCM.text = ConvertToCM(Vector3.Distance(position1, position2));
    }

    string  ConvertToCM(float unityU) {
        return unityU >= 1 ? "MT " + unityU : "CM " + unityU * 100f ;
    }
}
