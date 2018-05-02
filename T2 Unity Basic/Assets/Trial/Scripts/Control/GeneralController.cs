using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : MonoBehaviour {

    public static GeneralController instance;

    public GameObject splashTextPrefab;

    private void Awake()
    {
        instance = this;
    }
}
