// dnSpy decompiler from Assembly-CSharp.dll class: EndlessPlayer
using System;
using UnityEngine;

public class EndlessPlayer : MonoBehaviour
{
	public float StartTrackPos
	{
		get
		{
			return this.startTrackPos;
		}
	}

	public float Meters
	{
		get
		{
			return Mathf.Max(0f, this.trackPos - this.startTrackPos);
		}
	}

	public float SessionMeters
	{
		get
		{
			return this.sessionMeters;
		}
	}

	public float TrackPos
	{
		get
		{
			return this.trackPos;
		}
	}

	public Vector3G TokenTangent
	{
		get
		{
			return this.tokenTan;
		}
	}

	public float TokenTrasversal
	{
		get
		{
			return this.tokenTrasversal;
		}
	}

	public float TokenLongitudinal
	{
		get
		{
			return this.tokenLongitudinal;
		}
	}

	public Matrix4x4G BlockWorld
	{
		get
		{
			return this.blockWorld;
		}
	}

	public EndlessTrackInternal.BlocksBranch ActiveBranch
	{
		get
		{
			return this.activeBranch;
		}
	}

	public EndlessTrackInternal.BlockNode ActiveBlock
	{
		get
		{
			return this.activeBlock;
		}
	}

	public bool IsDied
	{
		get
		{
			return this.died;
		}
	}

	public int LastTokenIndex
	{
		get
		{
			return this.lastTokenIndex;
		}
	}

	public virtual void StartRunning(EndlessTrackInternal.BlocksBranch firstBranch)
	{
		this.activeBranch = firstBranch;
		this.prevBlock = null;
		this.activeBlock = this.activeBranch.head;
		this.blockWorld = this.activeBlock.transform.localToWorldMatrix;
		base.gameObject.SetActive(true);
		base.gameObject.SendMessage("OnStartRunning", SendMessageOptions.DontRequireReceiver);
		base.gameObject.SendMessage("OnBlockChanged", SendMessageOptions.DontRequireReceiver);
		Vector3G worldPos = base.transform.position;
		this.activeBranch.trackBranch[0].WorldToToken(worldPos, out this.tokenLongitudinal, out this.tokenTrasversal);
		this.activeBranch.trackBranch[0].TokenToWorld(this.tokenLongitudinal, this.tokenTrasversal, out this.tokenPos, out this.tokenTan);
		Camera.main.gameObject.SendMessage("OnStartRunning", SendMessageOptions.DontRequireReceiver);
		Camera.main.gameObject.SendMessage("OnBlockChanged", SendMessageOptions.DontRequireReceiver);
		this.died = false;
		this.sessionMeters = 0f;
	}

	public void EndRunning()
	{
		base.gameObject.SendMessage("OnEndRunning", SendMessageOptions.DontRequireReceiver);
		Camera.main.gameObject.SendMessage("OnEndRunning", SendMessageOptions.DontRequireReceiver);
		this.activeBranch = null;
		this.prevBlock = null;
		this.activeBlock = null;
		this.tokenLongitudinal = 0f;
		this.tokenTrasversal = 0f;
		this.lastTokenIndex = 0;
		this.trackPos = 0f;
		this.startTrackPos = 0f;
		this.sessionMeters = 0f;
		base.gameObject.SetActive(false);
	}

	public virtual int GetDifficultyValue()
	{
		return Mathf.Min(100, Mathf.RoundToInt(this.sessionMeters * 0.05f));
	}

	protected virtual bool SelectBranch(EndlessBlock block, int linkIndex)
	{
		return true;
	}

	private void OnTutorialEnd()
	{
		this.startTrackPos = this.trackPos;
	}

	private void OnDie()
	{
		this.died = true;
	}

	private void LateUpdate()
	{
		if (this.activeBranch == null)
		{
			return;
		}
		Vector3G worldPos = base.transform.position;
		this.lastToken = null;
		this.lastToken = this.activeBranch.trackBranch[this.lastTokenIndex];
		this.lastToken.WorldToToken(worldPos, out this.tokenLongitudinal, out this.tokenTrasversal);
		while (this.tokenLongitudinal > 1f)
		{
			this.lastTokenIndex++;
			if (this.lastTokenIndex < this.activeBranch.trackBranch.Count)
			{
				this.lastToken = this.activeBranch.trackBranch[this.lastTokenIndex];
				if (this.activeBlock.next != null && this.lastToken == this.activeBlock.next.component.firstToken)
				{
					this.activeBlock = this.activeBlock.next;
					this.blockWorld = this.activeBlock.transform.localToWorldMatrix;
					base.gameObject.SendMessage("OnBlockChanged", SendMessageOptions.DontRequireReceiver);
				}
				this.lastToken.WorldToToken(worldPos, out this.tokenLongitudinal, out this.tokenTrasversal);
			}
			else
			{
				bool flag = false;
				EndlessBlock component = this.activeBlock.component;
				for (int i = 1; i < component.links.Length; i++)
				{
					if (this.SelectBranch(component, i))
					{
						this.activeBranch = component.links[i].branch;
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Choosed link #",
							i - 1,
							", Token ",
							this.activeBranch.trackBranch[0].name
						}));
						this.lastTokenIndex = 0;
						flag = true;
						break;
					}
				}
				this.lastToken = this.activeBranch.trackBranch[this.lastTokenIndex];
				this.lastToken.WorldToToken(worldPos, out this.tokenLongitudinal, out this.tokenTrasversal);
				if (flag)
				{
					base.gameObject.SendMessage("OnBranchChanged", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		if (this.lastToken != null)
		{
			this.lastToken.TokenToWorld(this.tokenLongitudinal, this.tokenTrasversal, out this.tokenPos, out this.tokenTan);
			this.trackPos = this.activeBranch.trackBranch.GetTrackPosition(this.lastToken, this.tokenLongitudinal);
		}
		if (!this.died)
		{
			this.sessionMeters = this.trackPos - this.startTrackPos;
		}
	}

	protected float tokenLongitudinal;

	protected float tokenTrasversal;

	protected int lastTokenIndex;

	protected float trackPos;

	protected Vector3G tokenPos = default(Vector3G);

	protected Vector3G tokenTan = default(Vector3G);

	protected EndlessTrackInternal.BlocksBranch activeBranch;

	protected EndlessTrackInternal.BlockNode prevBlock;

	protected EndlessTrackInternal.BlockNode activeBlock;

	protected Matrix4x4G blockWorld = default(Matrix4x4G);

	protected bool died;

	protected float startTrackPos;

	protected float sessionMeters;

	internal Mark lastToken;
}
