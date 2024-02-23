// dnSpy decompiler from Assembly-CSharp.dll class: CurioAssets.MeshBuilder
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CurioAssets
{
	public class MeshBuilder
	{
		public void AddPolygon(Vector3[] poly, Vector3 normal, Rect uvRect)
		{
			int item = this.AddVertex(poly[0], normal, uvRect);
			for (int i = 1; i < poly.Length - 1; i++)
			{
				int item2 = this.AddVertex(poly[i], normal, uvRect);
				int item3 = this.AddVertex(poly[i + 1], normal, uvRect);
				this.indices.Add(item);
				this.indices.Add(item2);
				this.indices.Add(item3);
			}
		}

		private int AddVertex(Vector3 vertex, Vector3 normal, Rect uvRect)
		{
			int num = this.FindVertex(vertex);
			if (num == -1)
			{
				this.vertices.Add(vertex);
				this.normals.Add(normal);
				this.AddTexCoord(vertex, uvRect);
				return this.vertices.Count - 1;
			}
			this.normals[num] = (this.normals[num] + normal).normalized;
			return num;
		}

		private int FindVertex(Vector3 vertex)
		{
			for (int i = 0; i < this.vertices.Count; i++)
			{
				if (Vector3.Distance(this.vertices[i], vertex) < 0.01f)
				{
					return i;
				}
			}
			return -1;
		}

		private void AddTexCoord(Vector3 ver, Rect uvRect)
		{
			float x = Mathf.Lerp(uvRect.xMin, uvRect.xMax, ver.x + 0.5f);
			float y = Mathf.Lerp(uvRect.yMin, uvRect.yMax, ver.y + 0.5f);
			this.texCoords.Add(new Vector2(x, y));
		}

		public void Push(float distance)
		{
			for (int i = 0; i < this.vertices.Count; i++)
			{
				List<Vector3> list;
				int index;
				list = this.vertices; index = i; (list )[index ] = list[index] + this.normals[i] * distance;
			}
		}

		public void ToMesh(Mesh mesh)
		{
			mesh.Clear(true);
			if (this.indices.Count == 0)
			{
				return;
			}
			mesh.vertices = this.vertices.ToArray();
			mesh.normals = this.normals.ToArray();
			mesh.uv = this.texCoords.ToArray();
			mesh.uv2 = this.texCoords.ToArray();
			mesh.triangles = this.indices.ToArray();
			this.vertices.Clear();
			this.normals.Clear();
			this.texCoords.Clear();
			this.indices.Clear();
		}

		private readonly List<Vector3> vertices = new List<Vector3>();

		private readonly List<Vector3> normals = new List<Vector3>();

		private readonly List<Vector2> texCoords = new List<Vector2>();

		private readonly List<int> indices = new List<int>();
	}
}
