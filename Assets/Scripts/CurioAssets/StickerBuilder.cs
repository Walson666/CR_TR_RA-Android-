// dnSpy decompiler from Assembly-CSharp.dll class: CurioAssets.StickerBuilder
using System;
using System.Linq;
using UnityEngine;

namespace CurioAssets
{
	public static class StickerBuilder
	{
		public static GameObject[] BuildAndSetDirty(Sticker decal)
		{
			return StickerBuilder.Build(StickerBuilder.builder, decal);
		}

		private static GameObject[] Build(MeshBuilder builder, Sticker decal)
		{
			MeshFilter meshFilter = decal.GetComponent<MeshFilter>() ?? decal.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = decal.GetComponent<MeshRenderer>() ?? decal.gameObject.AddComponent<MeshRenderer>();
			if (meshFilter.sharedMesh != null && !meshFilter.sharedMesh.isReadable)
			{
				return null;
			}
			if (decal.material == null || decal.sprite == null)
			{
				UnityEngine.Object.DestroyImmediate(meshFilter.sharedMesh);
				meshFilter.sharedMesh = null;
				meshRenderer.sharedMaterial = null;
				return null;
			}
			MeshFilter[] affectedObjects = StickerUtils.GetAffectedObjects(decal);
			foreach (MeshFilter @object in affectedObjects)
			{
				StickerBuilder.Build(builder, decal, @object);
			}
			builder.Push(decal.pushDistance);
			if (meshFilter.sharedMesh == null)
			{
				meshFilter.sharedMesh = new Mesh();
				meshFilter.sharedMesh.name = StickerBuilder.GetName(decal.name, decal.transform.parent.name, decal.mutualParent.name);
			}
			builder.ToMesh(meshFilter.sharedMesh);
			meshRenderer.sharedMaterial = decal.material;
			return (from i in affectedObjects
			select i.gameObject).ToArray<GameObject>();
		}

		public static string GetName(string n, string pN, string ppN)
		{
			return string.Format("{0}{1}{2}", n, pN, ppN);
		}

		private static void Build(MeshBuilder builder, Sticker decal, MeshFilter @object)
		{
			Matrix4x4 matrix4x = decal.transform.worldToLocalMatrix * @object.transform.localToWorldMatrix;
			Mesh sharedMesh = @object.sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			int[] triangles = sharedMesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				Vector3 v = matrix4x.MultiplyPoint(vertices[num]);
				Vector3 v2 = matrix4x.MultiplyPoint(vertices[num2]);
				Vector3 v3 = matrix4x.MultiplyPoint(vertices[num3]);
				StickerBuilder.AddTriangle(builder, decal, v, v2, v3);
			}
		}

		private static void AddTriangle(MeshBuilder builder, Sticker decal, Vector3 v1, Vector3 v2, Vector3 v3)
		{
			Rect uvRect = StickerBuilder.To01(decal.sprite.textureRect, decal.sprite.texture);
			Vector3 normalized = Vector3.Cross(v2 - v1, v3 - v1).normalized;
			if (Vector3.Angle(Vector3.forward, -normalized) <= decal.maxAngle)
			{
				Vector3[] array = PolygonClippingUtils.Clip(new Vector3[]
				{
					v1,
					v2,
					v3
				});
				if (array.Length > 0)
				{
					builder.AddPolygon(array, normalized, uvRect);
				}
			}
		}

		private static Rect To01(Rect rect, Texture2D texture)
		{
			rect.x /= (float)texture.width;
			rect.y /= (float)texture.height;
			rect.width /= (float)texture.width;
			rect.height /= (float)texture.height;
			return rect;
		}

		private static readonly MeshBuilder builder = new MeshBuilder();
	}
}
