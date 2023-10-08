using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicktoHeal : MonoBehaviour
{
    public GameObject crewMember;
    public GameObject waypointMarker;
    public Vector3 medBay;


    public void walktoMedBay()
    {
        crewMember.transform.position = medBay;
        waypointMarker.transform.position = medBay;
    }
}
