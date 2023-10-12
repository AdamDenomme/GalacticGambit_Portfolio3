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


    Sprite icon;
    string itemName;
    string description;
    float price;
    bool mouseOver;
    bool isHoverMenuSpawned;
    int count;

    bool isSelected;
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isSelected)
        {
            border.gameObject.SetActive(true);
            isSelected = true;
        }
        else
        {
            border.gameObject.SetActive(false);
            isSelected = false;
        }
        
    }
    void updateSlot()
    {
        itemNameText.text = itemName;
        descriptionText.text = description;
        priceText.text = "$" + price.ToString();
        iconImage.sprite = icon;
        image.sprite = icon;
        itemCount.text = count.ToString();
    }

    public void updateItemInSlot(string item, string desc, float itemPrice, Sprite iconV, int countV)
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

        updateSlot();
    }
}
