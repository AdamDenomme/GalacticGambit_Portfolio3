using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wormhole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ship!");
        StartCoroutine(shipManager.instance.extract());
    }
}
