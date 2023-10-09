using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turret : MonoBehaviour, IDamage, IInteractable
{
    [SerializeField] string interactionText;
    [SerializeField] string interactionKeyCode;
    [SerializeField] GameObject sitPosition;
    [SerializeField] GameObject healthIndicator;
    [SerializeField] Image healthBar;
    [SerializeField] turretController controller;

    [SerializeField] int baseRepairTime;

    [SerializeField] List<Interaction> interactionsPossible;

    private bool interactable;
    private bool isSitting;

    private float health = 100;

    private bool isDisabled;

    private bool isManned;
    private bool updatePower;
    [SerializeField] float powerUsage;

    void Update()
    {
        if (interactable && Input.GetButtonDown(interactionKeyCode))
        {
            onInteract();
        }

        if (isSitting && Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Stop Interaction");
            isManned = false;
            controller.isActive = false;

        }

        if (health < 100 && healthIndicator.gameObject.activeSelf != true)
        {
            healthIndicator.gameObject.SetActive(true);
        }
        else if (health >= 100 && healthIndicator.gameObject.activeSelf != false)
        {
            healthIndicator.gameObject.SetActive(false);
        }

        if (!isDisabled && updatePower)
        {
            Debug.Log("Start drawing power: " + powerUsage);
            shipManager.instance.drawPower(powerUsage);
            updatePower = false;
        }
        if (isDisabled && updatePower)
        {
            Debug.Log("Stop drawing power: " + powerUsage);
            shipManager.instance.stopDrawingPower(powerUsage);
            updatePower = false;
        }
    }
    //The function used to display information when an object becomes interactable.
    public void onInteractable(bool state)
    {
        if (state)
        {
            //Debug.Log("Interactable");
            gamemanager.instance.interactionMenu.updateInteractionOptions(interactionsPossible);
            gamemanager.instance.interactionMenu.gameObject.SetActive(true);
            interactable = true;

        }
        else
        {
            Debug.Log("Interaction Off");
            gamemanager.instance.interactionMenu.gameObject.SetActive(false);
            interactable = false;
            gamemanager.instance.interactionMenu.clearList();
        }

    }

    public void takeDamage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = health / 100;
    }

    //The function used to trigger the interaction when an object is interacted with.
    public void onInteract()
    {
        Debug.Log("Interact");
    }

    public bool isInteractable()
    {
        return interactable;
    }

    public string interactionKey()
    {
        return interactionKeyCode;
    }

    private IEnumerator repairComponent(int timeToRepair)
    {
        yield return new WaitForSeconds(timeToRepair);
        health = 100;
    }

    public void repair(int experience = 0, int modifier = 0)
    {
        float timeReduction = 0;
        for (int i = 0; i <= experience; i++)
        {
            timeReduction += .1f;
        }

        StartCoroutine(repairComponent((int)(baseRepairTime - (baseRepairTime * timeReduction) + modifier)));
    }

    public void man()
    {
        Debug.Log("Man");
        if (!isDisabled)
        {
            //Toggle Navigation Menu
            isManned = true;
            controller.isActive = true;
        }
    }

    public bool amIManned()
    {
        if (isManned && !isDisabled)
        {
            return isManned;
        }
        return false;
    }

    public void toggleEnabled()
    {
        isDisabled = !isDisabled;
    }

    public void back()
    {
        gamemanager.instance.toggleInteractionMenu(false);
    }
}
