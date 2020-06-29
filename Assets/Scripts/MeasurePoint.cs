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

    private void Update()
    {

        //if (Input.GetMouseButtonDown(0))
        //{
        //    MovePoint();
        //    return;
        //}
        //foreach (Touch touch in Input.touches)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit, 100))
        //    {
        //        //Might need to set X and Z depending on how your game is set up as touch.position is a 2D Vector
        //        transform.position = new Vector3(touch.position.x, touch.position.y, transform.position.z);
        //    }
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    IsDragging = false;
        //}
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
