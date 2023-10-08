using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretCameraController : MonoBehaviour
{
    [Header("--- Camera Stats ---")]
    [SerializeField] int sensitivity;
    [SerializeField] int lockRotMin;
    [SerializeField] int lockRotMax;
    [SerializeField] bool invertY;

    [SerializeField] Transform barrel;

    //Variable Definitions:
    float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        // keeps rotation on x axis
        xRotation = Mathf.Clamp(xRotation, lockRotMin, lockRotMax);

        // rotate camera x axis
        barrel.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // rotate camera y axis
        barrel.parent.Rotate(Vector3.up, mouseX);
    }
}
