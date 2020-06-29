using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Input.simulateMouseWithTouches = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("touching from CameraInputListener");
        }
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("clicking from CameraInputListener");
        }
        //foreach (Touch touch in Input.touches)
        //{
        //    Debug.Log("DETECTING TOUCH on CameraInputListener");
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        // Construct a ray from the current touch coordinates
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        Debug.Log("DETECTING RAY on CameraInputListener");
        //        if (Physics.Raycast(ray))
        //        {
        //            // Create a particle if hit
        //            Debug.Log("TOUCHING SCREEN on CameraInputListener");
        //        }
        //    }
        //    else if (touch.phase == TouchPhase.Moved)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        //        if (Physics.Raycast(ray))
        //        {
        //            // Create a particle if hit
        //            Debug.Log("MOVING ON SCREEN on CameraInputListener");
        //        }
        //    }
        //}
    }
}
