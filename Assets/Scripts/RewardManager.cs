using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CAS;
using CAS.AdObject;
//using YG;

public class RewardManager : MonoBehaviour
{
    public PopupsMan popupsMan;
    private bool musicLastState;

    public InterstitialAdObject InterstitialAdObject;
    public RewardedAdObject RewardedAdObject;

    string _key;

    private void OnEnable()
    {
        RewardedAdObject.OnReward.AddListener(OnReward);
    }
    private void OnDisable()
    {
        RewardedAdObject.OnReward.RemoveListener(OnReward);
    }

    public void ShowRewarded(string key)  
    {
        RewardedAdObject.Present();
        _key = key;
    }

    public void ShowInterstitialAd()
    {
        InterstitialAdObject.Present();
    }

    public void OnReward()
    {
        Debug.Log("The user earned the reward.");
        OnkeyRewarded(_key);
    }
    private void OnkeyRewarded(string value)
    {
        if (value == "COINS")
        {
            PlayerDataPersistant.Instance.Coins += 500;
            popupsMan.OnClickBack();
            //Debug.Log("ON REWARDED + 500 Coins");
            //Debug.Log($"Balance After Ad:{PlayerDataPersistant.Instance.Coins}");
        }
            

        if (value == "REVIVE")
            Singleton<UIManager>.Instance.inGamePage.RessurectPlayer();
    }

    private void OnStartAd()
    {
        StopMusic();
    }

    public void OnEndAds()
    {
        ReturnMusicState();
    }

    public void StopMusic()
    {
        if (Singleton<SoundManager>.Instance.musicOn)
        {
            Singleton<SoundManager>.Instance.ToggleMusicOnOff();
        }
    }

    public void ReturnMusicState()
    {
        if (!Singleton<SoundManager>.Instance.musicOn)
        {
            Singleton<SoundManager>.Instance.ToggleMusicOnOff();
        }
    }

    private void OnFullscreenStart()
    {

        StopMusic();
    }
    // Закончился показ
    private void OnFullscreenClose(bool success) 
    {
        ReturnMusicState();
    } 


}
