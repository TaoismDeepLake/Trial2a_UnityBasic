using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarMoving : MonoBehaviour {

    public Vector3 speed = Vector3.zero;

    /// <summary>
    /// Damage per second on contact
    /// </summary>
    public float DPS = 15f;

	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime);
	}

    

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Unit")
        {
            AttrController attr = other.GetComponent<AttrController>();
            attr.TakeDamage(DPS * Time.deltaTime);
        }
    }
}
