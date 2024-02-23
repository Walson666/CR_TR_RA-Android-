// dnSpy decompiler from Assembly-CSharp.dll class: LevelCell
using System;
using System.Collections.Generic;

public class LevelCell
{
	private LevelCell()
	{
	}

	public LevelCell(BoundsG _bounds)
	{
		this.bounds = _bounds;
		this.objects = new List<LevelObject>();
		this.numObjectsByCat = new Dictionary<string, int>();
		this.numObjectsByMask = new int[32];
		this.parent = null;
		this.children = new List<LevelCell>();
		for (int i = 0; i < 32; i++)
		{
			this.numObjectsByMask[i] = 0;
		}
	}

	public BoundsG Bounds
	{
		get
		{
			return this.bounds;
		}
	}

	public IEnumerable<LevelObject> Objects
	{
		get
		{
			return this.objects;
		}
	}

	public LevelCell Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			if (this.parent != null)
			{
				this.parent.children.Remove(this);
			}
			this.parent = value;
			if (this.parent != null)
			{
				this.parent.children.Add(this);
			}
		}
	}

	public IEnumerable<LevelCell> Children
	{
		get
		{
			return this.children;
		}
	}

	protected void UpdateNumObjectsOfCategory(string category, int delta)
	{
		if (category == null)
		{
			return;
		}
		if (!this.numObjectsByCat.ContainsKey(category))
		{
			this.numObjectsByCat.Add(category, 0);
		}
		Dictionary<string, int> dictionary;
		dictionary = this.numObjectsByCat; (dictionary )[category] = dictionary[category] + delta;
		if (this.parent != null)
		{
			this.parent.UpdateNumObjectsOfCategory(category, delta);
		}
	}

	protected void UpdateNumObjectsWithMask(int mask, int delta)
	{
		if (mask == 0)
		{
			return;
		}
		int num = 1;
		for (int i = 0; i < 32; i++)
		{
			if ((mask & num) != 0)
			{
				this.numObjectsByMask[i] += delta;
			}
			num <<= 1;
		}
		if (this.parent != null)
		{
			this.parent.UpdateNumObjectsWithMask(mask, delta);
		}
	}

	public int GetNumObjectsOfCategory(string category)
	{
		int result;
		if (this.numObjectsByCat.TryGetValue(category, out result))
		{
			return result;
		}
		return 0;
	}

	public int GetNumObjectsWithMask(int mask)
	{
		int num = 0;
		int num2 = 1;
		for (int i = 0; i < 32; i++)
		{
			if ((mask & num2) != 0)
			{
				num += this.numObjectsByMask[i];
			}
			num2 <<= 1;
		}
		return num;
	}

	public void AttachObject(LevelObject obj)
	{
		this.objects.Add(obj);
		this.UpdateNumObjectsOfCategory(obj.Category, 1);
		this.UpdateNumObjectsWithMask(obj.LayersMask, 1);
	}

	public void DetachObject(LevelObject obj)
	{
		this.objects.Remove(obj);
		this.UpdateNumObjectsOfCategory(obj.Category, -1);
		this.UpdateNumObjectsWithMask(obj.LayersMask, -1);
	}

	public void NotifyObjectCategory(LevelObject obj, string oldCat, string newCat)
	{
		this.UpdateNumObjectsOfCategory(oldCat, -1);
		this.UpdateNumObjectsOfCategory(newCat, 1);
	}

	public void NotifyObjectLayersMask(LevelObject obj, int oldMask, int newMask)
	{
		this.UpdateNumObjectsWithMask(oldMask, -1);
		this.UpdateNumObjectsWithMask(newMask, 1);
	}

	public LevelCell FindObjectContainmentCell(LevelObject obj)
	{
		BoundsG other = obj.Bounds;
		LevelCell levelCell = this;
		while (levelCell.parent != null && !levelCell.bounds.Contains(other))
		{
			levelCell = levelCell.parent;
		}
		int count;
		int i;
		do
		{
			List<LevelCell> list = levelCell.children;
			count = list.Count;
			for (i = 0; i < count; i++)
			{
				LevelCell levelCell2 = list[i];
				if (levelCell2.bounds.Contains(other))
				{
					levelCell = levelCell2;
					break;
				}
			}
		}
		while (i != count);
		return levelCell;
	}

	public void RecurseQuery(BoundsG bbox, MathC.ClipStatus clipStatus, string category, int mask, List<LevelObject> results)
	{
		if (category != null && this.GetNumObjectsOfCategory(category) == 0)
		{
			return;
		}
		if (mask != -1 && this.GetNumObjectsWithMask(mask) == 0)
		{
			return;
		}
		if (clipStatus == MathC.ClipStatus.Overlapping)
		{
			clipStatus = MathC.GetClipStatus(bbox, this.bounds);
		}
		if (clipStatus == MathC.ClipStatus.Outside)
		{
			return;
		}
		bool flag = -1 != mask;
		bool flag2 = null != category;
		bool flag3 = MathC.ClipStatus.Overlapping == clipStatus;
		foreach (LevelObject levelObject in this.objects)
		{
			if (!flag || (mask & levelObject.LayersMask) != 0)
			{
				if (!flag2 || !(category != levelObject.Category))
				{
					if (!flag3 || MathC.GetClipStatus(bbox, levelObject.Bounds) != MathC.ClipStatus.Outside)
					{
						results.Add(levelObject);
					}
				}
			}
		}
		foreach (LevelCell levelCell in this.children)
		{
			levelCell.RecurseQuery(bbox, clipStatus, category, mask, results);
		}
	}

	private BoundsG bounds;

	private List<LevelObject> objects;

	private Dictionary<string, int> numObjectsByCat;

	private int[] numObjectsByMask;

	private LevelCell parent;

	private List<LevelCell> children;
}
