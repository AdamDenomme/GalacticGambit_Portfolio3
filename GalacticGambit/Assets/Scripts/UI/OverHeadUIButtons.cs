using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class OverHeadUIButtons : MonoBehaviour
{
    public GameObject UIObject;
    public GameObject menuParent;
    public GameObject activeMenu;
    public bool willPause;
    public bool willunPause;
    public int isPausing;
    private bool isActive;




    public void openUI()
    {
        if (menuParent != null)
        {
            menuParent.SetActive(false);

            if (UIObject != null)
            {
                isActive = UIObject.activeSelf;

                UIObject.SetActive(!isActive);
            }
        }
        else
        {
            if (UIObject != null)
            {
                isActive = UIObject.activeSelf;

                UIObject.SetActive(!isActive);
            }

        }

    }

    public void pauseUI()
    {
        if (willPause && (UIObject != null || menuParent != null))
        {
            if (isPausing == 1)
            {
                isPausing = 0;
                gamemanager.instance.stateUnpause();
            }
            if (isPausing == 0)
            {
                isPausing = 1;
                gamemanager.instance.statePause();
            }
        }

        else if (willunPause && menuParent != null)
        {
            gamemanager.instance.stateUnpause();
            isPausing = 0;
        }
    }
}
