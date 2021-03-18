using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{

    public AudioMixer mixer;
    public Slider sliderSFX;
    public Slider sliderBGM;

    private void Start()
    {
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sliderBGM.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
    }

    public void SetLevelSFX(float sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetLevelBGM(float sliderValue)
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("BGMVolume", sliderValue);
    }
}