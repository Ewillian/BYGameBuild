using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ResolutionManager : MonoBehaviour
{
    private const int COUNTDOWN_DURATION_MAX = 15;

    [Header("Visual settings")]
    public TMP_Dropdown resolutionDropdown;

    [Header("Alert pop up")]
    public GameObject popupUI;
    public TMP_Text countdownText;

    private Resolution[] resolutions;
    private int currentIndexResolution;
    private Coroutine countdownCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupResolutions();
        popupUI.SetActive(false);
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

        currentIndexResolution = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        resolutionDropdown.value = currentIndexResolution;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentIndexResolution);

        resolutionDropdown.onValueChanged.AddListener(delegate(int index) {
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
        Resolution res = resolutions[index];

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
}
