using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : Singleton<Translation>
{
    public TranslationContainer[] translationContainers;
    public Dictionary<string, string[]> translationContainersDict;

    private void Awake()
    {
        translationContainersDict = new Dictionary<string, string[]>();
        for (int i = 0; i < translationContainers.Length; i++)
        {
            translationContainersDict.Add(translationContainers[i].key, translationContainers[i].values);
        }
    }

    public string Get(string key)
    {
        //Language lang = GP_Language.Current();
        SystemLanguage language = Application.systemLanguage;
        int lang = 0;
        if (language == SystemLanguage.English)
            lang = 0;
        else if (language == SystemLanguage.Russian)
            lang = 1;
        else if (language == SystemLanguage.Turkish)
            lang = 2;
        else
            lang = 0;
        if (translationContainersDict.ContainsKey(key))
        {
            string[] values = translationContainersDict[key];
            if((int)lang < values.Length)
            {
                return values[(int)lang];
            }
            else
            {
                return values[0];
            }
        }
        return "UNKNOWN";
    }

}

[System.Serializable]
public class TranslationContainer
{
    public string key;
    public string[] values;
}
