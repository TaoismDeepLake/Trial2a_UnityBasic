using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RamII : MonoBehaviour {

    [SerializeField] float initDistance = 1f;
    [SerializeField] float projectileSpeed = 3f;

    [SerializeField] GameObject prefab = null;

	// Use this for initialization
	void Start () {
        if (null == prefab)
            Debug.LogError("Please assign the prefab");

        GetComponent<MotionController>().Attack += Attacking;	
	}
	
    void Attacking()
    {
        GameObject g = Instantiate(prefab, transform.position + initDistance * transform.forward, Quaternion.identity);
        g.GetComponent<PillarMoving>().speed = transform.forward * projectileSpeed;
    }
}
