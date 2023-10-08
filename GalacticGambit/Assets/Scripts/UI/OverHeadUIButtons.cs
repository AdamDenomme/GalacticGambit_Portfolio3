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
    private bool isPaused;
   

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

    public void pauseUI()
    {
        if (willPause && menuParent == null)
        {
            gamemanager.instance.statePause();
            activeMenu = menuParent;
            activeMenu.SetActive(isPaused);
        }
        else if (willPause && menuParent != null)
        {
            activeMenu = menuParent;
            activeMenu.SetActive(false);
            gamemanager.instance.stateUnpause();
        }
    }

}
