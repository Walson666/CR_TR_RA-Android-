using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButtonController : MonoBehaviour
{
    public GameObject button;
    private void OnEnable()
    {
        //button.SetActive(GP_Player.IsLoggedIn());
        button.SetActive(true);
    }
}
