using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : MonoBehaviour {

    public static GeneralController instance;

    public static MotionController playerMC;

    public GameObject splashTextPrefab;

    public bool useEasyTouch = false;

    private void Awake()
    {
        instance = this;
    }

    public void HaltPlayer()
    {
        playerMC.GetComponent<PlayerAutomation>().StopAI();
    }
}
