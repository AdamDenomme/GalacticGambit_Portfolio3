using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] Slider volSlider;

    private void Awake()
    {
        volSlider.value = PlayerPrefs.GetFloat("savedVolumeSlider");
        AudioListener.volume = PlayerPrefs.GetFloat("savedVolumeSlider");
    }

    public void changeGameVol()
    {
        AudioListener.volume = volSlider.value;
        PlayerPrefs.SetFloat("savedVolumeSlider", volSlider.value);
        PlayerPrefs.SetFloat("savedVolume", AudioListener.volume);
        PlayerPrefs.Save();
    }

}
