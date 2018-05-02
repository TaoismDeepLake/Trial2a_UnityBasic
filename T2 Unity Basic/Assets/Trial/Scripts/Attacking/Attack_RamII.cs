using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_RamII : MonoBehaviour {

    [SerializeField] float initDistance = 1f;
    [SerializeField] float projectileSpeed = 3f;
    [SerializeField] AttrController attr;
    [SerializeField] GameObject prefab = null;
    [SerializeField] MotionController motion;

	// Use this for initialization
	void Start () {
        if (null == prefab)
            Debug.LogError("Please assign the prefab");

        if (null == motion)
            motion = GetComponent<MotionController>();

        motion.Attack += Attacking;
        attr = GetComponent<AttrController>();
	}
	
    void Attacking()
    {
        GameObject g = Instantiate(prefab, transform.position + initDistance * transform.forward, Quaternion.identity);
        PillarMoving pillar = g.GetComponent<PillarMoving>();
        pillar.speed = transform.forward * projectileSpeed;
        pillar.DPS = attr.atk * 1.5f;

        PillarScale ps = g.GetComponentInChildren<PillarScale>();
        ps.source = motion;
    }
}
