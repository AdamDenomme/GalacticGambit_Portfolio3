using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class playerInventory : MonoBehaviour, IInventory
{
    public List<KeyValuePair<Item, int>> items = new List<KeyValuePair<Item, int>>();
    [SerializeField] Item currency;
    [SerializeField] List<Image> slots;
    [SerializeField] GameObject inventory;
    [SerializeField] List<Item> shopInventory;
    [SerializeField] List<inventorySlot> shopInventorySlots;
    [SerializeField] List<inventorySlot> playerInventorySlotsForShop;
    [SerializeField] List<Item> possibleShopItems;
    [SerializeField] List<Item> possibleInventoryItems;
    [SerializeField] GameObject notEnoughMoneyUI;
    [SerializeField] GameObject noItemSelectedUI;
    [SerializeField] GameObject itemPurchasedUI;
    [SerializeField] GameObject itemSoldUI;

    [Header("--- Upgrades ---")]
    [SerializeField] List<Item> weaponUpgrades;
    [SerializeField] List<inventorySlot> weaponUpgradesUI;
    [SerializeField] List<Item> shieldUpgrades;
    [SerializeField] List<inventorySlot> shieldUpgradesUI;
    [SerializeField] List<Item> hullUpgrades;
    [SerializeField] List<inventorySlot> hullUpgradesUI;
    [SerializeField] List<Item> powerUpgrades;
    [SerializeField] List<inventorySlot> powerUpgradesUI;
    [SerializeField] GameObject tooManyUpgradesUI;
    [SerializeField] GameObject notEquipableItemUI;

    public int money;
    public GameObject hoverMenu;
    public inventorySlot selectedInventorySlot;

    float defaultAttackSpeed;
    int defaultDamage;
    int defaultRotationSpeed;
    int defaultShieldHealth;
    int defaultShieldRegenTime;
    int defaultHullHealth;
    float defaultPowerAvailable;
    float defaultBatteryCapacity;

    bool firstUpgradeRun;

    private void Start()
    {
        
    }
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
            shopInventory.Add(possibleShopItems[Random.Range(5, possibleShopItems.Count)]);
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
    IEnumerator notEquipable()
    {
        notEquipableItemUI.SetActive(true);
        yield return new WaitForSeconds(2);
        notEquipableItemUI.SetActive(false);
    }
    IEnumerator tooManyUpgrades()
    {
        tooManyUpgradesUI.SetActive(true);
        yield return new WaitForSeconds(2);
        tooManyUpgradesUI.SetActive(false);
    }

    public void equipItem()
    {
        Item item = selectedInventorySlot.inventoryItem;
        if (item.isEquipable)
        {
            if(item.equipment.updatetype == Equipment.upgradeType.weaponUpgrade && weaponUpgrades.Count <= 6)
            {
                weaponUpgrades.Add(item);
            }else if (item.equipment.updatetype == Equipment.upgradeType.Shield &&  shieldUpgrades.Count <= 6)
            {
                shieldUpgrades.Add(item);
            }else if (item.equipment.updatetype == Equipment.upgradeType.Hull &&  hullUpgrades.Count <= 6) 
            { 
                hullUpgrades.Add(item);
            }else if (item.equipment.updatetype == Equipment.upgradeType.Power && powerUpgrades.Count <= 6)
            {
                powerUpgrades.Add(item);
            }
            else
            {
                StartCoroutine(tooManyUpgrades());
            }
        }
        else
        {
            StartCoroutine(notEquipable());
        }
        removeItem(item, 1);
        setUpgrades();
    }
    public void setUpgrades()
    {
        if(firstUpgradeRun)
        {
            defaultAttackSpeed = shipManager.instance.turretController.attackSpeed;
            defaultDamage = shipManager.instance.turretController.additionalDamage;
            defaultRotationSpeed = shipManager.instance.turretController.rotationSpeed;
            defaultShieldHealth = shipManager.instance.shield.health;
            defaultShieldRegenTime = shipManager.instance.shield.regenTime;
            defaultHullHealth = shipManager.instance.maxHealth;
            defaultPowerAvailable = shipManager.instance.powerAvailable;
            defaultBatteryCapacity = shipManager.instance.reservePowerCapacity;
            firstUpgradeRun = false;
        }

        shipManager.instance.turretController.attackSpeed = defaultAttackSpeed;
        shipManager.instance.turretController.additionalDamage =  defaultDamage;
        shipManager.instance.turretController.rotationSpeed=  defaultRotationSpeed;
        shipManager.instance.shield.health = defaultShieldHealth;
        shipManager.instance.shield.regenTime = defaultShieldRegenTime;
        shipManager.instance.maxHealth = defaultHullHealth;
        shipManager.instance.powerAvailable = defaultPowerAvailable;
        shipManager.instance.reservePowerCapacity = defaultBatteryCapacity;

        foreach (Item item in weaponUpgrades)
        {
            shipManager.instance.turretController.attackSpeed += item.equipment.attackSpeed;
            shipManager.instance.turretController.additionalDamage += item.equipment.damage;
            shipManager.instance.turretController.rotationSpeed += item.equipment.turretRotationSpeed;
        }

        foreach(Item item in shieldUpgrades)
        {
            shipManager.instance.shield.health += item.equipment.shieldCapacity;
            shipManager.instance.shield.regenTime += item.equipment.shieldRechargeRate;

        }
        foreach(Item item in hullUpgrades)
        {
            shipManager.instance.health += item.equipment.hullHealthPoints;
            shipManager.instance.maxHealth += item.equipment.hullHealthPoints;
        }
        foreach(Item item in powerUpgrades)
        {
            shipManager.instance.powerAvailable += item.equipment.powerOutput;
            shipManager.instance.reservePowerCapacity += item.equipment.batterCapacity;
        }
        updateUpgradesUI();
    }

    public void updateUpgradesUI()
    {
        for(int i = 0; i < weaponUpgrades.Count-1; i++)
        {
            Item item = weaponUpgrades[i];
            weaponUpgradesUI[i].updateItemInSlot(item.itemName, item.description, item.price, item.sprite, 1, item);
        }
        for (int i = 0; i < shieldUpgrades.Count-1; i++)
        {
            Item item = shieldUpgrades[i];
            shieldUpgradesUI[i].updateItemInSlot(item.itemName, item.description, item.price, item.sprite, 1, item);
        }
        for (int i = 0; i < powerUpgrades.Count-1; i++)
        {
            Item item = powerUpgrades[i];
            powerUpgradesUI[i].updateItemInSlot(item.itemName, item.description, item.price, item.sprite, 1, item);
        }
        for (int i = 0; i < hullUpgrades.Count-1; i++)
        {
            Item item = hullUpgrades[i];
            hullUpgradesUI[i].updateItemInSlot(item.itemName, item.description, item.price, item.sprite, 1, item);
        }
    }
}