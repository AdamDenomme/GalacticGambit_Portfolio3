using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class turrets : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField] Renderer model;
    public NavMeshAgent agent;
    [SerializeField] GameObject selectedIndicator;
    [SerializeField] int health;
    int startHealth;

    public IInteractable selectedInteraction;

    private Vector3 aimPos;
    public GameObject turret;


    bool isManned;
    bool isSelected;

    GameObject inGameWaypointMarker;

    void Awake()
    {
        health = startHealth;
    }

    void Update()
    {
        if (inGameWaypointMarker != null)
        {
            if ((agent.destination - transform.position).magnitude < .2f)
            {
                Destroy(inGameWaypointMarker);
                inGameWaypointMarker = null;
            }
        }

        // aimPos = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        // Vector3 diff = aimPos - turret.transform.position;
        // float turretRotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //
        // turret.transform.rotation = Quaternion.Euler(0.0f, 0.0f, turretRotation);
    }


    // Toggle Turret Selection
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
        if (other.TryGetComponent(out interactable))
        {
            Debug.Log("Interactable!");
            interactable.onInteractable(true);
            selectedInteraction = interactable;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (selectedInteraction != null)
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


}
