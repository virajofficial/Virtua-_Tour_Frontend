using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRCameraScript : MonoBehaviour
{
    public bool isDragging = false;
    bool isUIDetected = false;
    
    float startMouseX;
    float startMouseY;
    
    public Camera cam;
    void Start()
    {
        cam = this.gameObject.GetComponent<Camera>();
    }
    
    void Update()
    {
        DetectObject();


        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            isDragging = true;
            
            startMouseX = Input.mousePosition.x;
            startMouseY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            ZoomButton(true);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomButton(false);
        }
        {

        }
    }

    void LateUpdate()
    {
        if (isDragging && !UIManager.isLoading && !isUIDetected)
        {
            float endMouseX = Input.mousePosition.x;
            float endMouseY = Input.mousePosition.y;
            
            float diffX = endMouseX - startMouseX;
            float diffY = endMouseY - startMouseY;
            
            float newCenterX = Screen.width / 2 + -diffX;
            float newCenterY = Screen.height / 2 + -diffY;
            
            Vector3 LookHerePoint = cam.ScreenToWorldPoint(new Vector3(newCenterX, newCenterY, cam.nearClipPlane));
            
            transform.LookAt(LookHerePoint);
            
            startMouseX = endMouseX;
            startMouseY = endMouseY;
        }
    }

    void DetectObject()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isUIDetected = true;
            }
            else
            {
                isUIDetected = false;
            }
        }
    }

    public void ZoomButton(bool isZoom)
    {
        if (isZoom && !UIManager.isLoading && cam.fieldOfView >= 15)
        {
            cam.fieldOfView--;
        }
        else if(!isZoom && !UIManager.isLoading && cam.fieldOfView <= 75)
        {
            cam.fieldOfView++;
        }
    }
    
}
