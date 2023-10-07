using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class inventorySlot : MonoBehaviour
{
    string itemName;
    string description;
    float price;
    bool mouseOver;
    // Update is called once per frame
    void Update()
    {
        if (mouseOver)
        {
            Debug.Log("Mouse over");
        }
    }

    void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void updateItemInSlot(string item, string desc, float itemPrice)
    {
        itemName = item;
        description = desc;
        price = itemPrice;
    }
}
