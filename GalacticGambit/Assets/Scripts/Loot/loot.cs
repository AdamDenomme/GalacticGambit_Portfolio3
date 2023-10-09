using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loot : MonoBehaviour
{
    [SerializeField] List<Item> possibleLoot;

    public Item lootItem;


    private void Start()
    {
        lootItem = possibleLoot[Random.Range(0, possibleLoot.Count)];
    }

}
