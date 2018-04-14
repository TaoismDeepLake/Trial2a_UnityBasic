using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_FireBallThrow : MonoBehaviour {

    [SerializeField] float initDistance = 1f;
    [SerializeField] float initHeight = 1f;
    [SerializeField] float projectileSpeed = 4f;
    [SerializeField] float projectileRange = 4f;
    [SerializeField] AttrController attr;
    [SerializeField] MotionController mc;
    [SerializeField] GameObject prefab = null;

    // Use this for initialization
    void Start()
    {
        if (null == prefab)
            Debug.LogError("Please assign the prefab");
        if (null == mc)
            mc = GetComponent<MotionController>();
        mc.Attack += Attacking;
        attr = GetComponent<AttrController>();
    }



    void Attacking()
    {
        GameObject g = Instantiate(prefab, transform.position + initDistance * transform.forward + initDistance * transform.up, Quaternion.identity);
        Fireball projectile = g.GetComponent<Fireball>();
        projectile.speed = mc.atkDirection * projectileSpeed;
        projectile.damage = attr.atk;
        g.GetComponent<Fading>().fading = projectileRange / projectileSpeed;
    }
}
