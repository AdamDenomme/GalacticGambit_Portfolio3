using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    [SerializeField] Slider brightSlider;
    [SerializeField] Light gameLight;


    private void Awake()
    {
        brightSlider.value = PlayerPrefs.GetFloat("savedMusicSlider");
    }

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
