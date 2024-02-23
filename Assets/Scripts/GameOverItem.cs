// dnSpy decompiler from Assembly-CSharp.dll class: GameOverItem
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverItem : MonoBehaviour
{
	public int id
	{
		get
		{
			return base.transform.GetSiblingIndex();
		}
	}

	private void Awake()
	{
		//this.titleItem.text = GameOverItem.titles[this.id];
	}

	public void SetValue(int count)
	{
		this.currency = Mathf.CeilToInt(GameOverItem.reward[this.id] * (float)count);
		this.currencyCountText.text = this.currency.ToString();
		this.ItemCountText.text = string.Format("{0} {1}", count, GameOverItem.postFix[this.id]);
		if (base.gameObject.activeInHierarchy)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.IncrementCoroutine(this.currencyCountText, this.currency, 0));
		}
	}

	public void SetValue(bool active, int count)
	{
		base.gameObject.SetActive(active);
		this.SetValue(count);
	}

	private IEnumerator IncrementCoroutine(Text l, int targetValue, int startingValue = 0)
	{
		float time = 0f;
		l.text = startingValue.ToString();
		float incrementTime = 1.2f;
		while (time < incrementTime)
		{
			yield return null;
			time += Time.deltaTime;
			float factor = time / incrementTime;
			l.text = ((int)Mathf.Lerp((float)startingValue, (float)targetValue, factor)).ToString();
		}
		l.text = targetValue.ToString();
		yield break;
	}

	public Text titleItem;

	public Text ItemCountText;

	public Text currencyCountText;

	private int score;

	internal int currency;

	public static string[] titles = new string[]
	{
		"Distance",
		"Overtakes",
		"High Speed",
		"Coins Collected",
		"Wrong Lane"
	};

	public static float[] reward = new float[]
	{
		0.1f,
		1f,
		0.25f,
		1f,
		0.1f
	};

	public static string[] postFix = new string[]
	{
		"(M)",
		string.Empty,
		"(M)",
		string.Empty,
		"(M)"
	};
}
