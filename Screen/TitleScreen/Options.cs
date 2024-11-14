using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public Dropdown graphicsDropdown;

    public Toggle fullScreenToggle;

    public Slider masterSlider;

    public Slider musicSlider;

    public Slider sfxSlider;

    float volumeNum;

    Resolution[] resolutions;


    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        audioMixer.GetFloat("volume", out volumeNum);
        masterSlider.value = volumeNum;

        audioMixer.GetFloat("music", out volumeNum);
        musicSlider.value = volumeNum;

        audioMixer.GetFloat("sfx", out volumeNum);
        sfxSlider.value = volumeNum;

        int qualityLevel = QualitySettings.GetQualityLevel();

        graphicsDropdown.value = qualityLevel;

        if (Screen.fullScreen == true)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn = false;
        }


    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        audioMixer.SetFloat("music", volume);
        audioMixer.SetFloat("sfx", volume);

        audioMixer.GetFloat("music", out volumeNum);
        musicSlider.value = volumeNum;

        audioMixer.GetFloat("sfx", out volumeNum);
        sfxSlider.value = volumeNum;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfx", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
