// dnSpy decompiler from Assembly-CSharp.dll class: CurioAssets.StickerUtils
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CurioAssets
{
	public static class StickerUtils
	{
		public static MeshFilter[] GetAffectedObjects(Sticker decal)
		{
			Bounds bounds = StickerUtils.GetBounds(decal.transform);
			return (from obj in UnityEngine.Object.FindObjectsOfType<MeshRenderer>()
			where obj.gameObject.name.Contains("Paint") && obj.transform.IsChildOf(decal.mutualParent)
			where StickerUtils.HasLayer(decal.affectedLayers, obj.gameObject.layer)
			where obj.GetComponent<Sticker>() == null
			where bounds.Intersects(obj.bounds)
			select obj.GetComponent<MeshFilter>() into obj
			where obj != null && obj.sharedMesh != null
			select obj).ToArray<MeshFilter>();
		}

		private static bool HasLayer(LayerMask mask, int layer)
		{
			return (mask.value & 1 << layer) != 0;
		}

		private static Bounds GetBounds(Transform transform)
		{
			Vector3 lossyScale = transform.lossyScale;
			Vector3 b = -lossyScale / 2f;
			Vector3 a = lossyScale / 2f;
			Vector3[] array = new Vector3[]
			{
				new Vector3(b.x, b.y, b.z),
				new Vector3(a.x, b.y, b.z),
				new Vector3(b.x, a.y, b.z),
				new Vector3(a.x, a.y, b.z),
				new Vector3(b.x, b.y, a.z),
				new Vector3(a.x, b.y, a.z),
				new Vector3(b.x, a.y, a.z),
				new Vector3(a.x, a.y, a.z)
			};
			array = array.Select(new Func<Vector3, Vector3>(transform.TransformDirection)).ToArray<Vector3>();
			IEnumerable<Vector3> source = array;
			if (StickerUtils._003C_003Ef__mg_0024cache0 == null)
			{
				StickerUtils._003C_003Ef__mg_0024cache0 = new Func<Vector3, Vector3, Vector3>(Vector3.Min);
			}
			b = source.Aggregate(StickerUtils._003C_003Ef__mg_0024cache0);
			IEnumerable<Vector3> source2 = array;
			if (StickerUtils._003C_003Ef__mg_0024cache1 == null)
			{
				StickerUtils._003C_003Ef__mg_0024cache1 = new Func<Vector3, Vector3, Vector3>(Vector3.Max);
			}
			a = source2.Aggregate(StickerUtils._003C_003Ef__mg_0024cache1);
			return new Bounds(transform.position, a - b);
		}

		private const string PAINT = "Paint";

		[CompilerGenerated]
		private static Func<Vector3, Vector3, Vector3> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<Vector3, Vector3, Vector3> _003C_003Ef__mg_0024cache1;
	}
}
