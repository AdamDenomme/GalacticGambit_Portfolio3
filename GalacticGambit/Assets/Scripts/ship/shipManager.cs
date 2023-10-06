using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class shipManager : MonoBehaviour, IDamage
{
    public static shipManager instance;

    [SerializeField] List<GameObject> thrusters;

    [Header("--- Ship Stats ---")]
    int health;
    [SerializeField] int maxHealth;

    [Header("--- Power System ---")]
    [SerializeField] Image powerAvailableIndicator;
    [SerializeField] TMP_Text powerAvailableText;
    [SerializeField] Image reservePowerIndicator;
    [SerializeField] TMP_Text reservePowerText;
    public float powerAvailable;
    public float powerConsumption;
    //Battery storage
    float reservePower;
    float reservePowerCapacity;

    [Header("--- Hull ---")]
    [SerializeField] Image hullIndicator;
    [SerializeField] TMP_Text hullText;

    [Header("--- Sub Systems ---")]
    [SerializeField] pilotSeat pilotSeat;
    //public shipController shipController;

    [Header("--- Shield System ---")]
    [SerializeField] shield shield;
    [SerializeField] Image shieldIndicator;
    [SerializeField] TMP_Text shieldText;
    //Applied when a subsystem is manned by a crew member.
    [Header("--- Modifiers ---")]
    float shieldModifier;
    float pilotModifier;
    float powerModifier;

    [Header("--- Ship Inventory ---")]
    playerInventory inventory;


    bool checkShipIsRunning;
    void Awake()
    {
        instance = this;
        health = maxHealth;
    }

    private void Start()
    {
        pilotSeat.takeDamage(10);
    }


    void Update()
    {
        if (!checkShipIsRunning)
        {
            StartCoroutine(checkShip());
        }
        
    }

    IEnumerator checkShip()
    {
        checkShipIsRunning = true;
        //Apply bonus for manned system.
        if (pilotSeat.amIManned())
        {
            pilotModifier = .3f;
        }
        else
        {
            pilotModifier = 0;
        }

        //Check is system can sustain power draw, if not start drawing from battery.
        if(powerAvailable < powerConsumption)
        {
            float powerDraw = powerConsumption - powerAvailable;
            drawReservePower(powerDraw);
        }

        if(powerAvailable >= powerConsumption)
        {
            float powerDraw = powerAvailable - powerConsumption;
            generateReservePower(powerDraw);
        }

        if(powerConsumption > powerAvailable)
        {
            powerAvailableIndicator.fillAmount = 1;
            
        }
        else
        {
            powerAvailableIndicator.fillAmount = powerConsumption / powerAvailable;
        }

        shieldIndicator.fillAmount = ((float)shield.currentHealth / shield.health);
        shieldText.text = shield.currentHealth.ToString() + " / " + shield.health.ToString();

        hullIndicator.fillAmount = (float)health / maxHealth;
        hullText.text = health.ToString() + " / " + maxHealth.ToString();

        powerAvailableText.text = (powerConsumption).ToString() + " / " + powerAvailable.ToString() + " GW/s";
        reservePowerIndicator.fillAmount = reservePower / reservePowerCapacity;
        reservePowerText.text = reservePower.ToString() + " / " +  reservePowerCapacity.ToString() + " GW";

        yield return new WaitForSeconds(1);
        checkShipIsRunning = false;
    }
    /*|**************************|
     *| --- Power Generation --- |
      |**************************|*/
    public void generatePower(float amount)
    {
        powerAvailable += amount;
    }
    public void stopGeneratingPower(float amount)
    {
        powerAvailable -= amount;
    }
    public void drawPower(float amount)
    {
        powerConsumption += amount;
    }
    public void stopDrawingPower(float amount)
    {
        powerConsumption -= amount;
    }

    public void addReserveCapacity(float amount)
    {
        reservePowerCapacity += amount;
    }
    public void removeReserveCapacity(float amount)
    {
        reservePowerCapacity -= amount;
    }
    void generateReservePower(float amount)
    {
        if (reservePower + amount < reservePowerCapacity)
        {
            reservePower += amount;
        }else if (reservePower + amount > reservePowerCapacity && reservePower != reservePowerCapacity)
        {
            reservePower += reservePowerCapacity - reservePower;
        }
        
    }
    void drawReservePower(float amount)
    {
        reservePower -= amount;
    }
    void updatePowerUI()
    {

    }

    IEnumerator explode()
    {
        //Play particle effect, play sound effect.
        //Zoom camera out.
        yield return new WaitForSeconds(5);
        //Display lose menu
        Destroy(transform.gameObject);
    }

    public void takeDamage(int amount)
    {
        //Debug.Log("Take damage");
        health -= amount;

        if(health <= 0)
        {
            StartCoroutine(explode());
        }

    }



}
