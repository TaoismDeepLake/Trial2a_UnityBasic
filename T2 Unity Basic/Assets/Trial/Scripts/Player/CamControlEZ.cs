using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this onto a camera
/// </summary>
public class CamControlEZ : MonoBehaviour {

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


    }

    private void Start()
    {
        pitchMax *= Mathf.Deg2Rad;
        pitchMin *= Mathf.Deg2Rad;

        mr = player.GetComponentInChildren<SkinnedMeshRenderer>();
        originalMaterial = mr.sharedMaterial;

        player.GetComponent<AttrController>().Death += Death;

    }

    private void Death()
    {
        pivot = null;
    }

    // Update is called once per frame
    void Update () {

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


    /// <summary>
    /// Make sure you have a kinametic-rigidbody3D attached.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);

        if ("Terrain" == other.tag)
        {
            distance -= Time.deltaTime * zoomSensi;
            //Debug.Log("Crashed.");
        }
    }


}
