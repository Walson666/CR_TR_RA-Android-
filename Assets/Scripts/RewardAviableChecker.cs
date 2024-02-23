using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CAS;
using CAS.AdObject;

public class RewardAviableChecker : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    Text adsNotLoaded;

    RewardedAdObject rewardedAd;

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
        button.interactable = true;
        adsNotLoaded.gameObject.SetActive(false);
    }



}
