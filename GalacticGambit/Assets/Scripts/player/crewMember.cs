using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] public int health;
    public int startHealth;

    bool isSelected;
    GameObject inGameWaypointMarker;
    public IInteractable selectedInteraction;

    private void Start()
    {
        startHealth = health;
        // This is to take damage WITHOUT killing to test healing.
        //takeDamage(4);
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
        gamemanager.instance.playerHPText.text = health.ToString() + " / " + startHealth.ToString();
        gamemanager.instance.repairModText.text = repairModifier.ToString();
        gamemanager.instance.repairXPText.text = repairExperience.ToString();
    }
    public void resetGameUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = 0;
        gamemanager.instance.playerHPText.text = " ";
        gamemanager.instance.repairModText.text = " ";
        gamemanager.instance.repairXPText.text = " ";
    }
    public IEnumerator heal(int hp)
    {
        if (startHealth > health)
        {
            health += hp;
            updateGameUI();
        }
        yield return new WaitForSeconds(1);
        if (health < startHealth)
        {
            StartCoroutine(heal(hp));
        }
    }
    //Function for damage testing.

    //IEnumerator damageTest()
    //{
    //    yield return new WaitForSeconds(5);
    //    takeDamage(1);
    //}
}
