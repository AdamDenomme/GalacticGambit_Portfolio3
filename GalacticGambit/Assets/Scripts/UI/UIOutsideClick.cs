using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIOutsideClick : MonoBehaviour
{
    public GameObject uiActiveMenu1;
    public GameObject uiActiveMenu2;
    public GameObject uiActiveMenu3;
    public GameObject uiActiveMenu4;
    public GameObject uiActiveMenu5;
    public GameObject uiActiveMenu6;

    public void closeUI()
    {
        if (uiActiveMenu1 != null ||
            uiActiveMenu2 != null ||
            uiActiveMenu3 != null ||
            uiActiveMenu4 != null ||
            uiActiveMenu5 != null ||
            uiActiveMenu6 != null)
        {
            uiActiveMenu1.SetActive(false);
            uiActiveMenu2.SetActive(false);
            uiActiveMenu3.SetActive(false);
            uiActiveMenu4.SetActive(false);
            uiActiveMenu5.SetActive(false);
            uiActiveMenu6.SetActive(false);
        }

    }
}
