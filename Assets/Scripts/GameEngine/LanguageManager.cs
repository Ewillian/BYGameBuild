using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager
{
    private static LanguageManager _instance;
    public string LanguageFileName = "Assets/Language/lang_";

    private Dictionary<LanguageEnum, Lang> _languagesDictionary;
    private LanguageEnum _currentLanguage = LanguageEnum.English;

    private LanguageManager()
    {
        _languagesDictionary = new Dictionary<LanguageEnum, Lang>();
    }

    public static LanguageManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LanguageManager();
        }

        return _instance;
    }

    public void CreateFromJSON(LanguageEnum newLanguage)
    {
        _currentLanguage = newLanguage;
        if(!_languagesDictionary.ContainsKey(_currentLanguage))
        {
            string jsonString = File.ReadAllText(LanguageFileName + _currentLanguage + ".json");

            _languagesDictionary.Add(_currentLanguage , JsonUtility.FromJson<Lang>(jsonString));
        }
    }

    public string GetValue(string name)
    {
        for(int i=0; i<_languagesDictionary[_currentLanguage].quotes.Count ; i++)
        {
            Quote currentQuote = _languagesDictionary[_currentLanguage].quotes[i];
            if(name == currentQuote.name)
            {
                return currentQuote.value;
            }
        }
        return "error";
    }
}

[Serializable]
public class Lang
{
    public string lang;
    public List<Quote> quotes;
}

[Serializable]
public class Quote
{
    public string name;
    public string value;
}
