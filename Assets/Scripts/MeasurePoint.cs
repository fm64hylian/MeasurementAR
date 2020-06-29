using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType {
    Start,
    End
}

public class MeasurePoint : MonoBehaviour
{
    public PointType pointType;
    // Start is called before the first frame update
    Vector3 screenPoint;
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint); // + offset;
        transform.position = curPosition;

    }
}
