using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CreditsPlay : MonoBehaviour
{
    public Animator credits;
    public GameObject mainCamera;
    public GameObject creditsMenu;
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        credits = mainCamera.GetComponent<Animator>();
    }

    public void creditsClick()
    {
        StopAllCoroutines();
        StartCoroutine(WaitTime());
        credits.SetBool("isPressed", true);
    }
    public void backToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(BackToMain());
        credits.SetBool("isPressed", false);
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(5.5f);
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }
    IEnumerator BackToMain()
    {
        yield return new WaitForSeconds(2);
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
}
