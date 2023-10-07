using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Sprite iconImage;

    Sprite icon;
    string itemName;
    string description;
    float price;
    bool mouseOver;
    bool isHoverMenuSpawned;
    // Update is called once per frame
    void Update()
    {
        if (mouseOver && !isHoverMenuSpawned)
        {
            isHoverMenuSpawned = true;
            shipManager.instance.inventory.hoverMenu.gameObject.SetActive(true);
        }
        else if (!mouseOver && isHoverMenuSpawned)
        {
            isHoverMenuSpawned = false;
            shipManager.instance.inventory.hoverMenu.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Over");
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void updateItemInSlot(string item, string desc, float itemPrice, Sprite iconV)
    {
        itemName = item;
        description = desc;
        price = itemPrice;
        icon = iconV;
    }
}
