/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
    [SerializeField] private float planeYDistance; // y-distance of plane to camera
    [SerializeField] private bool oldCrosshair; // switch between crosshair modes step 4/5
    private Plane plane;
    private Vector3 planeCameraDistance;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    void Start()
    {
        planeCameraDistance = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - planeYDistance, Camera.main.transform.position.z);
        plane = new Plane(Vector3.up, planeCameraDistance); // normal vector n, right angle to plane
    }

    private void MoveCrosshair() 
    {
        if(oldCrosshair)
        {
            Vector2 mousePos = new Vector3(mouseAction.ReadValue<Vector2>().x,mouseAction.ReadValue<Vector2>().y, 0);
            Ray cameraToGround = Camera.main.ScreenPointToRay(mousePos); // Ray from camera passing through mouse cursor position

            float enter = 0.0f;
            Vector3 raycastPoint = new Vector3();

            if(plane.Raycast(cameraToGround, out enter))
            {
                raycastPoint = cameraToGround.GetPoint(enter);
                crosshair.position = raycastPoint;
            }
        }
        else
        {
            // new way to move crosshair using mouse.delta (change in mouse screen position)
            Vector2 mousePos = new Vector3(mouseAction.ReadValue<Vector2>().x,mouseAction.ReadValue<Vector2>().y, 0);
            Ray cameraToGround = Camera.main.ScreenPointToRay(mousePos);

            Vector2 mouseDelta = new Vector3(deltaAction.ReadValue<Vector2>().x,deltaAction.ReadValue<Vector2>().y, 0);

            float enter = 0.0f;
            Vector3 raycastPoint = new Vector3();

            if(plane.Raycast(cameraToGround, out enter))
            {
                //raycastPoint = cameraToGround.GetPoint(enter);
                Vector2 crosshairScreenPos = Camera.main.WorldToScreenPoint(crosshair.position);
                crosshairScreenPos += mouseDelta;
                
                Ray crosshairScreenToWorld = Camera.main.ScreenPointToRay(crosshairScreenPos);
                
                if(plane.Raycast(crosshairScreenToWorld, out enter))
                {
                    raycastPoint = crosshairScreenToWorld.GetPoint(enter);
                    crosshair.position = raycastPoint;
                }
            }
        }
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }

#endregion Update

}
