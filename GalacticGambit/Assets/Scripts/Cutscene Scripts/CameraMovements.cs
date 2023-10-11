using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{

    public Camera cutsceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        StartCoroutine(StartCutscene());
        Cursor.visible = true;

    }


    IEnumerator StartCutscene()
    {
        //cutsceneCamera.setDestination(transform.position);
        yield return new WaitForSeconds(5);
        
    }

}
