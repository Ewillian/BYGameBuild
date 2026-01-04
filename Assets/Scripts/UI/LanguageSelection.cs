using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    private TMP_Dropdown _languageSystem;

    LanguageManager _languagesManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _languageSystem = GetComponent<TMP_Dropdown>();
        PlayerPrefs.SetInt("Language", (int) LanguageEnum.English);

        _languagesManager = LanguageManager.GetInstance();
        _languagesManager.CreateFromJSON((LanguageEnum) PlayerPrefs.GetInt("Language"));
    }

    public void OnChangeLanguage()
    {
        PlayerPrefs.SetInt("Language", LanguageEnum.IsDefined(typeof(LanguageEnum), _languageSystem.value) ? _languageSystem.value : (int) LanguageEnum.English);
        LanguageEnum newLanguage = (LanguageEnum) PlayerPrefs.GetInt("Language");
        Debug.Log("Changing Language to " + newLanguage);
        _languagesManager.CreateFromJSON(newLanguage);

        Debug.Log("Res=" + _languagesManager.GetValue("mainmenu_options_video_resolution"));
    }
}
