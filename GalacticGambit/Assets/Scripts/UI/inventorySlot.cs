using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text itemCount;
    [SerializeField] Image iconImage;
    [SerializeField] Image image;
    [SerializeField] Image border;
    [SerializeField] Sprite defaultImage;

    [Header("--- Do Not Assign ---")]
    public Item inventoryItem;
    public bool canBuy;

    Sprite icon = null;
    string itemName = "";
    string description = "";
    public float price = 0;
    bool mouseOver;
    bool isHoverMenuSpawned;
    int count = 0;

    bool isSelected;

    void Start()
    {
        updateSlot(true);
    }
    
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
        updateSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        updateSlot();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(shipManager.instance.inventory.selectedInventorySlot == null)
        {
            mouseOver = false;
            itemNameText.text = "";
            descriptionText.text = "";
            priceText.text = "";
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isSelected)
        {
            border.gameObject.SetActive(true);
            isSelected = true;
            shipManager.instance.inventory.iAmSelectedInventorySlot(this);
        }
        else
        {
            border.gameObject.SetActive(false);
            isSelected = false;
            shipManager.instance.inventory.selectedInventorySlot = null;
        }
        
    }

    public void unSelect()
    {
        isSelected = false;
        border.gameObject.SetActive(false);
    }

    void updateSlot(bool start = false)
    {
        if (shipManager.instance.inventory.selectedInventorySlot == null && !start && inventoryItem != null)
        {
            itemNameText.text = itemName;
            descriptionText.text = description;
            priceText.text = "$" + price.ToString();
            iconImage.sprite = icon;
            image.sprite = icon;
            if (itemCount != null)
            {
                itemCount.text = count.ToString();
            }
        }else if (shipManager.instance.inventory.selectedInventorySlot != null && !start && inventoryItem != null)
        {
            iconImage.sprite = icon;
            image.sprite = icon;
            if (itemCount != null)
            {
                itemCount.text = count.ToString();
            }
        }
        else if (start || inventoryItem == null)
        {
            itemNameText.text = "";
            descriptionText.text = "";
            priceText.text = "";
            if (itemCount != null)
            {
                itemCount.text = "0";
            }
        }
        if(count <= 0)
        {
            itemNameText.text = "";
            descriptionText.text = "";
            priceText.text = "";
            if(itemCount != null)
            {
                itemCount.text = "0";
            }
            image.sprite = defaultImage;
        }
    }

    public void updateItemInSlot(string item, string desc, float itemPrice, Sprite iconV, int countV, Item invItem)
    {
        itemName = item;
        Debug.Log(itemName);
        description = desc;
        Debug.Log(description);
        price = itemPrice;
        Debug.Log(price);
        icon = iconV;
        Debug.Log(icon);
        count = countV;
        Debug.Log(count);
        inventoryItem = invItem;
        Debug.Log(inventoryItem);

        updateSlot();
    }

    public void clearSlot()
    {
        itemNameText.text = "";
        descriptionText.text = "";
        priceText.text = "";
        itemCount.text = "0";
        iconImage.sprite = null;
        icon = null;
    }
}
