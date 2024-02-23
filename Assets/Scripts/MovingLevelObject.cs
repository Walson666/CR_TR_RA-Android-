// dnSpy decompiler from Assembly-CSharp.dll class: MovingLevelObject
using System;
using UnityEngine;

public class MovingLevelObject : LevelObject
{
	public override bool IsMovable
	{
		get
		{
			return true;
		}
	}

	public Matrix4x4G PrevLocalToWorld
	{
		get
		{
			return this._prevLocalToWorld;
		}
	}

	public Matrix4x4G PrevWorldToLocal
	{
		get
		{
			return this._prevWorldToLocal;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		this._prevLocalToWorld = this._transform.localToWorldMatrix;
		this._prevWorldToLocal = this._transform.worldToLocalMatrix;
	}

	private void Update()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this._bounds = base.GetComponent<Renderer>().bounds;
		}
		else if (base.GetComponent<Collider>() != null)
		{
			this._bounds = base.GetComponent<Collider>().bounds;
		}
		else
		{
			this._bounds = new BoundsG(this._transform.position, Vector3G.zero);
		}
		if (this.cell == null)
		{
			this.cell = Singleton<LevelManager>.Instance.RootCell;
			if (this.cell == null)
			{
				return;
			}
			this.cell.AttachObject(this);
		}
		LevelCell levelCell = this.cell.FindObjectContainmentCell(this);
		if (levelCell != this.cell)
		{
			if (this.cell != null)
			{
				this.cell.DetachObject(this);
			}
			this.cell = levelCell;
			this.cell.AttachObject(this);
		}
		this._prevLocalToWorld = this._localToWorld;
		this._prevWorldToLocal = this._worldToLocal;
		if (this._transform == null)
		{
			return;
		}
		this._localToWorld = this._transform.localToWorldMatrix;
		this._worldToLocal = this._transform.worldToLocalMatrix;
	}

	protected Matrix4x4G _prevLocalToWorld;

	protected Matrix4x4G _prevWorldToLocal;
}
