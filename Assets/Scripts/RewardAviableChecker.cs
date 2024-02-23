using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardAviableChecker : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    Text adsNotLoaded;

    private void OnEnable()
    {
        /*if (GP_Ads.IsRewardedAvailable())
        {
            button.interactable = true;
            adsNotLoaded.gameObject.SetActive(false);
        }
        else
        {
            button.interactable = false;
            adsNotLoaded.gameObject.SetActive(true);
        }*/
        button.interactable = false;
        adsNotLoaded.gameObject.SetActive(true);
    }



}
