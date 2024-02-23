// dnSpy decompiler from Assembly-CSharp.dll class: FollowPlayer
using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
	private PlayerMovement pD
	{
		get
		{
			return Singleton<GamePlay>.Instance.player;
		}
	}

	public int cameraIndex
	{
		get
		{
			return this.c_Index;
		}
		set
		{
			this.c_Index = ((value <= 2) ? value : 0);
		}
	}

	private void Start()
	{
		this.lastPivotHeight = 0f;
		this.lastSourceOffset = (this.goalSourceOffset = this.camModes[0].source);
		this.lastTargetOffset = (this.goalTargetOffset = this.camModes[0].target);
		this.currentShakeData = new FollowPlayer.ShakeData(0f, 0f, 0f);
		ChangePlayerCamera();
        //this.ChangeCamera();
	}

	private void OnEnable()
	{
		this.Slowmo = false;
		ChangePlayerCamera();

    }

	private void OnDisable()
	{
		this.Slowmo = false;
	}

	public void ChangeCameraMode()
	{
		this.cameraIndex++;
		ChangePlayerCamera();
        //this.ChangeCamera();
	}

	private void StartEasing(Vector3 goalSource, Vector3 goalTarget, float duration)
	{
		this.interpolating = true;
		this.startEaseTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		this.easeTimeDuration = duration;
		this.goalSourceOffset = goalSource;
		this.goalTargetOffset = goalTarget;
	}

	private Vector3 EaseTo(Vector3 source, Vector3 goalSource, Vector3 defaultGoal)
	{
		if (this.interpolating)
		{
			float totalTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
			source.x = EasingG.EaseInOutQuad(totalTime - this.startEaseTime, source.x, goalSource.x - source.x, this.easeTimeDuration);
			source.y = EasingG.EaseInOutQuad(totalTime - this.startEaseTime, source.y, goalSource.y - source.y, this.easeTimeDuration);
			source.z = EasingG.EaseInOutQuad(totalTime - this.startEaseTime, source.z, goalSource.z - source.z, this.easeTimeDuration);
			if (totalTime - this.startEaseTime >= this.easeTimeDuration)
			{
				this.interpolating = false;
			}
		}
		else
		{
			source = goalSource;
		}
		return source;
	}

	private void NoFirstFrame()
	{
		this.firstFrame = false;
	}

	public void OnFollowCharaEnter(float time)
	{
		this.prevPlayerPivot = this.pD.transform.position;
		this.firstFrame = true;
		this.deadTime = -1f;
		this.actionTaken = false;
	}

	public void OnFollowCharaExec(float time)
	{
		if (this.pD == null)
		{
			return;
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float totalTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		Vector3 position = this.pD.transform.position;
		position.y = 0f;
		position.x = ((this.cameraIndex != 0) ? position.x : 0f);
		float y = position.y;
		if (this.firstFrame)
		{
			this.lastPivotHeight = y;
			this.prevPlayerPivot = position;
			this.heightVelocity = 0f;
			this.firstFrame = false;
		}
		else
		{
			float target = 0.1f;
			this.smoothTime = Mathf.MoveTowards(this.smoothTime, target, 2.5f * fixedDeltaTime);
			this.lastPivotHeight = Mathf.SmoothDamp(this.lastPivotHeight, y, ref this.heightVelocity, this.smoothTime, 50f, fixedDeltaTime);
			this.prevPlayerPivot = position;
		}
		this.offset.z = 0f;
		Vector3 a = new Vector3(this.prevPlayerPivot.x * 0.8f, this.lastPivotHeight, this.prevPlayerPivot.z);
		this.lastSourceOffset = this.EaseTo(this.lastSourceOffset, this.goalSourceOffset, this.sourceOffset);
		this.lastTargetOffset = this.EaseTo(this.lastTargetOffset, this.goalTargetOffset, this.targetOffset);
		//camera Position
        base.transform.position = pD.driver.transform.position + this.lastSourceOffset + this.offset * 0.1f + this.noise * this.noiseStrength;

        //base.transform.position = a + this.lastSourceOffset + this.offset * 0.1f + this.noise * this.noiseStrength;
		//base.transform.LookAt(a + this.lastTargetOffset + this.offset * 0.1f, Vector3.up);
		if (!Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			if (this.shakeCameraActive)
			{
				this.ShakeCamera(fixedDeltaTime);
			}
			this.UpdateTremor(fixedDeltaTime);
		}
		if (totalTime - this.deadTime > 3.6f && !this.actionTaken && this.deadTime > 0f)
		{
			this.actionTaken = true;
		}
	}

	public void OnFollowCharaExit(float time)
	{
	}

	private void OnReset()
	{
		this.interpolating = false;
		this.shakeCameraActive = false;
		this.sourceOffset = this.camModes[0].source;
		this.targetOffset = this.camModes[0].target;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
		{
			ChangePlayerCamera();
		}
    }

    internal void StartShakeCamera(FollowPlayer.ShakeData sData)
	{
		if (this.shakeCameraActive && sData.noise < this.currentShakeData.noise)
		{
			return;
		}
		this.currentShakeData = sData;
		this.shakeStartTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		this.shakeCameraActive = true;
	}

	private void UpdateTremor(float deltaTime)
	{
		Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
		this.noiseTremor += (onUnitSphere - this.noiseTremor) * deltaTime * 8f;
	}

	private void ShakeCamera(float deltaTime)
	{
		if (Singleton<TimeManager>.Instance.MasterSource.TotalTime - this.shakeStartTime <= this.currentShakeData.duration)
		{
			if (this.currentShakeData.smoothTime > 0f)
			{
				this.noiseStrength = Mathf.SmoothDamp(this.noiseStrength, this.currentShakeData.noise, ref this.noiseStrengthVel, this.currentShakeData.smoothTime, 300f, deltaTime);
			}
			else
			{
				this.noiseStrength = this.currentShakeData.noise;
			}
			if (this.noiseStrength > 0f)
			{
				Vector3 onUnitSphere = UnityEngine.Random.onUnitSphere;
				this.noise += (onUnitSphere - this.noise) * deltaTime * 8f;
			}
			else
			{
				this.noise = Vector3G.zero;
			}
		}
		if (Singleton<TimeManager>.Instance.MasterSource.TotalTime - this.shakeStartTime >= this.currentShakeData.duration)
		{
			this.StopShakeCamera();
		}
	}

	internal void StopShakeCamera()
	{
		this.currentShakeData = new FollowPlayer.ShakeData(0f, 0f, 0f);
		this.noiseStrength = 0f;
		this.noise = Vector3G.zero;
		this.shakeCameraActive = false;
	}

	/*public void ChangeCamera()
	{
		this.thisCam.fieldOfView = this.camModes[this.cameraIndex].foV;
		Vector3 source = this.camModes[this.cameraIndex].source;
		Vector3 target = this.camModes[this.cameraIndex].target;
		//this.StartEasing(source, target, 0.7f);
	}*/

	public void ChangePlayerCamera()
	{
        this.thisCam.fieldOfView = pD.camMode.foV;
        Vector3 source = pD.camMode.source;
        Vector3 target = pD.camMode.target;
        this.StartEasing(source, target, 0.7f);
    }

	public bool Slowmo
	{
		set
		{
			if (value)
			{
				Time.timeScale = 0.5f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}
			else
			{
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
			}
		}
	}

	internal void OnSlowMoActive(bool active)
	{
		this.cameraSwitchBtn.interactable = !active;
		this.Slowmo = active;
		if (active)
		{
			this.StartEasing(this.camSlowMo.source, this.camSlowMo.target, 1.2f);
		}
		else
		{
			//this.ChangeCamera();
			this.ChangePlayerCamera();
		}
	}

	public Camera thisCam;

	public CamMode[] camModes;

	public CamMode camSlowMo;

	private Vector3 noise = Vector3G.zero;

	private float noiseStrength;

	private float noiseStrengthVel;

	private float lastPivotHeight;

	private FollowPlayer.ShakeData currentShakeData;

	private Vector3 noiseTremor = Vector3.zero;

	private bool actionTaken;

	private bool interpolating;

	private bool firstFrame;

	private bool shakeCameraActive;

	private float deadTime = -1f;

	private float smoothTime = 0.1f;

	private float shakeStartTime;

	private float heightVelocity;

	private float startEaseTime;

	private float easeTimeDuration = 1f;

	private Vector3 sourceOffset;

	private Vector3 targetOffset;

	private Vector3 goalSourceOffset;

	private Vector3 goalTargetOffset;

	private Vector3 lastSourceOffset;

	private Vector3 lastTargetOffset;

	private Vector3 offset = Vector3.zero;

	private Vector3 prevPlayerPivot = Vector3.zero;

	private int c_Index;

	public Button cameraSwitchBtn;

	public Button sloMoButton;

	private const float slowmoEnterTime = 1.2f;

	private const float cameraSwitchTime = 0.7f;

	private const float slowmoSpeed = 0.45f;

	public class ShakeData
	{
		public ShakeData(float _duration, float _noise, float _smoothTime)
		{
			this.duration = _duration;
			this.noise = _noise;
			this.smoothTime = _smoothTime;
		}

		public float duration;

		public float noise;

		public float smoothTime;
	}
}
