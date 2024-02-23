// dnSpy decompiler from Assembly-CSharp.dll class: LevelObject
using System;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
	public Transform Transform
	{
		get
		{
			return this._transform;
		}
	}

	public Matrix4x4G LocalToWorld
	{
		get
		{
			return this._localToWorld;
		}
	}

	public Matrix4x4G WorldToLocal
	{
		get
		{
			return this._worldToLocal;
		}
	}

	public BoundsG Bounds
	{
		get
		{
			return this._bounds;
		}
		set
		{
			this._bounds = value;
			LevelCell levelCell = this.cell.FindObjectContainmentCell(this);
			if (levelCell != this.cell)
			{
				this.cell.DetachObject(this);
				this.cell = levelCell;
				this.cell.AttachObject(this);
			}
		}
	}

	public string Category
	{
		get
		{
			return this.category;
		}
		set
		{
			if (this.cell != null)
			{
				this.cell.NotifyObjectCategory(this, this.category, value);
			}
			this.category = value;
		}
	}

	public int LayersMask
	{
		get
		{
			return this.layersMask;
		}
		set
		{
			if (this.cell != null)
			{
				this.cell.NotifyObjectLayersMask(this, this.layersMask, value);
			}
			this.layersMask = value;
		}
	}

	public virtual bool IsMovable
	{
		get
		{
			return false;
		}
	}

	public LevelCell Cell
	{
		get
		{
			return this.cell;
		}
	}

	public virtual void Initialize()
	{
		this._transform = base.transform;
		this._localToWorld = this._transform.localToWorldMatrix;
		this._worldToLocal = this._transform.worldToLocalMatrix;
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
			this._bounds = new BoundsG(this._localToWorld.position, Vector3G.zero);
		}
		this.cell = Singleton<LevelManager>.Instance.RootCell.FindObjectContainmentCell(this);
		this.cell.AttachObject(this);
		base.gameObject.SendMessage("OnInit", SendMessageOptions.DontRequireReceiver);
	}

	public string category;

	public int layersMask;

	protected Transform _transform;

	protected Matrix4x4G _localToWorld;

	protected Matrix4x4G _worldToLocal;

	protected BoundsG _bounds;

	protected LevelCell cell;
}
