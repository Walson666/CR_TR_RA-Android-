using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using YG;

public class RewardManager : MonoBehaviour
{
    public PopupsMan popupsMan;
    private bool musicLastState;


    private void OnEnable()
    {
        //GP_Payments.OnFetchPlayerPurchases += OnFetchPlayerPurchases;
        //GP_Payments.OnFetchProductsError += OnFetchProductsError;
    }
    //Отписка от событий
    private void OnDisable()
    {
        //GP_Payments.OnFetchPlayerPurchases -= OnFetchPlayerPurchases;
        //GP_Payments.OnFetchProductsError -= OnFetchProductsError;
    }

    //public void Fetch() => GP_Payments.Fetch();

    // Ошибки при получении
    private void OnFetchProductsError() => Debug.Log("FETCH PRODUCTS: ERROR");

    /*private void OnFetchPlayerPurchases(List<FetchPlayerPurchases> purcahses)
    {
        for (int i = 0; i < purcahses.Count; i++)
        {
            //Debug.LogError("PLAYER PURCHASES: PRODUCT TAG: " + purcahses[i].tag);
            //Debug.LogError("PLAYER PURCHASES: PRODUCT ID: " + purcahses[i].productId);
            //Debug.LogError("PLAYER PURCHASES: PAYLOAD: " + purcahses[i].payload);
            //Debug.LogError("PLAYER PURCHASES: CREATED AT: " + purcahses[i].createdAt);
            //Debug.LogError("PLAYER PURCHASES: EXPIRED AT: " + purcahses[i].expiredAt);
            //Debug.LogError("PLAYER PURCHASES: GIFT: " + purcahses[i].gift);
            //Debug.LogError("PLAYER PURCHASES: SUBSCRIBED: " + purcahses[i].subscribed);
            PlayerDataPersistant.Instance.Coins += 5000;
            Consume();
        }
        PlayerDataPersistant.Instance.SaveGameData(true);
    }*/

    public void ShowRewarded(string key)  
    {
        //GP_Ads.IsRewardedAvailable();
        //GP_Ads.ShowRewarded(key, OnRewardedReward, OnRewardedStart, OnRewardedClose);
    }


    private void Start()
    {
        //Fetch();
    }


    // Начался показ
    private void OnRewardedStart()
    {
        StopMusic();
        Debug.Log($"Balance before Ad:{PlayerDataPersistant.Instance.Coins}");
    }
    // Получена награда
    private void OnRewardedReward(string value)
    {
        if (value == "COINS")
        {
            PlayerDataPersistant.Instance.Coins += 500;
            popupsMan.OnClickBack();
            Debug.Log("ON REWARDED + 500 Coins");
            Debug.Log($"Balance After Ad:{PlayerDataPersistant.Instance.Coins}");
        }
            

        if (value == "REVIVE")
            Singleton<UIManager>.Instance.inGamePage.RessurectPlayer();
    }

    // Закончился показ
    private void OnRewardedClose(bool success)
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


    //public void ShowFullscreen() => GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);

    // Начался показ
    private void OnFullscreenStart()
    {

        StopMusic();
    }
    // Закончился показ
    private void OnFullscreenClose(bool success) 
    {
        ReturnMusicState();
    } 


    

    //public void Purchase() => GP_Payments.Purchase("GOLD5000", OnPurchaseSuccess, OnPurchaseError);

    // Успешная покупка
    private void OnPurchaseSuccess(string productIdOrTag)
    {
        Debug.LogError("PURCHASE: SUCCESS: " + productIdOrTag);
        PlayerDataPersistant.Instance.Coins += 5000;
        PlayerDataPersistant.Instance.SaveGameData(true);
        //Consume();
        popupsMan.OnClickBack();
    }
    // Ошибка если покупка не удалась
    private void OnPurchaseError() => Debug.Log("PURCHASE: ERROR");


    //public void Consume() => GP_Payments.Consume("GOLD5000", OnConsumeSuccess, OnConsumeError);

    // Успешно использована
    private void OnConsumeSuccess(string productIdOrTag)
    {
        Debug.LogError("CONSUME: SUCCESS: " + productIdOrTag);

    }
    // Ошибка при использовании
    private void OnConsumeError() => Debug.LogError("CONSUME: ERROR");
}
