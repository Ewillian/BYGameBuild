using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

/// <summary>
/// 
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("Visual settings")]
    public TMP_Dropdown languageDropdown;

    [Header("Volume settings")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI MasterVolumeTextOutput;
    public TextMeshProUGUI MusicVolumeTextOutput;
    public TextMeshProUGUI SFXVolumeTextOutput;
    public AudioMixer audioMixer;

    private ResolutionManager _resolutionManager;
    private MultiDeviceKeybindManager _multiDeviceKeybindManager;
    private const float DEFAULT_VOLUME_VALUE = 0.5f;

    /// <summary>
    /// Setups the different settings ui elements
    /// </summary>
    void Awake()
    {
        SetupVolumes();
        SetupLanguage();
    }

    void Start()
    {
        _resolutionManager = ResolutionManager.Instance;
        _multiDeviceKeybindManager = MultiDeviceKeybindManager.Instance;
    }

    /// <summary>
    /// Dynamicaly setup the volume of SFX and music using the setting audio sliders
    /// </summary>
    void SetupVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", DEFAULT_VOLUME_VALUE);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", DEFAULT_VOLUME_VALUE);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", DEFAULT_VOLUME_VALUE);

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        SetVolume(VolumeEnum.MasterVolume, masterVolume);
        SetVolume(VolumeEnum.MusicVolume, musicVolume);
        SetVolume(VolumeEnum.SFXVolume, sfxVolume);

        masterSlider.onValueChanged.AddListener(delegate(float volumeValue)
        {
            SetVolume(VolumeEnum.MasterVolume, volumeValue);
        });

        musicSlider.onValueChanged.AddListener(delegate(float volumeValue)
        {
            SetVolume(VolumeEnum.MusicVolume, volumeValue);
        });

        sfxSlider.onValueChanged.AddListener(delegate(float volumeValue)
        {
            SetVolume(VolumeEnum.SFXVolume, volumeValue);
        });
    }

     /// <summary>
    /// Apply volume
    /// </summary>
    /// <param name="volume">The volume enum to identify which mixer to apply values</param>
    /// <param name="volumeValue">The volume value</param>
    /// Convertit un volume linéaire en décibels (Unity utilise des dB dans les mixers)
    /// 1.0f → 0 dB (volume max)
    /// 0.001f → -60 dB environ (quasiment silence)
    public void SetVolume(VolumeEnum volume, float volumeValue)
    {
        if ((int)volume > 2 && (int)volume < 0)
        {
            Debug.LogWarning($"Volume type not expected or not implemented: {volume}");
            return;
        }

        string volumeTextValue = Mathf.Round(volumeValue * 100).ToString();
        audioMixer.SetFloat(volume.ToString(), Mathf.Log10(Mathf.Clamp(volumeValue, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat(volume.ToString(), volumeValue);

        switch (volume)
        {
            case VolumeEnum.MasterVolume:
                MasterVolumeTextOutput.SetText(volumeTextValue);
                break;
            case VolumeEnum.MusicVolume:
                MusicVolumeTextOutput.SetText(volumeTextValue);
                break;
            case VolumeEnum.SFXVolume:
                SFXVolumeTextOutput.SetText(volumeTextValue);
                break;
            default:
                Debug.LogWarning($"Volume type not expected or not implemented: {volume}");
                return;
        }
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

    public void RestoreDefaultSettings()
    {
        SetVolume(VolumeEnum.MasterVolume, DEFAULT_VOLUME_VALUE);
        masterSlider.SetValueWithoutNotify(DEFAULT_VOLUME_VALUE);

        SetVolume(VolumeEnum.MusicVolume, DEFAULT_VOLUME_VALUE);
        musicSlider.SetValueWithoutNotify(DEFAULT_VOLUME_VALUE);

        SetVolume(VolumeEnum.SFXVolume, DEFAULT_VOLUME_VALUE);
        sfxSlider.SetValueWithoutNotify(DEFAULT_VOLUME_VALUE);

        _resolutionManager.RestoreDefaultResolution();
        _multiDeviceKeybindManager.RestoreDefaultKeybinds();
    }
}
