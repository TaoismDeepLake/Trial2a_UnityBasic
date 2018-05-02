using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

    /// <summary>
    /// Uses the finger gesture plugin?
    /// </summary>

    [SerializeField] CamControl cam;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] NGUI_Bar playerBar;


    [SerializeField] CamControlEZ camAssist;

    public GameObject CreatePlayer()
    {
        GameObject g = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        if (GeneralController.instance.useEasyTouch)
        {
            PlayerEasyToucher.instance.InitEasyTouch(g);
            camAssist.pivot = GameObject.Find("Pivot").transform;
            camAssist.player = g.transform;
        }
        else
        {
            cam.pivot = GameObject.Find("Pivot").transform;
            cam.player = g.transform;
        }

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

        if (null == camAssist)
        {
            camAssist = FindObjectOfType<CamControlEZ>();
        }

    }

    // Use this for initialization
    void Start () {
        CreatePlayer();
	}
	
	
}
