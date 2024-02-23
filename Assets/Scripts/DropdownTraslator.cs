using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using YG;

public class DropdownTraslator : MonoBehaviour
{
    //[SerializeField] InfoYG infoYG;
    [SerializeField] Dropdown dropdown;
    [SerializeField] Text labelText;
    Text itemText;
    [SerializeField] int fontNumber;
    [Space(5)]
    [Header("Translate")]
    [SerializeField] string[] ru = new string[6];
    [SerializeField] string[] en = new string[6];
    [SerializeField] string[] tr = new string[6];

    int labelBaseFontSize, itemBaseFontSize;

    void Awake()
    {
        SystemLanguage lang = Application.systemLanguage;
        //Language lang = GP_Language.Current();
        itemText = dropdown.itemText;
        labelBaseFontSize = labelText.fontSize;
        //itemBaseFontSize = itemText.fontSize;

        //dropdown.ClearOptions();
        //dropdown.AddOptions(QualitySettings.names.ToList());
        //dropdown.value = QualitySettings.GetQualityLevel();
        //dropdown.AddOptions()

        SwitchLanguage(lang);

        
    }

    private void Start()
    {
        dropdown.value = Singleton<UIManager>.Instance.InputMode;
    }

    //private void OnEnable() => YandexGame.SwitchLangEvent += SwitchLanguage;
    //private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;



    void SwitchLanguage(SystemLanguage lang)
    {
        switch (lang)
        {
            case SystemLanguage.Russian:
                Debug.Log("ru");
                labelText.text = ru[dropdown.value];
                //SwithFont(infoYG.fonts.ru);
                //FontSizeCorrect(infoYG.fontsSizeCorrect.ru);
                //dropdown.AddOptions(ru.ToList());
                for (int i = 0; i < ru.Length; i++)
                    dropdown.options[i].text = ru[i];
                break;
            case SystemLanguage.Turkish:
                labelText.text = tr[dropdown.value];
                //SwithFont(infoYG.fonts.tr);
                //FontSizeCorrect(infoYG.fontsSizeCorrect.tr);
                for (int i = 0; i < tr.Length; i++)
                    dropdown.options[i].text = tr[i];
                break;
            case SystemLanguage.English:
                labelText.text = en[dropdown.value];
                //SwithFont(infoYG.fonts.en);
                //FontSizeCorrect(infoYG.fontsSizeCorrect.en);
                for (int i = 0; i < en.Length; i++)
                    dropdown.options[i].text = en[i];
                break;
           
            default:
                labelText.text = en[dropdown.value];
                //SwithFont(infoYG.fonts.en);
                //FontSizeCorrect(infoYG.fontsSizeCorrect.en);
                for (int i = 0; i < en.Length; i++)
                    dropdown.options[i].text = en[i];
                break;
        }
    }

    void SwithFont(Font[] fontArray)
    {
        Font font = labelText.font;

        if (fontArray.Length >= fontNumber + 1 && fontArray[fontNumber])
        {
            font = fontArray[fontNumber];
        }
        else
        {
            /*if (infoYG.fonts.defaultFont.Length >= fontNumber + 1 && infoYG.fonts.defaultFont[fontNumber])
            {
                font = infoYG.fonts.defaultFont[fontNumber];
            }
            else if (infoYG.fonts.defaultFont.Length >= 1 && infoYG.fonts.defaultFont[0])
            {
                font = infoYG.fonts.defaultFont[0];
            }*/
        }

        labelText.font = font;
        itemText.font = font;
    }

    void FontSizeCorrect(int[] fontSizeArray)
    {
        labelText.fontSize = labelBaseFontSize;
        itemText.fontSize = itemBaseFontSize;

        if (fontSizeArray.Length != 0 && fontSizeArray.Length >= fontNumber - 1)
        {
            labelText.fontSize += fontSizeArray[fontNumber];
            itemText.fontSize += fontSizeArray[fontNumber];
        }
    }
}


