using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    [SerializeField] CamControl cam;
    [SerializeField] GameObject playerPrefab;
    

    public GameObject CreatePlayer()
    {
        GameObject g = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        cam.pivot = GameObject.Find("Pivot").transform;
        cam.player = g.transform;
        return g;


    }

    private void Awake()
    {
        if (null == cam)
        {
            cam = FindObjectOfType<CamControl>();
        }

    }

    // Use this for initialization
    void Start () {
        CreatePlayer();
	}
	
	
}
