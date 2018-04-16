using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUI_Bar : MonoBehaviour {

    Camera cam;

    public Transform targetTrans;
    public float floatDistance = 2.3f;

    public UISprite bar;

    private void Start()
    {
        cam = Camera.main;
    }

    public void SetRatio(float ratio)
    {
        bar.fillAmount = Mathf.Clamp(ratio, 0f, 1f);
    }

    // Update is called once per frame
    void Update () {
        if (null == targetTrans)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = targetTrans.position + floatDistance * Vector3.up;
        transform.LookAt(cam.transform);
	}
}
