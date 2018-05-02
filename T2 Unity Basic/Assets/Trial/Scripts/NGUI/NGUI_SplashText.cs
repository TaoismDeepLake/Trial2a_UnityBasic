using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUI_SplashText : MonoBehaviour {
    Camera cam;

    Vector3 dir;
    public float floatSpeed;
    public float life = 1f;
    [SerializeField] UILabel text;

    Vector3 origin;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        transform.position = origin;
        transform.LookAt(cam.transform);
        dir = transform.up;
        floatSpeed *= Vector3.Distance(origin, cam.transform.position);
        
    }
	
    public static NGUI_SplashText CreateText(Vector3 pos, float dmg)
    {
        NGUI_SplashText instance = Instantiate(GeneralController.instance.splashTextPrefab).GetComponent<NGUI_SplashText>();
        //pos +=  Vector3.up;
        instance.SetPos(pos);
        instance.SetText(dmg);


        return instance;
    }


    public void SetPos(Vector3 pos)
    {
        origin = pos;
    }

    public void SetText(float dmg)
    {
        Debug.Log("Text = " + Mathf.RoundToInt(dmg).ToString());
        text.text = Mathf.RoundToInt(dmg).ToString();
    }

    [SerializeField] bool handlePos = true;

	// Update is called once per frame
	void Update () {
        transform.LookAt(cam.transform);
        transform.forward = -transform.forward;

        if (handlePos)
        {
            //handlePos = !handlePos;
            transform.position = origin;
        }

        origin += dir * floatSpeed * Time.deltaTime;
        //transform.Translate(dir * floatSpeed * Time.deltaTime, Space.World);

        life -= Time.deltaTime;
        if (life < 0)
        {
            Debug.Log("Text faded");
            Destroy(gameObject);
        }
    }
}
