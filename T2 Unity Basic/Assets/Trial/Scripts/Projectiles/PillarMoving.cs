using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMoving : MonoBehaviour {

    public Vector3 speed = Vector3.zero;

	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime);
	}
}
