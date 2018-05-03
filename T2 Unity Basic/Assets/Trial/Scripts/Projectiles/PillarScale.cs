using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScale : MonoBehaviour {

    [SerializeField] float timeToLive = 1f;
    [SerializeField] float maxWidth = 1f;
    [SerializeField] PillarMoving main;
    [SerializeField] Light lamp;
    public MotionController source;

    float speed;
    float curWidth = 0;
    [SerializeField]float maxIntensity = 15f;

	// Use this for initialization
	void Start () {
        speed = maxWidth / timeToLive;
        
	}
	
	// Update is called once per frame
	void Update () {

        curWidth += speed * Time.deltaTime;

        if (curWidth  < 0)
        {
            Destroy(transform.parent.gameObject);
        }

        if (curWidth > maxWidth)
        {
            curWidth = maxWidth;
            speed = -speed;
        }
        lamp.intensity = maxIntensity * curWidth / maxWidth;
        transform.localScale = new Vector3(curWidth, 200, curWidth);

	}

    readonly int textCycle = 5;
    int textCD = 0;

    private void FixedUpdate()
    {
        textCD++;
        if (textCycle == textCD)
            textCD = 0;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Unit")
        {
            AttrController attr = other.GetComponent<AttrController>();
            if (!attr.isAlive)
                return;

            attr.TakeDamage(main.DPS * Time.deltaTime);

            AIController ai = other.GetComponent<AIController>();
            if (ai)
            {
                ai.targetList.Add(source);
            }

            if (0 == textCD)
            {
                NGUI_SplashText.CreateText(other.transform.position + 0.5f * Vector3.up, main.DPS * (Time.deltaTime * textCycle));
            }
        }
        
    }
}
