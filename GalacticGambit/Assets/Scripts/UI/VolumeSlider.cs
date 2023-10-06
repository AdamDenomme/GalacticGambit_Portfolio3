using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] Slider volSlider;

    public void changeGameVol()
    {
        AudioListener.volume = volSlider.value;
    }

}
