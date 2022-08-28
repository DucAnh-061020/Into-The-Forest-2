using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    public static float musicVolume { get; private set; }
    public static float soundEffectVolume { get; private set; }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetFloat("BGM", 1);
        }
        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetFloat("SFX", 1);
        }
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                if (!PlayerPrefs.HasKey("ResolutionIndex"))
                {
                    PlayerPrefs.SetInt("ResolutionIndex", i);
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Awake()
    {
        
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("BGM", musicVolume);
        PlayerPrefs.SetFloat("SFX", soundEffectVolume);
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                PlayerPrefs.SetInt("ResolutionIndex", i);
            }
        }
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        if (resIndex == Screen.resolutions.Length-1)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
        else
        {
            Screen.SetResolution(resolution.width, resolution.height, false);
        }
        AudioManager.instace.PlayClipByName("Select");
    }

    public void SetBGM(float volume)
    {
        musicVolume = volume;
        AudioManager.instace.UpdateMixerVolume();
    }

    public void SetSFX(float volume)
    {
        soundEffectVolume = volume;
        AudioManager.instace.UpdateMixerVolume();
    }

    public void Back()
    {
        QuestionDialogUI.Instance.ShowQuestion("Save new changes?", () =>
        {
            PlayerPrefs.SetFloat("BGM", musicVolume);
            PlayerPrefs.SetFloat("SFX", soundEffectVolume);
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width
                    && resolutions[i].height == Screen.currentResolution.height)
                {
                    PlayerPrefs.SetInt("ResolutionIndex", i);
                }
            }
        }, () =>
        {
            SetBGM(PlayerPrefs.GetFloat("BGM"));
            SetSFX(PlayerPrefs.GetFloat("SFX"));
            SetResolution(PlayerPrefs.GetInt("ResolutionIndex"));
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
        });
    }
}
