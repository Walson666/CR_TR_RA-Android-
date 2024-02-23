// dnSpy decompiler from Assembly-CSharp.dll class: LevelManager
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
	public Transform Root
	{
		get
		{
			return base.transform;
		}
	}

	public LevelCell RootCell
	{
		get
		{
			return this.rootCell;
		}
	}

	protected void RecursiveBuildQuadTree(LevelCell parentCell, LevelCell cell, int depth, int maxDepth, bool yUp)
	{
		cell.Parent = parentCell;
		if (maxDepth == depth)
		{
			return;
		}
		Vector3G size = new Vector3G(cell.Bounds.size.x * 0.5f, (!yUp) ? (cell.Bounds.size.y * 0.5f) : cell.Bounds.size.y, (!yUp) ? cell.Bounds.size.z : (cell.Bounds.size.z * 0.5f));
		Vector3G center = new Vector3G((cell.Bounds.min.x + cell.Bounds.center.x) * 0.5f, (!yUp) ? ((cell.Bounds.min.y + cell.Bounds.center.y) * 0.5f) : cell.Bounds.center.y, (!yUp) ? cell.Bounds.center.z : ((cell.Bounds.min.z + cell.Bounds.center.z) * 0.5f));
		Vector3G center2 = new Vector3G((cell.Bounds.max.x + cell.Bounds.center.x) * 0.5f, (!yUp) ? ((cell.Bounds.min.y + cell.Bounds.center.y) * 0.5f) : cell.Bounds.center.y, (!yUp) ? cell.Bounds.center.z : ((cell.Bounds.min.z + cell.Bounds.center.z) * 0.5f));
		Vector3G center3 = new Vector3G((cell.Bounds.max.x + cell.Bounds.center.x) * 0.5f, (!yUp) ? ((cell.Bounds.max.y + cell.Bounds.center.y) * 0.5f) : cell.Bounds.center.y, (!yUp) ? cell.Bounds.center.z : ((cell.Bounds.max.z + cell.Bounds.center.z) * 0.5f));
		Vector3G center4 = new Vector3G((cell.Bounds.min.x + cell.Bounds.center.x) * 0.5f, (!yUp) ? ((cell.Bounds.max.y + cell.Bounds.center.y) * 0.5f) : cell.Bounds.center.y, (!yUp) ? cell.Bounds.center.z : ((cell.Bounds.max.z + cell.Bounds.center.z) * 0.5f));
		this.RecursiveBuildQuadTree(cell, new LevelCell(new BoundsG(center, size)), depth + 1, maxDepth, yUp);
		this.RecursiveBuildQuadTree(cell, new LevelCell(new BoundsG(center2, size)), depth + 1, maxDepth, yUp);
		this.RecursiveBuildQuadTree(cell, new LevelCell(new BoundsG(center3, size)), depth + 1, maxDepth, yUp);
		this.RecursiveBuildQuadTree(cell, new LevelCell(new BoundsG(center4, size)), depth + 1, maxDepth, yUp);
	}

	protected LevelCell BuildQuadTree(BoundsG bounds, int depth, bool yUp)
	{
		LevelCell levelCell = new LevelCell(bounds);
		this.RecursiveBuildQuadTree(null, levelCell, 0, depth - 1, yUp);
		return levelCell;
	}

	public void Initialize()
	{
		this.worldBounds = default(BoundsG);
		this.worldBounds.min = this.worldBoundsMin;
		this.worldBounds.max = this.worldBoundsMax;
		this.rootCell = this.BuildQuadTree(this.worldBounds, 5, true);
		LevelObject[] componentsInChildren = base.gameObject.GetComponentsInChildren<LevelObject>(true);
		foreach (LevelObject levelObject in componentsInChildren)
		{
			levelObject.Initialize();
		}
		foreach (LevelObject levelObject2 in componentsInChildren)
		{
			levelObject2.gameObject.SendMessage("OnPostInit", SendMessageOptions.DontRequireReceiver);
		}
	}

	public LevelObject[] Query(BoundsG bbox, string category, int mask)
	{
		if (this.rootCell == null)
		{
			return new LevelObject[0];
		}
		List<LevelObject> list = new List<LevelObject>();
		this.rootCell.RecurseQuery(bbox, MathC.ClipStatus.Overlapping, category, mask, list);
		return list.ToArray();
	}

	public LevelObject[] Query(BoundsG bbox, string category)
	{
		return this.Query(bbox, category, -1);
	}

	public LevelObject[] Query(BoundsG bbox, int mask)
	{
		return this.Query(bbox, null, mask);
	}

	public LevelObject[] Query(BoundsG bbox)
	{
		return this.Query(bbox, null, -1);
	}

	public LevelObject[] Query(string category)
	{
		if (this.rootCell == null)
		{
			return new LevelObject[0];
		}
		return this.Query(this.rootCell.Bounds, category, -1);
	}

	public LevelObject[] Query(int mask)
	{
		if (this.rootCell == null)
		{
			return new LevelObject[0];
		}
		return this.Query(this.rootCell.Bounds, null, mask);
	}

	public static void SetUtcDate(string key, DateTime dateTime)
	{
		PlayerPrefs.SetString(key, dateTime.Ticks.ToString(CultureInfo.InvariantCulture));
	}

	public static DateTime GetUtcDate(string key)
	{
		string @string = LevelManager.GetString(key, string.Empty);
		long ticks;
		if (!string.IsNullOrEmpty(@string) && long.TryParse(@string, NumberStyles.None, CultureInfo.InvariantCulture, out ticks))
		{
			return new DateTime(ticks, DateTimeKind.Utc);
		}
		return new DateTime(0L, DateTimeKind.Utc);
	}

	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	private Vector3 worldBoundsMin = Vector3.zero;

	private Vector3 worldBoundsMax = Vector3.zero;

	protected LevelCell rootCell;

	protected BoundsG worldBounds;
}
