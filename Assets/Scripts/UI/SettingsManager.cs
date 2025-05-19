using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("Visual settings")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown languageDropdown;

    [Header("Volume settings")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;

    [Header("Keybinds")]
    public TextMeshProUGUI keyBindText;

    private Resolution[] resolutions;
    private KeyCode assignedKey = KeyCode.Space;
    private bool waitingForKey = false;

    /// <summary>
    /// Setups the different settings ui elements
    /// </summary>
    void Start()
    {
        SetupResolutions();
        SetupVolumes();
        SetupKeybind();
        SetupLanguage();
    }

    /// <summary>
    /// Setup the resolution dropdown values and set the default as used by the player
    /// </summary>
    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedIndex);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    /// <summary>
    /// Apply the chosen resolution
    /// </summary>
    /// <param name="index"></param>
    void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    /// <summary>
    /// Dynamicaly setup the volume of SFX and music using the setting audio sliders
    /// </summary>
    void SetupVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    /// <summary>
    /// Apply master volume
    /// </summary>
    /// <param name="volume"></param>
    /// Convertit un volume linéaire en décibels (Unity utilise des dB dans les mixers)
    /// 1.0f → 0 dB (volume max)
    /// 0.001f → -60 dB environ (quasiment silence)
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    /// <summary>
    /// Apply music volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Apply SFX volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// Setup the saved keybind and display it on the setting ui
    /// </summary>
    void SetupKeybind()
    {
        string savedKey = PlayerPrefs.GetString("KeyBind", "Space");
        assignedKey = (KeyCode)Enum.Parse(typeof(KeyCode), savedKey);
        keyBindText.text = assignedKey.ToString();
    }

    /// <summary>
    /// Update used only by the keybinding setting
    /// </summary>
    void Update()
    {
        if (waitingForKey)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    assignedKey = key;
                    waitingForKey = false;
                    keyBindText.text = assignedKey.ToString();
                    PlayerPrefs.SetString("KeyBind", assignedKey.ToString());
                    break;
                }
            }
        }
    }

    public void StartKeyBinding()
    {
        waitingForKey = true;
        keyBindText.text = "Press a key...";
    }

    public KeyCode GetAssignedKey()
    {
        return assignedKey;
    }

    // -------- Language --------
    void SetupLanguage()
    {
        string savedLang = PlayerPrefs.GetString("Language", "English");

        int langIndex = 0;
        for (int i = 0; i < languageDropdown.options.Count; i++)
        {
            if (languageDropdown.options[i].text == savedLang)
            {
                langIndex = i;
                break;
            }
        }

        languageDropdown.value = langIndex;
        languageDropdown.RefreshShownValue();

        languageDropdown.onValueChanged.AddListener(SetLanguage);
    }

    void SetLanguage(int index)
    {
        string selectedLang = languageDropdown.options[index].text;
        PlayerPrefs.SetString("Language", selectedLang);

        // Si tu as un gestionnaire de langue, appelle-le ici
        // LocalizationManager.Instance.SetLanguage(selectedLang);
    }
}
