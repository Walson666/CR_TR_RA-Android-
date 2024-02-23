using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInput : MonoBehaviour
{
    private PlayerMovement pD
    {
        get
        {
            return Singleton<GamePlay>.Instance.player;
        }
    }
    public InGameUIPage InGameUIPage;
    public GameObject[] Tuttorial;


    private void OnEnable()
    {
        foreach (var item in Tuttorial)
        {
            item.SetActive(true);
        }
    }
    private void OnDisable()
    {
        foreach (var item in Tuttorial)
        {
            item.SetActive(false);
        }
        //Debug.LogError("Disabled");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(Singleton<GamePlay>.Instance.GameState != GamePlay.GameplayStates.Paused)
                InGameUIPage.PauseClicked();
        }
        if (Input.GetKey(KeyCode.W))
        {
            pD.OnUpInput(true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            pD.OnUpInput(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            pD.OnLeftInput(true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            pD.OnLeftInput(false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            pD.OnDownInput(true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            pD.OnDownInput(false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            pD.OnRightInput(true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            pD.OnRightInput(false);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Singleton<GamePlay>.Instance.NitroButtonPressed = true;
            if (Singleton<GamePlay>.Instance.NitroRatio > 0.1f)
            {
                Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(true);
                pD.OnUpInput(true);
            }
            else
            {
                Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(false);
                //pD.OnUpInput(false);
            }
                
            
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Singleton<GamePlay>.Instance.NitroButtonPressed = false;
            Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(false);
            pD.OnUpInput(false);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            pD.OnUpInput(false);
            pD.OnDownInput(false);
            pD.OnLeftInput(false);
            pD.OnRightInput(false);
            Singleton<GamePlay>.Instance.NitroButtonPressed = false;
            Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(false);
        }
    }
}
