using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUp : MonoBehaviour
{

    public int cutsceneTime;
    public GameObject ui;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(popUpUI(cutsceneTime));
    }

    // Update is called once per frame
    IEnumerator popUpUI(int cutscene)
    {
        cutsceneTime = cutscene;
        yield return new WaitForSeconds(cutsceneTime);
        ui.SetActive(true);
    }
}
