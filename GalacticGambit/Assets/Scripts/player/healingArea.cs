using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingArea : MonoBehaviour
{
    [SerializeField] int healthPerSecond;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Crew Mate"))
        {
            crewMember crew;
           if (gamemanager.instance.topDownPlayerController.selectedCrewMember != null)
            {
                crew = gamemanager.instance.topDownPlayerController.selectedCrewMember;
                StartCoroutine(crew.heal(healthPerSecond));
                
            }

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        crewMember crew = gamemanager.instance.topDownPlayerController.selectedCrewMember;
        crew.StopAllCoroutines();
    }
   
}
