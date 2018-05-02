using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this onto a camera
/// </summary>
public class CamControlFG : MonoBehaviour {

    public Transform pivot;
    public Transform player;

    [SerializeField] float zoomSensi = 1f;

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
        cam = GetComponent<Camera>();
        //orbit = GetComponent<TBOrbit>();
    }

    private void Start()
    {
        mr = player.GetComponentInChildren<SkinnedMeshRenderer>();
        originalMaterial = mr.sharedMaterial;

        //LockView();
        //LockView();

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

        player.forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        //if (orbit.distance < 1)
        //{
        //    if (mr.sharedMaterial != alternateMaterial)
        //        mr.sharedMaterial = alternateMaterial;
        //}
        //else
        //{
        //    if (mr.sharedMaterial != originalMaterial)
        //        mr.sharedMaterial = originalMaterial;
        //}
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
            //orbit.idealDistance -= Time.deltaTime * zoomSensi;
            //Debug.Log("Crashed.");
        }
    }

    void LockView()
    {
        lockView = !lockView;
        if (lockView)
        {
            //Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
