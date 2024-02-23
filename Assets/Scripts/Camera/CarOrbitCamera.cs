using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOrbitCamera : MonoBehaviour
{
    public Transform Car;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1f)]
    public float smoothFactor;

    public bool LookAtCar;

    public bool RotateAroundCar;

    public float RotationSpeed = 5f;

    private void Start()
    {
        _cameraOffset = transform.position - Car.position;
    }

    private void LateUpdate()
    {
        if(Input.GetMouseButton(0))
            RotateAroundCar = true;
        if (Input.GetMouseButtonUp(0))
            RotateAroundCar = false;


        if (RotateAroundCar)
        {
            Quaternion turnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            _cameraOffset = turnAngle * _cameraOffset;
        }

        Vector3 newPos = Car.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        if (LookAtCar || RotateAroundCar)
        {
            transform.LookAt(Car);
        }
    }
}
