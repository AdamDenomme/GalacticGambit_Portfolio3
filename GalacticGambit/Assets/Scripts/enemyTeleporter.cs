using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyTeleporter : MonoBehaviour
{
    [SerializeField] int randomSpawnChance;
    [SerializeField] int spawnChance;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject toSpawn;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Tag: " + other.tag);
        if (other.CompareTag("Enemy Ship"))
        {
            teleport();
        }
    }

    public void teleport()
    {
        int holder = 0;
        System.Random rand = new System.Random();
        holder = rand.Next(randomSpawnChance);
        

        if (holder <= randomSpawnChance / spawnChance)
        {
            Instantiate(toSpawn, spawner.transform.position, spawner.transform.rotation);
        }
        else
        {
            return;
        }
    }
}
