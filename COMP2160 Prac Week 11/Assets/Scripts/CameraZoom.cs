using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Vector2 ZoomRange;
    [SerializeField] Vector2 ZoomRangeOrtho;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomSpeedOrtho;
    private Actions actions;
    private InputAction zoomAction;

    void Awake()
    {
        actions = new Actions();
        zoomAction = actions.camera.zoom;
    }

    void OnEnable()
    {
        zoomAction.Enable();
    }

    void OnDisable()
    {
        zoomAction.Disable();
    }

    void Update()
    {
        float zoomInput = zoomAction.ReadValue<float>();
        bool isOrthographic = Camera.main.orthographic;

        if(!isOrthographic)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, ZoomRange.x, ZoomRange.y);

            if(zoomInput > 0)
            {
                Camera.main.fieldOfView -= zoomSpeed; // Zoom in
            }
            if(zoomInput < 0)
            {
                Camera.main.fieldOfView += zoomSpeed; // Zoom out
            }
        }
        else 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, ZoomRangeOrtho.x, ZoomRangeOrtho.y);

            if(zoomInput > 0)
            {
                Camera.main.orthographicSize -= zoomSpeedOrtho; // Zoom in
            }
            if(zoomInput < 0)
            {
                Camera.main.orthographicSize += zoomSpeedOrtho; // Zoom out
            }
        }
    }
}
