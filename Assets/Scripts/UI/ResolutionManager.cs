using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ResolutionManager : MonoBehaviour
{

    [Header("Visual settings")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Alert pop up")]
    public GameObject popupUI;
    public TMP_Text countdownText;
    public TMP_Text messageText;

    public static ResolutionManager Instance { get; private set; }

    private const int COUNTDOWN_DURATION_MAX = 15;
    private int currentIndexResolution;
    private int _defaultResolutionIndexValue = 0;
    private Coroutine countdownCoroutine;
    private Resolution[] _resolutions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetInstance();
    }

    void Start()
    {
        SetupResolutions();
        popupUI.SetActive(false);
    }

    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Setup the resolution dropdown values and set the default as used by the player
    /// </summary>
    void SetupResolutions()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                _defaultResolutionIndexValue = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        currentIndexResolution = PlayerPrefs.GetInt("ResolutionIndex", _defaultResolutionIndexValue);
        resolutionDropdown.value = currentIndexResolution;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentIndexResolution);

        resolutionDropdown.onValueChanged.AddListener(delegate (int index)
        {
            if (index == currentIndexResolution)
            {
                return;
            }

            SetResolution(index, true);
        });
    }

    /// <summary>
    /// Apply the chosen resolution
    /// </summary>
    /// <param name="index"></param>
    void SetResolution(int index, bool isTimerNeeded = false)
    {
        Resolution res = _resolutions[index];

        if (Screen.currentResolution.Equals(res))
        {
            return;
        }

        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        if (isTimerNeeded)
        {
            popupUI.SetActive(true);
            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }

    private IEnumerator StartCountdown()
    {
        int timeLeft = COUNTDOWN_DURATION_MAX;

        while (timeLeft > 0)
        {
            countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        RevertResolution();
    }

    public void ConfirmResolution()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        popupUI.SetActive(false);

        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
    }

    public void RevertResolution()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        popupUI.SetActive(false);

        SetResolution(PlayerPrefs.GetInt("ResolutionIndex", 0));

        resolutionDropdown.value = currentIndexResolution;
        resolutionDropdown.RefreshShownValue();
    }

    public void RestoreDefaultResolution()
    {
        SetResolution(_defaultResolutionIndexValue, false);
        PlayerPrefs.SetInt("ResolutionIndex", _defaultResolutionIndexValue);
        resolutionDropdown.SetValueWithoutNotify(currentIndexResolution);
        resolutionDropdown.RefreshShownValue();
    } 
}
