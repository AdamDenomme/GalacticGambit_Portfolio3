using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using JetBrains.Annotations;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using System.IO;
using UnityEngine.XR;
using Unity.VisualScripting;

public class playerShooting : MonoBehaviour
{
    [SerializeField] crewMember crewMate;
   
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            if (crewMate.targetLock == null)
            {
                crewMate.targetLock = other.gameObject;
            }
            else if (crewMate.targetLock != null)
            {

                crewMate.lookAtEnemy();

                if (!crewMate.isShooting)
                {
                    Debug.Log(3);
                    StartCoroutine(crewMate.shoot());
                }
            }
            else
            {
                crewMate.isShooting = false;
                crewMate.targetLock = null;
            }
        }
    }
}
