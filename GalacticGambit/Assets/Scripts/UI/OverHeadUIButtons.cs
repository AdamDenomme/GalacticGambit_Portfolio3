using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class OverHeadUIButtons : MonoBehaviour
{
    // Ui being called and UI it derives from
    public GameObject UIObject;
    public GameObject menuParent;

   
    public bool willPause;
    public bool willunPause;
    public int isPausing;
    private bool isActive;
    public int timeNeeded;


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

    public void closeAfterAnimation()
    {
       // StartCoroutine("waitTime");
       // //UIObject.SetActive(false);
       // isActive = true;
       //
       // if (isActive)
       // {
       //     UIObject.SetActive(true);
       // }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(4000);
    }

    public void openAfterAnimation()
    {
        menuParent.SetActive(false);
    }
}
