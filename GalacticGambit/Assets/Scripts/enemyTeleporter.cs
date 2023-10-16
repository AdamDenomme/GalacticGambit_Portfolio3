
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class enemyTeleporter : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float randomSpawnChance;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject toSpawn;
    [SerializeField] List<Transform> spawnPoints;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy Ship"))
        {
            teleport();
        }
    }

    public void teleport()
    {
        float holder = Random.Range(0, 1);

        if (holder <= randomSpawnChance)
        {
            Instantiate(toSpawn, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
        }
        else
        {
            return;
        }
    }
}
