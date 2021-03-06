﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this onto a camera
/// </summary>
public class CamControl : MonoBehaviour {

    public Transform pivot;
    public Transform player;

    [SerializeField] float zoomSensi = 100f;
    [SerializeField] float rotateSensi = 10f;

    [SerializeField] float yaw = - Mathf.PI/2;//radians

    [SerializeField] float pitchMax = 85f;
    [SerializeField] float pitchMin = -5f;
    [SerializeField] float pitch = 25f;

    [SerializeField] float distMax = 5f;
    [SerializeField] float distMin = 0.3f;
    [SerializeField] float distance = 2f;
    [SerializeField] float distanceThreshold = 1f;

    [SerializeField] EasyJoystick joystick;

    public static CamControl instance;


    Camera _cam;
    Camera cam {
        get
        {
            if (!_cam)
                _cam = GetComponent<Camera>();

            return _cam;
        }
        set
        {
            _cam = value;
        }
    }

    SkinnedMeshRenderer mr;
    [SerializeField] Material alternateMaterial;
    Material originalMaterial;

    bool lockView = false;

    void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        pitchMax *= Mathf.Deg2Rad;
        pitchMin *= Mathf.Deg2Rad;
    }

    /// <summary>
    /// Motion controller of the player
    /// </summary>
    MotionController mc;

    public void Init()
    {
        mr = player.GetComponentInChildren<SkinnedMeshRenderer>();
        originalMaterial = mr.sharedMaterial;
        player.GetComponent<AttrController>().Death += Death;
        mc = player.GetComponent<MotionController>();
        
    }

    private void Death()
    {
        pivot = null;
    }

    public void ResetRotation()
    {
        yaw = -Mathf.PI/2;
        pitch = 0;
    }

    // Update is called once per frame
    void LateUpdate () {

        if (!pivot )
            return;

        if (Input.GetMouseButton(0))// || Input.GetTouch(0).position)
            CheckClick();

        if (GeneralController.playerMC.playerControlled)
            HandleInput();

        player.Rotate(new Vector3(0, - rotateSensi * Time.deltaTime * joystick.JoystickValue.x * Mathf.Rad2Deg, 0));

        float yaw1 = yaw - player.rotation.eulerAngles.y * Mathf.Deg2Rad;

        transform.position = new Vector3(distance * Mathf.Cos(pitch) * Mathf.Cos(yaw1),
            distance * Mathf.Sin(pitch),
            distance * Mathf.Cos(pitch) * Mathf.Sin(yaw1)) + pivot.position;

        transform.LookAt(pivot);

        if (distance < 1)
        {
            if (mr.sharedMaterial != alternateMaterial)
                mr.sharedMaterial = alternateMaterial;
        }
        else
        {
            if (mr.sharedMaterial != originalMaterial)
                mr.sharedMaterial = originalMaterial;
        }
	}

    Vector3 detectionRange = new Vector3(0.1f,0.1f,0.1f);


    Vector3 dy;
    Vector3 dx;//half the near clip plane's width

    float dyf, dxf;
    Vector3 nearOrigin;

    bool CheckWall()
    {
        dyf = cam.nearClipPlane * Mathf.Tan(cam.fieldOfView / 2 * Mathf.Deg2Rad);
        dxf = dyf * cam.aspect;
        dx = transform.right * dxf;
        dy = transform.up * dyf;
        nearOrigin = transform.position + transform.forward * cam.nearClipPlane;

        return(Physics.BoxCast(transform.position, new Vector3(dxf, dyf, 0.001f), transform.forward, transform.rotation, cam.nearClipPlane, 1<<11));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = CheckWall() ? Color.red : Color.yellow;
        Gizmos.DrawLine(transform.position, nearOrigin);
        Gizmos.DrawRay(nearOrigin, dx);
        Gizmos.DrawRay(nearOrigin, dy);
        Gizmos.DrawRay(nearOrigin, -dx);
        Gizmos.DrawRay(nearOrigin, -dy);
    }

    /// <summary>
    /// Make sure you have a kinametic-rigidbody3D attached.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);

        if ("Terrain" == other.tag)
        {
            distance -= Time.deltaTime * 10 * zoomSensi;
            //Debug.Log("Crashed.");
        }
    }

    void HandleInput()
    {
        

        distance += zoomSensi * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance, distMin, distMax);

        //yaw += rotateSensi * Time.deltaTime * joystick.JoystickValue.x;

        //pitch += rotateSensi * Time.deltaTime * joystick.JoystickValue.y;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    // Subscribe to events
    void OnEnable()
    {
        EasyTouch.On_TouchStart += BeginTouch;
        EasyTouch.On_TouchUp += EndTouch;
        EasyTouch.On_PinchIn += ZoomIn;
        EasyTouch.On_PinchOut += ZoomOut;
        EasyTouch.On_TouchDown += Moving;
    }

    void OnDisable()
    {
        UnsubscribeEvent();
    }

    void OnDestroy()
    {
        UnsubscribeEvent();
    }

    // Unsubscribe to events
    void UnsubscribeEvent()
    {
        EasyTouch.On_TouchStart -= BeginTouch;
        EasyTouch.On_TouchUp -= EndTouch;
        EasyTouch.On_PinchIn -= ZoomIn;
        EasyTouch.On_PinchOut -= ZoomOut;
        EasyTouch.On_TouchDown -= Moving;
    }

    bool freeSwipe = true;

    void BeginTouch(Gesture gesture)
    {
        if (GeneralController.playerMC.playerControlled)
        {
            freeSwipe = !gesture.isHoverReservedArea;
            yaw = -Mathf.PI / 2;
            pitch = 0;
        }
    }

    void EndTouch(Gesture gesture)
    {
        if (freeSwipe && GeneralController.playerMC.playerControlled)
        {
            mc.needReset = true;
            player.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            yaw = -Mathf.PI / 2;
            
        }
    }

    void Moving(Gesture gesture)
    {

        //Debug.Log(gesture.pickObject);

        if (freeSwipe && GeneralController.playerMC.playerControlled)
        {
            yaw += rotateSensi * Time.deltaTime * gesture.deltaPosition.x;

            pitch += rotateSensi * Time.deltaTime * gesture.deltaPosition.y;
        }
        
    }

    void ZoomIn(Gesture gesture)
    {
        if (freeSwipe && GeneralController.playerMC.playerControlled)
        {
            float zoom = Time.deltaTime * gesture.deltaPinch;

            distance += zoomSensi * zoom;
        }
    }

    void ZoomOut(Gesture gesture)
    {
        if (freeSwipe && GeneralController.playerMC.playerControlled)
        {

            float zoom = Time.deltaTime * gesture.deltaPinch;

            distance -= zoomSensi * zoom;
        }

    }

    public void CheckClick()
    {
        RaycastHit hitInfo;
        Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 50f, 1 << 9 | 1 << 10 | 1<< 11, QueryTriggerInteraction.Ignore);

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 50f, 1 << 9 | 1 << 10 | 1 << 11, QueryTriggerInteraction.Ignore))
        {
            MotionController mc = hitInfo.collider.GetComponent<MotionController>();

            if (mc && mc.listItem != null)
            {
                GeneralController.instance.HaltPlayer();

                Debug.LogFormat("Clicked on {0}", mc.listItem.attrController.unitName);
                mc.listItem.PlayerAttack();
            }
        }

        

    }

}
