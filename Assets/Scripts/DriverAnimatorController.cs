using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverAnimatorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float AnimationSpeed = 1;
    [SerializeField]
    private float steeringWheelAngle = 35;

    [Header("Links")]
    public Animator driverAnimator;
    public GameObject steeringWheel;

    float turnAngle;
    float endValue;
    private PlayerMovement pD
    {
        get
        {
            return Singleton<GamePlay>.Instance.player;
        }
    }


    


    private void FixedUpdate()
    {

        /*if(Input.GetKey(KeyCode.A)) 
        {
            endValue = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            endValue = 1;
        }

        else
        {
            endValue = 0;
        }*/

        if (pD.InputLeft)
        {
            endValue = -1;
        }
        else if (pD.InputRight)
        {
            endValue = 1;
        }
        else
        {
            endValue = 0;
        }
        turnAngle = Mathf.Lerp(turnAngle, endValue, AnimationSpeed * Time.deltaTime);
        driverAnimator.SetFloat("turnAngle", turnAngle);

        if (steeringWheel != null)
            steeringWheel.transform.localRotation = Quaternion.Euler(0, turnAngle * steeringWheelAngle, 0);
    }

   
}
