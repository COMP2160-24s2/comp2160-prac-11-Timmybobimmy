using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Vector2 ZoomRange;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomSpeedOrthographic;
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
        Mathf.Clamp(Camera.main.fieldOfView, ZoomRange.x, ZoomRange.y);
        Mathf.Clamp(Camera.main.orthographicSize, ZoomRange.x, ZoomRange.y);

        float zoomInput = zoomAction.ReadValue<float>();
        bool isOrthographic = Camera.main.orthographic;

        if(!isOrthographic)
        {
            if(zoomInput == 120)
            {
                Camera.main.fieldOfView += zoomSpeed;
            }
            if(zoomInput == -120)
            {
                Camera.main.fieldOfView -= zoomSpeed;
            }
        }
        else 
        {
            if(zoomInput == 120)
            {
                Camera.main.orthographicSize += zoomSpeedOrthographic;
            }
            if(zoomInput == -120)
            {
                Camera.main.orthographicSize -= zoomSpeedOrthographic;
            }
        }
    }
}
