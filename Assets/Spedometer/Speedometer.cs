using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
 
    private PlayerMovement pD
    {
        get
        {
            return Singleton<GamePlay>.Instance.player;
        }
    }

   

    public float maxSpeed = 0.0f; // The maximum speed of the target ** IN KM/H **

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public Text speedLabel; // The label that displays the speed;
    public RectTransform arrow; // The arrow in the speedometer

    private float speed = 0.0f;
    private void Update()
    {
        // 3.6f to convert in kilometers
        // ** The speed must be clamped by the car controller **
        float prevVal = pD.Speed;
        speed = Mathf.CeilToInt(prevVal * 3f);
        

        if (speedLabel != null)
            speedLabel.text = ((int)speed) + " km/h";
        if (arrow != null)
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
}
