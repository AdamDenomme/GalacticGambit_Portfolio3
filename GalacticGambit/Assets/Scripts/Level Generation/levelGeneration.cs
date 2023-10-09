using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGeneration : MonoBehaviour
{
    [SerializeField] Navigation navigation;
    [SerializeField] GameObject navigationMenu;
    public static levelGeneration instance;
    public List<GameObject> encounters;
    public List<GameObject> lootSpawned;
    public GameObject wormhole;
    [SerializeField] List<level> levels;
    List<GameObject> possibleEncounters;
    List<GameObject> possibleLoot;
    Material skybox;
    float lootRatio;
    public int timeToCompleteMax;
    public int timeToCompleteMin;
    public int timeToComplete;
    public int numberOfEncountersMax;
    public int numberOfEncountersMin;
    public int numberOfEncounters;

    GameObject currentEncounter;

    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        navigation.generateNavigationMenu();
    }

    public void selectLevel()
    {
        int random = Random.Range(0, levels.Count);

        possibleEncounters = levels[random].encounters;
        possibleLoot = levels[random].loot;
        lootRatio = levels[random].lootRatio;
        timeToCompleteMax = levels[random].timeToCompleteMax;
        timeToCompleteMin = levels[random].timeToCompleteMin;
        skybox = levels[random].skybox;
        numberOfEncountersMax = levels[random].numberOfEncountersMax;
        numberOfEncountersMin = levels[random].numberOfEncountersMin;
        setLevel();
    }

    public void setLevel()
    {
        RenderSettings.skybox = skybox;
        timeToComplete = Random.Range(timeToCompleteMin, timeToCompleteMax);

        numberOfEncounters = Random.Range(numberOfEncountersMin, numberOfEncountersMax);

        for(int i = 0; i < numberOfEncounters; i++)
        {
            int selectedEncounter = Random.Range(0, possibleEncounters.Count);
            encounters.Add(possibleEncounters[selectedEncounter]);
        }
        for(int i = 0; i < possibleLoot.Count; i++)
        {
            int selectedLoot = Random.Range(0, possibleLoot.Count);
            lootSpawned.Add(possibleLoot[selectedLoot]);
        }
    }

    void spawnEncounter()
    {
        if(currentEncounter != null)
        {
            Destroy(currentEncounter);
        }
        currentEncounter = Instantiate(encounters[navigation.currentWaypointIndex].gameObject);
    }

    public void nextEncounter()
    {
        
        if (currentEncounter != null)
        {
            navigation.nextWaypoint();
            navigationMenu.gameObject.SetActive(false);
            StartCoroutine(shipManager.instance.playWarp());
        }
        spawnEncounter();

    }

    public void extract()
    {
        navigationMenu.gameObject.SetActive(false);
        if (currentEncounter != null) 
        {
            Destroy(currentEncounter);
            StartCoroutine(shipManager.instance.playWarp());
        }
        currentEncounter = Instantiate(wormhole);
        currentEncounter.transform.parent = null;
        currentEncounter.SetActive(true);
        
    }

    public void removeWormhole()
    {
        Destroy(currentEncounter);
    }
}
