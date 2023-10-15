using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CreditsPlay : MonoBehaviour
{
    public Animator credits;
    public GameObject mainCamera;
    public GameObject creditsMenu;

    // Start is called before the first frame update
    void Start()
    {
        credits = mainCamera.GetComponent<Animator>();
    }

    public void creditsClick()
    {
        credits.SetBool("isPressed", true);
    }
    public void backToMenu()
    {
        credits.SetBool("isPressed", false);
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3);
    }
}
