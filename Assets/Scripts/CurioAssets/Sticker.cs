// dnSpy decompiler from Assembly-CSharp.dll class: CurioAssets.Sticker
using System;
using UnityEngine;

namespace CurioAssets
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[ExecuteInEditMode]
	public class Sticker : MonoBehaviour
	{
		public Texture texture
		{
			get
			{
				return (!this.material) ? null : this.material.mainTexture;
			}
		}

		private void Start()
		{
			base.transform.hasChanged = false;
		}

		public Transform mutualParent
		{
			get
			{
				return base.transform.parent.parent;
			}
		}

		public void ChangeTheSticker(Sprite sp)
		{
			this.sprite = sp;
			StickerBuilder.BuildAndSetDirty(this);
		}

		public Material material;

		public Sprite sprite;

		public float maxAngle = 90f;

		public float pushDistance = 0.009f;

		public LayerMask affectedLayers = -1;
	}
}
