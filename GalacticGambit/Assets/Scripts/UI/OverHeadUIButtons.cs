using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class OverHeadUIButtons : MonoBehaviour
{
    public GameObject UIObject;

    public void openUI()
    {
        if (UIObject != null)
        {
            bool isActive = UIObject.activeSelf;

            UIObject.SetActive(!isActive);
        }
    }
}
