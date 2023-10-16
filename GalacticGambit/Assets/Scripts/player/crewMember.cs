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

[System.Serializable]
public class crewMember : MonoBehaviour, IDamage
{
    [Header("--- Components ---")]
    public NavMeshAgent agent;
    [SerializeField] GameObject selectedIndicator;
    [SerializeField] GameObject waypointMarker;
    [SerializeField] Renderer model;
    private Vector3 medBay = new Vector3(-1, -6.5f ,3);

    [Header("--- Stats ---")]
    public int repairExperience;
    public int repairModifier;
    [SerializeField] public int health;
    public int startHealth;
    public TMP_Text crewStability;

    [Header("--- Shootie Gat ---")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int shootAngle;
    [SerializeField] float shootRate;

    GameObject targetLock;
    bool isShooting;
    bool isDead;
    bool isSelected;
    GameObject inGameWaypointMarker;
    public IInteractable selectedInteraction;
    private Vector3 enemyDirection;
    private float angleToEnemy;

    public static List<crewMember> list = new List<crewMember>();


    private void Start()
    {
        isDead = false;
        startHealth = health;
        stabilityCrew();
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
        stabilityCrew();

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

    private void OnTriggerStay(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            if (targetLock == null)
            {
                targetLock = other.gameObject;
            }
            else if (targetLock != null)
            {
                enemyDirection = targetLock.transform.position - (transform.position - Vector3.down).normalized;
                angleToEnemy = Vector3.Angle(new Vector3(enemyDirection.x, 0, enemyDirection.z), transform.forward);
                
                transform.LookAt(enemyDirection);

                if(!isShooting)
                {
                    StartCoroutine(shoot());
                }
            }
            else 
            {

            }
        }
    }
    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(flashDamage());
        updateGameUI();

        if (health <= 0)
        {
            isDead = true;
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

    public void walktoMedBay()
    {
        if (inGameWaypointMarker != null)
        {
            agent.SetDestination(medBay);
            Destroy(inGameWaypointMarker);
            inGameWaypointMarker = null;
            inGameWaypointMarker = Instantiate(waypointMarker, medBay, Quaternion.identity);
            inGameWaypointMarker.transform.parent = null;
        }
        else
        {
            agent.SetDestination(medBay);
            inGameWaypointMarker = Instantiate(waypointMarker, medBay, Quaternion.identity);
            inGameWaypointMarker.transform.parent = null;
        }
        
        Destroy(inGameWaypointMarker);
    }


    public void stabilityCrew()
    {
        if (health > startHealth * .75f)
        {
            crewStability.text = "STABLE";
            crewStability.color = Color.green;
        }

        if (health <= startHealth * .5f)
        {
            crewStability.text = "DAMAGED";
            crewStability.color = Color.yellow;
        }

        if (health <= startHealth * .25f)
        {
            crewStability.text = "CRITICAL";
            crewStability.color = Color.red;
        }
    }

    public void Save()
    {
        Vector3 savedPosition = transform.position;
        int savedHP = health;
        IInteractable savedinteraction = selectedInteraction;
        //sss
        string[] data = new string[]
        {
            ""+savedPosition,
            ""+savedHP,
            ""+savedinteraction
        };

        string json = string.Join("\n", data);
        File.WriteAllText(Application.persistentDataPath + "/save.txt", json);
    }

    
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //Function for damage testing.

    //IEnumerator damageTest()
    //{
    //    yield return new WaitForSeconds(5);
    //    takeDamage(1);
    //}
}
