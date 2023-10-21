using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicScript : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource musicSource;


    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("savedMusicSlider");
    }


    public void changeMusicVol()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("savedMusicSlider", musicSlider.value);
        PlayerPrefs.Save();
    }

}
