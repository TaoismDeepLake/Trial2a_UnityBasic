using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this onto a camera
/// </summary>
public class CamControl : MonoBehaviour {

    [SerializeField] Transform pivot;
    [SerializeField] Transform player;

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

    Camera cam;

    SkinnedMeshRenderer mr;
    [SerializeField] Material alternateMaterial;
    Material originalMaterial;

    bool lockView = false;

    void Awake()
    {
        cam = GetComponent<Camera>();

        pitchMax *= Mathf.Deg2Rad;
        pitchMin *= Mathf.Deg2Rad;

        mr = player.GetComponentInChildren<SkinnedMeshRenderer>();
        originalMaterial = mr.sharedMaterial;

        LockView();
        LockView();
    }

    private void Start()
    {
        player.GetComponent<AttrController>().Death += Death;

    }

    private void Death()
    {
        pivot = null;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            LockView();

        if (!pivot || lockView)
            return;

        HandleInput();

        player.Rotate(new Vector3(0, - rotateSensi * Time.deltaTime * Input.GetAxis("Mouse X") * Mathf.Rad2Deg, 0));

        transform.position = new Vector3(distance * Mathf.Cos(pitch) * Mathf.Cos(yaw),
            distance * Mathf.Sin(pitch),
            distance * Mathf.Cos(pitch) * Mathf.Sin(yaw)) + pivot.position;

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

    void LockView()
    {
        lockView = !lockView;
        if (lockView)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void HandleInput()
    {
        

        distance += zoomSensi * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance, distMin, distMax);

        yaw += rotateSensi * Time.deltaTime * Input.GetAxis("Mouse X");

        pitch += rotateSensi * Time.deltaTime * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }
}
