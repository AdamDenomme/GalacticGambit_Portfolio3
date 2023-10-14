using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class overview : MonoBehaviour
{
    [SerializeField] TMP_Text moneyUI;
    [SerializeField] TMP_Text ammoUI;
    [SerializeField] TMP_Text missleUI;
    // Update is called once per frame
    void FixedUpdate()
    {
        moneyUI.text = "$" + shipManager.instance.inventory.money.ToString();
        ammoUI.text = shipManager.instance.ammoTotal.ToString();
        missleUI.text = shipManager.instance.missleTotal.ToString();
    }
}
