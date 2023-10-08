using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class OverHeadUIButtons : MonoBehaviour
{
    public GameObject UIObject;
    public GameObject menuParent;
   

    public void openUI()
    {
        if (menuParent != null)
        {
            menuParent.SetActive(false);

            if (UIObject != null)
            {
                bool isActive = UIObject.activeSelf;

                UIObject.SetActive(!isActive);
            }
        }
        else
        {
            if (UIObject != null)
            {
                bool isActive = UIObject.activeSelf;

                UIObject.SetActive(!isActive);
            }

        }

    }

}
