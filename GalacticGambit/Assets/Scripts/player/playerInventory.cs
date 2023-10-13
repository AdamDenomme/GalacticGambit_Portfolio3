using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerInventory : MonoBehaviour, IInventory
{
    public List<KeyValuePair<Item, int>> items = new List<KeyValuePair<Item, int>>();
    [SerializeField] Item currency;
    [SerializeField] List<Image> slots;
    [SerializeField] GameObject inventory;
    [SerializeField] List<Item> shopInventory;
    [SerializeField] List<inventorySlot> shopInventorySlots;
    [SerializeField] List<inventorySlot> playerInventorySlotsForShop;
    [SerializeField] List<Item> possibleInventoryItems;
    [SerializeField] GameObject notEnoughMoneyUI;
    [SerializeField] GameObject noItemSelectedUI;
    [SerializeField] GameObject itemPurchasedUI;
    [SerializeField] GameObject itemSoldUI;
    public int money;
    public GameObject hoverMenu;
    public inventorySlot selectedInventorySlot;


    private void Update()
    {
        if (Input.GetButtonDown("Inventory") && !inventory.activeSelf)
        {
            inventory.SetActive(true);
        }
        else if (Input.GetButtonDown("Inventory") && inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
    }
    public void addItem(Item item, int ammount = 1)
    {
        if (!containsItem(item))
        {
            items.Add(new KeyValuePair<Item, int>(item, 1));
            updateInventory();
        }
        else
        {
            KeyValuePair<Item, int> existingItem = items.Find(kv => kv.Key == item);
            items.Remove(existingItem);
            items.Add(new KeyValuePair<Item, int>(item, existingItem.Value + 1));
            updateInventory();
        }
    }

    public void removeItem(Item item, int ammount = 1)
    {
        if (containsItem(item))
        {
            KeyValuePair<Item, int> existingItem = items.Find(kv => kv.Key == item);
            items.Remove(existingItem);
            if(existingItem.Value < 1)
            {
                updateInventory();
            }
            else
            {
                items.Add(new KeyValuePair<Item, int>(item, existingItem.Value - ammount));
                updateInventory();
            }
            
            
        }
    }

    public bool containsItem(Item item)
    {
        return items.Exists(kv => kv.Key == item);
    }

    public bool hasEnough(int amountRequired = 0)
    {
        if(money >= amountRequired)
        {
            return true;
        }
        return false;
    }
    public void iAmSelectedInventorySlot(inventorySlot inventorySlot)
    {
        if (selectedInventorySlot != null)
        {
            selectedInventorySlot.unSelect();
        }
        selectedInventorySlot = inventorySlot;
    }
    private void updateInventory()
    {
        int i = 0;
        foreach (Image slot in slots)
        {
            if (i < items.Count)
            {
                Item item = items[i].Key;
                inventorySlot iSlot = slot.GetComponent<inventorySlot>();
                Debug.Log(iSlot.gameObject.name);
                iSlot.updateItemInSlot(item.itemName, item.description, item.price, item.sprite, items[i].Value, item);
                i++;
            }
            else
            {
                slot.sprite = null;
            }
        }
    }

    public void tryPurchaseItem()
    {
        if(selectedInventorySlot != null)
        {
            if(hasEnough((int)selectedInventorySlot.price) && selectedInventorySlot.canBuy)
            {
                money -= (int)selectedInventorySlot.price;
                addItem(selectedInventorySlot.inventoryItem);
                StartCoroutine(itemPurchased());

                foreach (inventorySlot slot in playerInventorySlotsForShop)
                {
                    slot.clearSlot();
                }

                int k = 0;
                foreach (inventorySlot slot in playerInventorySlotsForShop)
                {
                    if (k < items.Count)
                    {
                        Item item = items[k].Key;
                        Debug.Log(slot.gameObject.name);
                        slot.updateItemInSlot(item.itemName, item.description, item.price, item.sprite, items[k].Value, item);
                        k++;
                    }
                }
            }
            else
            {
                StartCoroutine(notEnoughMoney());
            }
        }
        else
        {
            StartCoroutine(noItemSelected());
        }
    }

    public void trySellItem()
    {
        if(selectedInventorySlot != null)
        {
            if(containsItem(selectedInventorySlot.inventoryItem) && !selectedInventorySlot.canBuy)
            {
                money += (int)selectedInventorySlot.price;
                removeItem(selectedInventorySlot.inventoryItem);
                StartCoroutine(itemSold());

                foreach(inventorySlot slot  in playerInventorySlotsForShop)
                {
                    slot.clearSlot();
                }

                int k = 0;
                foreach (inventorySlot slot in playerInventorySlotsForShop)
                {
                    if (k < items.Count)
                    {
                        Item item = items[k].Key;
                        Debug.Log(slot.gameObject.name);
                        slot.updateItemInSlot(item.itemName, item.description, item.price, item.sprite, items[k].Value, item);
                        k++;
                    }
                }
            }
        }
        else
        {
            StartCoroutine(noItemSelected());
        }
    }
    public void generateShop()
    {
        int random = Random.Range(0, 17);
        shopInventory.Clear();
        for(int i = 0; i < random; i++)
        {
            shopInventory.Add(possibleInventoryItems[Random.Range(0, possibleInventoryItems.Count)]);
        }

        int j = 0;
        foreach (inventorySlot slot in shopInventorySlots)
        {
            if (j < shopInventory.Count)
            {
                Item item = shopInventory[j];
                inventorySlot iSlot = slot.GetComponent<inventorySlot>();
                Debug.Log(iSlot.gameObject.name);
                iSlot.updateItemInSlot(item.itemName, item.description, item.price, item.sprite, 1, item);
                j++;
            }
        }
        int k = 0;
        foreach(inventorySlot slot in playerInventorySlotsForShop)
        {
            if (k < items.Count)
            {
                Item item = items[k].Key;
                Debug.Log(slot.gameObject.name);
                slot.updateItemInSlot(item.itemName, item.description, item.price, item.sprite, items[k].Value, item);
                k++;
            }
        }
    }

    IEnumerator notEnoughMoney()
    {
        notEnoughMoneyUI.SetActive(true);
        yield return new WaitForSeconds(2);
        notEnoughMoneyUI.SetActive(false);
    }
    IEnumerator noItemSelected()
    {
        noItemSelectedUI.SetActive(true);
        yield return new WaitForSeconds(2);
        noItemSelectedUI.SetActive(false);
    }
    IEnumerator itemPurchased()
    {
        itemPurchasedUI.SetActive(true);
        yield return new WaitForSeconds(2);
        itemPurchasedUI.SetActive(false);
    }
    IEnumerator itemSold()
    {
        itemSoldUI.SetActive(true);
        
        yield return new WaitForSeconds(2);
        itemSoldUI.SetActive(false);
    }
}