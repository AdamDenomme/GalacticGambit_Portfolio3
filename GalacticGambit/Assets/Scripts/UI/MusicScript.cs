using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicScript : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource musicSource;

    public void changeMusicVol()
    {
        musicSource.volume = musicSlider.value;
    }
}
