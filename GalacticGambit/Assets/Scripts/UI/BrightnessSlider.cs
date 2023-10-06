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
        gameLight.intensity = brightSlider.value;
    }
}
