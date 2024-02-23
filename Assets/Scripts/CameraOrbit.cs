// dnSpy decompiler from Assembly-CSharp.dll class: CameraOrbit
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
	internal void Init()
	{
		this.targetDistance = this.distance;
		this.y = 0f;
		this.xVelocity = 0f;
		this.yVelocity = 0f;
		this.zoomVelocity = 0f;
	}

	private void OnEnable()
	{
		this.Init();
	}

	private void Update()
	{
		//this.MouseInput();
		this.HandleInput();
		this.UpdateCamera();
	}

	private void UpdateCamera()
	{
		base.transform.Rotate(new Vector3(0f, this.xVelocity, 0f), Space.World);
		if (this.y + this.yVelocity < this.yMinLimit + this.yLimitOffset)
		{
			this.yVelocity = this.yMinLimit + this.yLimitOffset - this.y;
		}
		else if (this.y + this.yVelocity > this.yMaxLimit + this.yLimitOffset)
		{
			this.yVelocity = this.yMaxLimit + this.yLimitOffset - this.y;
		}
		this.y += this.yVelocity;
		base.transform.Rotate(new Vector3(this.yVelocity, 0f, 0f), Space.Self);
		if (this.targetDistance + this.zoomVelocity < this.minDistance)
		{
			this.zoomVelocity = this.minDistance - this.targetDistance;
		}
		else if (this.targetDistance + this.zoomVelocity > this.maxDistance)
		{
			this.zoomVelocity = this.maxDistance - this.targetDistance;
		}
		this.targetDistance += this.zoomVelocity;
		this.distance = Mathf.Lerp(this.distance, this.targetDistance, this.smoothingZoom);
		this.position = base.transform.rotation * new Vector3(0f, 0f, -this.distance) + this.target.position;
		base.transform.position = this.position;
		this.xVelocity *= this.dampeningX;
		this.yVelocity *= this.dampeningY;
		this.zoomVelocity = 0f;
	}

	private void HandleInput()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			foreach (Touch touch in Input.touches)
			{
				TouchPhase phase = touch.phase;
				if (phase != TouchPhase.Began)
				{
					if (phase != TouchPhase.Moved)
					{
						if (phase != TouchPhase.Ended)
						{
						}
					}
					else
					{
						this.xVelocity += Input.touches[0].deltaPosition.x * 0.05f;
						this.yVelocity -= Input.touches[0].deltaPosition.y * 0.05f;
					}
				}
			}
		}
	}

	private void MouseInput()
	{
		int minswipeDetection = 400;
		Vector3 startPoint = Vector3.zero;
		if (Input.GetMouseButtonDown(0))
		{
			startPoint = Input.mousePosition;
		}
		if( Input.GetMouseButton(0))
		{
			Vector3 endpoint = Input.mousePosition;

			Vector3 delta = endpoint - startPoint;
			Debug.Log(delta.sqrMagnitude);
		}
        
    }

    

    public float yLimitOffset;

	public float yMaxLimit = 60f;

	public float yMinLimit = -60f;

	public float distance;

	public float maxDistance = 15f;

	public float minDistance = 5f;

	public Transform target;

	private float dampeningX = 0.9f;

	private float dampeningY = 0.9f;

	public const float xSpeedTouch = 0.05f;

	public const float ySpeedTouch = 0.05f;

	private Vector3 position;

	private float xVelocity;

	private float yVelocity;

	private float targetDistance = 10f;

	private float ySpeed = 1f;

	private float xSpeed = 1f;

	private float zoomVelocity = 2f;

	private float smoothingZoom = 0.5f;

	private float y;

	private const string MOUSE_X = "Mouse X";

	private const string MOUSE_Y = "Mouse Y";
}
