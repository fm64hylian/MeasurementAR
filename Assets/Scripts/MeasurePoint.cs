using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PointType {
    Start,
    End
}

public class MeasurePoint : MonoBehaviour
{
    public PointType pointType;
    public bool IsDragging = false;
    Vector3 screenPoint;

    void Start()
    {
        
    }

    void OnMouseDown()
    {
        IsDragging = true;
    }

    void OnMouseDrag()
    {
        MovePoint();
    }

    private void OnMouseUp()
    {
        IsDragging = false;
    }

    void MovePoint()
    {
        Debug.Log("DRAGGING MEASUREPOINT "+pointType);
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objPosition;
        IsDragging = true;
    }
}
