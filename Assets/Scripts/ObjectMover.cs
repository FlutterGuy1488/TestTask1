using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private float zDistance;

    void Start()
    {
        //locating the camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    selectedObject = hit.transform.gameObject;
                    zDistance = mainCamera.WorldToScreenPoint(selectedObject.transform.position).z;
                    offset = selectedObject.transform.position - GetMouseWorldPosition();
                }
            }
        }
        //checking if we are holding a movable object
        if (Input.GetMouseButton(0) && selectedObject != null && selectedObject.tag=="movable")
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition = new Vector3(newPosition.x, newPosition.y, -6.5f);
            if (newPosition.y <= 0.3f) {
                newPosition.y = 0.3f;
            }
            selectedObject.transform.position = newPosition;
        }

        //stopping movement
        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = zDistance; // Set the distance from the camera
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}

