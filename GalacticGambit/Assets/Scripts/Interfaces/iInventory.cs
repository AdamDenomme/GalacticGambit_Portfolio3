using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public void addItem(Item item, int ammount);
    public void removeItem(Item item, int ammount);
    public bool containsItem(Item item);
}
