using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    [SerializeField] Slider brightSlider;
    [SerializeField] Light gameLight;

    void Update()
    {
        try
        {
            gameLight.intensity = brightSlider.value;
            PlayerPrefs.SetFloat("savedBrightSlider", brightSlider.value);
            PlayerPrefs.Save();
        }
        catch(Exception e)
        {

        }
        
    }
}
