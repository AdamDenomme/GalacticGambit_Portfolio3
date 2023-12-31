using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int price;
    public string description;
    public bool isEquipable;
    public bool isShipRoom;
    public GameObject roomPrefab;
    public Equipment equipment;
}
