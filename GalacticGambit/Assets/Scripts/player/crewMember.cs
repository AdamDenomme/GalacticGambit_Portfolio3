using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class crewMember : MonoBehaviour, IDamage
{
    [Header("--- Components ---")]
    public NavMeshAgent agent;
    [SerializeField] GameObject selectedIndicator;
    [SerializeField] GameObject waypointMarker;
    [SerializeField] Renderer model;

    [Header("--- Stats ---")]
    public int repairExperience;
    public int repairModifier;
    [SerializeField] int health;
    int startHealth;

    bool isSelected;
    GameObject inGameWaypointMarker;
    public IInteractable selectedInteraction;

    private void Start()
    {
        startHealth = health;
    }
    // Update is called once per frame
    void Update()
    {
        if (inGameWaypointMarker != null)
        {
            if((agent.destination - transform.position).magnitude < .2f)
            {
                Destroy(inGameWaypointMarker);
                inGameWaypointMarker = null;
            }
        }
        // For damage testing.
        //StartCoroutine(damageTest());
    }

    public void moveTo(Vector3 position)
    {
        if(inGameWaypointMarker != null)
        {
            agent.SetDestination(position);
            Destroy(inGameWaypointMarker);
            inGameWaypointMarker = null;
            inGameWaypointMarker = Instantiate(waypointMarker, position, Quaternion.identity);
            inGameWaypointMarker.transform.parent = null;
        }
        else
        {
            agent.SetDestination(position);
            inGameWaypointMarker = Instantiate(waypointMarker, position, Quaternion.identity);
            inGameWaypointMarker.transform.parent = null;
        }
    }

    public void toggleSelected(bool state)
    {
        if (state)
        {
            selectedIndicator.SetActive(true);
            isSelected = true;
        }
        else
        {
            selectedIndicator.SetActive(false);
            isSelected = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable;
        if(other.TryGetComponent(out interactable))
        {
            Debug.Log("Interactable!");
            interactable.onInteractable(true);
            selectedInteraction = interactable;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(selectedInteraction != null)
        {
            selectedInteraction.onInteractable(false);
            selectedInteraction = null;
        }
    }
    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashDamage());
        updateGameUI();

        if (health <= 0)
        {
            StartCoroutine(killCrew());
        }
        
    }
    IEnumerator killCrew()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
    public void updateGameUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)health / startHealth;
    }

    //Function for damage testing.

    //IEnumerator damageTest()
    //{
    //    yield return new WaitForSeconds(5);
    //    takeDamage(1);
    //}
}
