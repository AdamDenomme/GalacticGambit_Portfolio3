using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shieldGenerator : MonoBehaviour, IDamage, IInteractable
{
    [SerializeField] string interactionText;
    [SerializeField] string interactionKeyCode;
    [SerializeField] GameObject sitPosition;
    [SerializeField] GameObject healthIndicator;
    [SerializeField] Image healthBar;
    [SerializeField] shield shieldScript;
    [SerializeField] float health;

    [SerializeField] Light shieldLight;
    [SerializeField] ParticleSystem shieldParticleSystem;
    [SerializeField] ParticleSystem shieldParticleSystem2;

    [SerializeField] int baseRepairTime;

    [SerializeField] List<Interaction> interactionsPossible;

    [SerializeField] float powerUsage;

    private bool interactable;
    private bool isSitting;

    private float origHP;

    private bool isDisabled;
    private bool updatePower;

    private bool isManned;

    void Start()
    {
        updatePower = true;
        origHP = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable && Input.GetButtonDown(interactionKeyCode))
        {
            onInteract();
        }

        if (isSitting && Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Stop Interaction");
        }

        if (health < origHP && healthIndicator.gameObject.activeSelf != true)
        {
            healthIndicator.gameObject.SetActive(true);
        }
        else if (health >= origHP && healthIndicator.gameObject.activeSelf != false)
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
        healthBar.fillAmount = health / origHP;
        if (health <= 0)
        {
            toggleEnabled();
        }
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
        health = origHP;
    }

    public void repair(int experience = 0, int modifier = 0)
    {
        float timeReduction = 0;
        for (int i = 0; i <= experience; i++)
        {
            timeReduction += .1f;
        }

        StartCoroutine(repairComponent((int)(baseRepairTime - (baseRepairTime * timeReduction) + modifier)));
        toggleEnabled();
    }

    public void man()
    {
        Debug.Log("Man");
        if (!isDisabled)
        {
            //Toggle Navigation Menu
            isManned = true;
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
        if (isDisabled)
        {
            isDisabled = false;
            updatePower = true;
            shieldScript.toggleState(false);
            shieldLight.enabled = true;
            shieldParticleSystem.Play();
            shieldParticleSystem2.Play();
        }
        else
        {
            isDisabled = true;
            updatePower = true;
            shieldScript.toggleState(true);
            shieldLight.enabled = false;
            shieldParticleSystem.Stop();
            shieldParticleSystem2.Stop();
        }
    }

    public void back()
    {
        gamemanager.instance.toggleInteractionMenu(false);
    }
    public bool amIEnabled()
    {
        return isDisabled;
    }
}
