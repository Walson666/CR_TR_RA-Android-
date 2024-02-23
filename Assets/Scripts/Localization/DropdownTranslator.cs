using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownTranslator : MonoBehaviour
{
    public Dropdown dropdown;
    public string[] keys;


    private void Start()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(Translation.Instance.Get(keys[i]));
            dropdown.options[i] = data;
        }
        
    }
}
