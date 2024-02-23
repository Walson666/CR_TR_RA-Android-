using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslator : MonoBehaviour
{
    public Text TranslationText;
    public string key;


    
    

    private void Start()
    {
        TranslationText.text = Singleton<Translation>.Instance.Get(key);
       
    }

    /*private void OnEnable()
    {
        GP_Language.OnChangeLanguage += GP_Language_OnChangeLanguage;
    }
    private void OnDisable()
    {
        GP_Language.OnChangeLanguage -= GP_Language_OnChangeLanguage;
    }*/
    private void GP_Language_OnChangeLanguage(SystemLanguage arg0)
    {

        TranslationText.text = Singleton<Translation>.Instance.Get(key);
    }

    


}
