using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    [SerializeField] CamControl cam;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] NGUI_Bar playerBar;


    //[SerializeField] CamControlEZ camAssist;

    public GameObject CreatePlayer()
    {
        GameObject g = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        PlayerEasyToucher.instance.InitEasyTouch(g);
        //camAssist.pivot = GameObject.Find("Pivot").transform;
        //camAssist.player = g.transform;

        cam.pivot = GameObject.Find("Pivot").transform;
        cam.player = g.transform;
        cam.Init();

        GeneralController.playerMC =  g.GetComponent<MotionController>();



        AttrController playerAttr = g.GetComponent<AttrController>();
        playerAttr.bar = playerBar;
        //playerBar.
        return g;
    }

    private void Awake()
    {
        if (null == cam)
        {
            cam = FindObjectOfType<CamControl>();
        }

        //if (null == camAssist)
        //{
        //    camAssist = FindObjectOfType<CamControlEZ>();
        //}

    }

    // Use this for initialization
    void Start () {
        CreatePlayer();
	}
	
	
}
