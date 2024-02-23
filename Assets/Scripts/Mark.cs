// dnSpy decompiler from Assembly-CSharp.dll class: Mark
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Mark : MonoBehaviour
{
	public int UniqueId
	{
		get
		{
			return this.uniqueId;
		}
	}

	public float Length
	{
		get
		{
			Mark.MarkType markType = this.type;
			if (markType == Mark.MarkType.Rect)
			{
				return this.lengthOrRadius;
			}
			if (markType != Mark.MarkType.Curve)
			{
				return 0f;
			}
			return this.arcAngle * MathC.ToRadians * this.lengthOrRadius;
		}
		set
		{
			Mark.MarkType markType = this.type;
			if (markType != Mark.MarkType.Rect)
			{
				if (markType == Mark.MarkType.Curve)
				{
					this.arcAngle = value / (this.lengthOrRadius * MathC.ToRadians);
				}
			}
			else
			{
				this.lengthOrRadius = value;
			}
		}
	}

	public Vector3G Center
	{
		get
		{
			return this.matWorld.position - this.matWorld.MultiplyVector(Vector3G.right) * (this.lengthOrRadius * this.curveDir);
		}
	}

	public bool IsTrackToken
	{
		get
		{
			return this.trackBranchIndex != -1;
		}
	}

	public TrackBranch TrackBranch
	{
		get
		{
			return this.trackBranch;
		}
	}

	public int TrackBranchIndex
	{
		get
		{
			return this.trackBranchIndex;
		}
	}

	private void OnInit()
	{
		LevelObject component = base.gameObject.GetComponent<LevelObject>();
		if (component != null)
		{
			component.Bounds = this.ComputeBounds();
		}
	}

	public BoundsG ComputeBounds()
	{
		int i = 0;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		Mark.MarkType markType = this.type;
		if (markType != Mark.MarkType.Curve)
		{
			if (markType == Mark.MarkType.Rect)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(-this.width * 0.5f, 0f, 0f),
					new Vector3(this.width * 0.5f, 0f, 0f),
					new Vector3(-this.width * 0.5f, 0f, this.lengthOrRadius),
					new Vector3(this.width * 0.5f, 0f, this.lengthOrRadius)
				};
				while (i < 4)
				{
					Vector3 vector = base.transform.TransformPoint(array[i]);
					num = Mathf.Min(num, vector.x);
					num2 = Mathf.Min(num2, vector.z);
					num3 = Mathf.Max(num3, vector.x);
					num4 = Mathf.Max(num4, vector.z);
					i++;
				}
			}
		}
		else
		{
			float num5 = base.transform.rotation.y * MathC.ToRadians;
			float num6 = 1f / this.arcAngle;
			float[] array2 = new float[]
			{
				0f,
				(1.57079637f - num5) * num6,
				(3.14159274f - num5) * num6,
				(4.712389f - num5) * num6,
				(6.28318548f - num5) * num6,
				1f
			};
			while (i < 6)
			{
				Vector3G vector3G;
				Vector3G vector3G2;
				this.TokenToWorld(Mathf.Clamp01(array2[i]), -1f, out vector3G, out vector3G2);
				Vector3G vector3G3;
				this.TokenToWorld(Mathf.Clamp01(array2[i]), 1f, out vector3G3, out vector3G2);
				num = Mathf.Min(num, Mathf.Min(vector3G.x, vector3G3.x));
				num2 = Mathf.Min(num2, Mathf.Min(vector3G.z, vector3G3.z));
				num3 = Mathf.Max(num3, Mathf.Max(vector3G.x, vector3G3.x));
				num4 = Mathf.Max(num4, Mathf.Max(vector3G.z, vector3G3.z));
				i++;
			}
		}
		BoundsG result = default(BoundsG);
		result.min.Set(num, 0f, num2);
		result.max.Set(num3, 0f, num4);
		return result;
	}

	public void LinkToTrack(TrackBranch _trackBranch, int _trackBranchIndex)
	{
		this.trackBranch = _trackBranch;
		this.trackBranchIndex = _trackBranchIndex;
	}

	public void OnMove()
	{
		this.matWorld = base.transform.localToWorldMatrix;
		this.matInvWorld = this.matWorld.inverseFast;
	}

	public void CreateMesh(int numSegments, ref Mesh mesh, bool append)
	{
		Vector3[] array = new Vector3[(numSegments + 1) * 2];
		int[] array2 = new int[numSegments * 6];
		float num = 0f;
		float num2 = 1f / (float)numSegments;
		this.OnMove();
		for (int i = 0; i < numSegments + 1; i++)
		{
			Vector3G v;
			Vector3G vector3G;
			this.TokenToWorld(num, -1f, out v, out vector3G);
			Vector3G v2;
			Vector3G vector3G2;
			this.TokenToWorld(num, 1f, out v2, out vector3G2);
			array[i * 2] = v;
			array[i * 2 + 1] = v2;
			num += num2;
		}
		for (int i = 0; i < numSegments; i++)
		{
			array2[i * 6] = i * 2;
			array2[i * 6 + 1] = i * 2 + 3;
			array2[i * 6 + 2] = i * 2 + 1;
			array2[i * 6 + 3] = i * 2 + 3;
			array2[i * 6 + 4] = i * 2;
			array2[i * 6 + 5] = i * 2 + 2;
		}
		if (null == mesh)
		{
			mesh = new Mesh();
			append = false;
		}
		if (append)
		{
			Vector3[] vertices = mesh.vertices;
			Vector3[] array3 = new Vector3[vertices.Length + array.Length];
			for (int j = 0; j < array3.Length; j++)
			{
				if (j < vertices.Length)
				{
					array3[j] = vertices[j];
				}
				else
				{
					array3[j] = array[j - vertices.Length];
				}
			}
			mesh.vertices = array3;
			int[] triangles = mesh.triangles;
			int[] array4 = new int[triangles.Length + array2.Length];
			for (int j = 0; j < array4.Length; j++)
			{
				if (j < triangles.Length)
				{
					array4[j] = triangles[j];
				}
				else
				{
					array4[j] = vertices.Length + array2[j - triangles.Length];
				}
			}
			mesh.triangles = array4;
		}
		else
		{
			mesh.vertices = array;
			mesh.triangles = array2;
		}
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	public void TokenToWorld(float longitudinal, float trasversal, out Vector3G pos, out Vector3G tang)
	{
		Mark.MarkType markType = this.type;
		if (markType != Mark.MarkType.Rect)
		{
			if (markType != Mark.MarkType.Curve)
			{
				pos.z = 0f; pos.x = (pos.y = (pos.z ));
				tang.z = 0f; tang.x = (tang.y = (tang.z ));
			}
			else
			{
				Vector3G t = -this.curveDir * this.lengthOrRadius * Vector3G.right;
				Vector3G p = (trasversal * this.width * 0.5f + this.curveDir * this.lengthOrRadius) * Vector3G.right;
				Matrix4x4G m = Matrix4x4G.TRS(t, QuaternionG.identity, Vector3G.one);
				Matrix4x4G m2 = Matrix4x4G.TRS(Vector3G.zero, QuaternionG.AngleAxis(-this.arcAngle * this.curveDir * longitudinal, Vector3G.up), Vector3G.one);
				pos = (this.matWorld * m * m2).MultiplyPoint3x4(p);
				tang = (this.matWorld * m2).MultiplyVector(Vector3G.forward);
			}
		}
		else
		{
			pos = this.matWorld.MultiplyPoint3x4(Vector3G.right * (trasversal * this.width * 0.5f) + Vector3G.forward * (longitudinal * this.lengthOrRadius));
			tang = this.matWorld.MultiplyVector(Vector3G.forward);
		}
	}

	public void WorldToToken(Vector3G worldPos, out float longitudinal, out float trasversal)
	{
		Mark.MarkType markType = this.type;
		if (markType != Mark.MarkType.Rect)
		{
			if (markType != Mark.MarkType.Curve)
			{
				longitudinal = 0f;
				trasversal = 0f;
			}
			else
			{
				Vector3G center = this.Center;
				Vector3G v = this.matWorld.position - center;
				Vector3G vector3G = worldPos - center;
				v.DecrementBy(Vector3G.Dot(v, Vector3G.up) * Vector3G.up);
				vector3G.DecrementBy(Vector3G.Dot(vector3G, Vector3G.up) * Vector3G.up);
				float num = Mathf.Sign(Vector3G.Dot(Vector3G.Cross(v, vector3G), Vector3G.up));
				float num2 = Vector3G.Angle(v, vector3G);
				longitudinal = num2 / this.arcAngle;
				if (num == this.curveDir)
				{
					longitudinal *= -1f;
				}
				trasversal = (vector3G.magnitude - this.lengthOrRadius) * 2f * this.curveDir / this.width;
			}
		}
		else
		{
			Vector3G v2 = this.matInvWorld.MultiplyPoint3x4(worldPos);
			longitudinal = Vector3G.Dot(v2, Vector3G.forward) / this.lengthOrRadius;
			trasversal = Vector3G.Dot(v2, Vector3G.right) * 2f / this.width;
		}
	}

	public void CreateBoxColliders(int numSegments, float trasversalOffset, float wallsHeight, float depth)
	{
		if (numSegments < 1)
		{
			return;
		}
		BoxCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider boxCollider in componentsInChildren)
		{
			UnityEngine.Object.DestroyImmediate(boxCollider.gameObject);
		}
		float num = 0f;
		float num2 = 1f / (float)numSegments;
		for (int j = 0; j < numSegments; j++)
		{
			Vector3G vector3G;
			Vector3G vector3G2;
			this.TokenToWorld(num, -1f - trasversalOffset, out vector3G, out vector3G2);
			Vector3G vector3G3;
			this.TokenToWorld(num + num2, -1f - trasversalOffset, out vector3G3, out vector3G2);
			Vector3G vector3G4;
			this.TokenToWorld(num, 1f + trasversalOffset, out vector3G4, out vector3G2);
			Vector3G vector3G5;
			this.TokenToWorld(num + num2, 1f + trasversalOffset, out vector3G5, out vector3G2);
			Vector3G v = vector3G3 - vector3G;
			Vector3G v2 = vector3G5 - vector3G4;
			Vector3G v3 = vector3G4 - vector3G;
			float x = v3.Normalize();
			float num3 = v.Normalize() + 0.2f;
			float num4 = v2.Normalize() + 0.2f;
			QuaternionG q = QuaternionG.AngleAxis(Vector3G.Angle(Vector3G.forward, v), Vector3G.Cross(Vector3G.forward, v));
			QuaternionG q2 = QuaternionG.AngleAxis(Vector3G.Angle(Vector3G.forward, v2), Vector3G.Cross(Vector3G.forward, v2));
			Vector3G v4 = (vector3G + vector3G3 + vector3G4 + vector3G5) * 0.25f - Vector3G.up * depth * 0.5f;
			Vector3G v5 = (vector3G + vector3G3 + Vector3G.up * wallsHeight - v3 * depth) * 0.5f;
			Vector3G v6 = (vector3G4 + vector3G5 + Vector3G.up * wallsHeight + v3 * depth) * 0.5f;
			Vector3G v7 = new Vector3G(x, depth, Mathf.Max(num3, num4));
			Vector3G v8 = new Vector3G(depth, wallsHeight, num3);
			Vector3G v9 = new Vector3G(depth, wallsHeight, num4);
			new GameObject("Ground", new Type[]
			{
				typeof(BoxCollider)
			})
			{
				transform = 
				{
					parent = base.transform,
					rotation = q,
					position = v4
				}
			}.GetComponent<BoxCollider>().size = v7;
			GameObject gameObject = new GameObject("WallLeft", new Type[]
			{
				typeof(BoxCollider)
			});
			GameObject gameObject2 = new GameObject("WallRight", new Type[]
			{
				typeof(BoxCollider)
			});
			Transform transform = gameObject.transform;
			Transform transform2 = base.transform;
			gameObject2.transform.parent = transform2;
			transform.parent = transform2;
			gameObject.transform.rotation = q;
			gameObject.GetComponent<BoxCollider>().size = v8;
			gameObject.transform.position = v5;
			gameObject2.transform.rotation = q2;
			gameObject2.GetComponent<BoxCollider>().size = v9;
			gameObject2.transform.position = v6;
			num += num2;
		}
	}

	private void Awake()
	{
		this.matWorld = base.transform.localToWorldMatrix;
		this.matInvWorld = this.matWorld.inverseFast;
	}

	private static int uniqueIdCounter;

	private int uniqueId = Mark.uniqueIdCounter++;

	private Matrix4x4G matWorld = default(Matrix4x4G);

	private Matrix4x4G matInvWorld = default(Matrix4x4G);

	private TrackBranch trackBranch;

	private int trackBranchIndex = -1;

	public Mark.MarkType type;

	public float lengthOrRadius;

	public float width;

	[HideInInspector]
	public float arcAngle;

	[HideInInspector]
	public float curveDir;

	public List<Mark> prevLinks = new List<Mark>();

	public List<Mark> nextLinks = new List<Mark>();

	public enum MarkType
	{
		Rect,
		Curve
	}
}
