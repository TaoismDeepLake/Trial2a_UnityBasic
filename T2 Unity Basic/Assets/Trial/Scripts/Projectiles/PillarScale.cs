using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScale : MonoBehaviour {

    [SerializeField] float timeToLive = 1f;
    [SerializeField] float maxWidth = 1f;
    [SerializeField] PillarMoving main;

    public MotionController source;

    float speed;
    float curWidth = 0;

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

        transform.localScale = new Vector3(curWidth, 200, curWidth);

	}

    private void OnTriggerStay(Collider other)
    {



            if (other.tag == "Unit")
            {
                AttrController attr = other.GetComponent<AttrController>();
                attr.TakeDamage(main.DPS * Time.deltaTime);
            }
        
    }
}
