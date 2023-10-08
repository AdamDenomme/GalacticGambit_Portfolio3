using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    [SerializeField] Camera turretCamera;
    [SerializeField] Camera overheadCamera;

    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView()
    {
        turretCamera.enabled = false;
        overheadCamera.enabled = true;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView()
    {
        if (Input.GetButtonDown("SwitchCam") == true)
        {
            turretCamera.enabled = true;
            overheadCamera.enabled = false;
        }
    }
}
