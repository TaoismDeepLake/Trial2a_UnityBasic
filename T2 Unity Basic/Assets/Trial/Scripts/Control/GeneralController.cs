using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : MonoBehaviour {

    public static GeneralController instance;

    public GameObject splashTextPrefab;

    public bool useEasyTouch = false;

    private void Awake()
    {
        instance = this;
    }
}
