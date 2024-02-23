// dnSpy decompiler from Assembly-CSharp.dll class: CurioAssets.PolygonClippingUtils
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CurioAssets
{
	public static class PolygonClippingUtils
	{
		public static Vector3[] Clip(params Vector3[] poly)
		{
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.right).ToArray<Vector3>();
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.left).ToArray<Vector3>();
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.top).ToArray<Vector3>();
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.bottom).ToArray<Vector3>();
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.front).ToArray<Vector3>();
			poly = PolygonClippingUtils.Clip(poly, PolygonClippingUtils.back).ToArray<Vector3>();
			return poly;
		}

		private static IEnumerable<Vector3> Clip(Vector3[] poly, Plane plane)
		{
			for (int i = 0; i < poly.Length; i++)
			{
				int next = (i + 1) % poly.Length;
				Vector3 v = poly[i];
				Vector3 v2 = poly[next];
				if (plane.GetSide(v))
				{
					yield return v;
				}
				if (plane.GetSide(v) != plane.GetSide(v2))
				{
					yield return PolygonClippingUtils.PlaneLineCast(plane, v, v2);
				}
			}
			yield break;
		}

		private static Vector3 PlaneLineCast(Plane plane, Vector3 a, Vector3 b)
		{
			Ray ray = new Ray(a, b - a);
			float distance;
			plane.Raycast(ray, out distance);
			return ray.GetPoint(distance);
		}

		private static readonly Plane right = new Plane(Vector3.right, 0.5f);

		private static readonly Plane left = new Plane(Vector3.left, 0.5f);

		private static readonly Plane top = new Plane(Vector3.up, 0.5f);

		private static readonly Plane bottom = new Plane(Vector3.down, 0.5f);

		private static readonly Plane front = new Plane(Vector3.forward, 0.5f);

		private static readonly Plane back = new Plane(Vector3.back, 0.5f);
	}
}
