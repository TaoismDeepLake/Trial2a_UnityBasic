using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public Vector3 speed = Vector3.zero;

    public GameObject explosion;


    /// <summary>
    /// Damage per second on contact
    /// </summary>
    public float damage = 15f;

    private void Start()
    {
        transform.forward = speed;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Unit")
        {
            AttrController attr = other.GetComponent<AttrController>();
            attr.TakeDamage(damage);
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
