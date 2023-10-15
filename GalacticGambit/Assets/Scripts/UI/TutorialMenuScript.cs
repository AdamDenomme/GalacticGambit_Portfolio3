using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenuScript : MonoBehaviour
{
    // Ui being called and UI it derives from
    public GameObject textObject;
    
    public GameObject otherObject1;
    public GameObject otherObject2;
    public GameObject otherObject3;
    public GameObject otherObject4;
    public GameObject otherObject5;
    public GameObject otherObject6;
    public GameObject otherObject7;
    public GameObject otherObject8;

    private bool isActive;

    public void textChange()
    {
        if (otherObject1 != null ||
            otherObject2 != null ||
            otherObject3 != null ||
            otherObject4 != null ||
            otherObject5 != null ||
            otherObject6 != null ||
            otherObject7 != null ||
            otherObject8 != null)
        {
            otherObject1.SetActive(false);
            otherObject2.SetActive(false);
            otherObject3.SetActive(false);
            otherObject4.SetActive(false);
            otherObject5.SetActive(false);
            otherObject6.SetActive(false);
            otherObject7.SetActive(false);
            otherObject8.SetActive(false);

            if (textObject != null)
            {
                isActive = textObject.activeSelf;

                textObject.SetActive(!isActive);
            }
        }
        else
        {
            if (textObject != null)
            {
                isActive = textObject.activeSelf;

                textObject.SetActive(!isActive);
            }

        }

    }
}
