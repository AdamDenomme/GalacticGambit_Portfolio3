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
    private bool isPausing = false;
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
        if (willPause && UIObject != null)
        {
            gamemanager.instance.statePause();
            isPausing = true;
        }
    }

    public void unPauseUI()
    {
        if (willunPause && UIObject != null)
        {
            if (isPausing)
            {
                gamemanager.instance.stateUnpause();
                isPausing = !isPausing;
            }
        }
    }

}
