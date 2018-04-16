using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [ExecuteInEditMode]
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("A " + other.name);
    }

    [ExecuteInEditMode]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("B " + other.name);
    }
}