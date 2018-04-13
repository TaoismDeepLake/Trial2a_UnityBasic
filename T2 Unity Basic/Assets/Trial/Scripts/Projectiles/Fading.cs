using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

    /// <summary>
    /// time to live
    /// </summary>
    public float fading = 5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Fade());
	}
	
	IEnumerator Fade()
    {
        yield return new WaitForSeconds(fading);
        Destroy(gameObject);
    }
}
