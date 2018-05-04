using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Vector3 speed = Vector3.zero;

    public GameObject explosion;
    public MotionController source;

    /// <summary>
    /// Damage per second on contact
    /// </summary>
    public float damage = 15f;

    private void Start()
    {
        transform.forward = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //AI, projectiles, editor
        if (other.gameObject.layer == 13 || other.gameObject.layer == 14 || other.gameObject.layer == 8)
        {
            Debug.Log("Passed vision");
            return;
        }

        Debug.LogFormat("Bullt hit {0}, tag = {1}.", other.gameObject.name, other.gameObject.tag);

        if (other.tag == "Unit")
        {
            AttrController attr = other.GetComponent<AttrController>();
            if (!attr.isAlive)
                return;

            MotionController mc = other.GetComponent<MotionController>();
            if (mc)
            {
                if (source && mc.teamIndex == source.teamIndex)
                    return;

                AIController ai = other.GetComponentInChildren<AIController>();
                if (ai)
                {
                    ai.targetList.Add(source);
                }
            }


            attr.TakeDamage(damage);

            NGUI_SplashText.CreateText(transform.position, damage);
        }

        
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        explosion.SetActive(true);
        explosion.transform.SetParent(transform.parent);
        explosion.transform.SetAsLastSibling();
        explosion.GetComponent<Fading>().enabled = true;
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(speed * Time.deltaTime, Space.World);
    }
}
