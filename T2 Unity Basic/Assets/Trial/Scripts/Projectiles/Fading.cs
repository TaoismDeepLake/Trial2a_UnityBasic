using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {
    /// <summary>
    /// Should be a parent of this.
    /// </summary>
    [SerializeField] GameObject toDestroy;
    [SerializeField] bool fadingStarted;
    /// <summary>
    /// time to live
    /// </summary>
    public float fading = 5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Fade());
        fadingStarted = true;
	}
	
	IEnumerator Fade()
    {
        yield return new WaitForSeconds(fading);
        if (toDestroy)
            Destroy(toDestroy);
        else
            Destroy(gameObject);

    }
}
