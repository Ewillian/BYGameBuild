using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
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
    public TextMeshProUGUI MasterVolumeTextOutput;
    public TextMeshProUGUI MusicVolumeTextOutput;
    public TextMeshProUGUI SFXVolumeTextOutput;
    public AudioMixer audioMixer;

    [Header("Keybinds")]
    public TextMeshProUGUI keyBindText;

    private Resolution[] resolutions;

    /// <summary>
    /// Setups the different settings ui elements
    /// </summary>
    void Start()
    {
        SetupResolutions();
        SetupVolumes();
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
        string volumeTextValue = Mathf.Round(volume * 100).ToString();
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        MasterVolumeTextOutput.SetText(volumeTextValue);
    }

    /// <summary>
    /// Apply music volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        string volumeTextValue = Mathf.Round(volume * 100).ToString();
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        MusicVolumeTextOutput.SetText(volumeTextValue);
    }

    /// <summary>
    /// Apply SFX volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        string volumeTextValue = Mathf.Round(volume * 100).ToString();
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        SFXVolumeTextOutput.SetText(volumeTextValue);
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
