/*Script created by Pierre Stempin*/

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ComponentsFinder
{
	public class CategoryCreator_Layout 
	{
		static FoldoutInfos foldoutInfos;
		public static FoldoutInfos _FoldoutInfos 
		{
			get 
			{
				if (foldoutInfos == null)
				{
					ConfigureCategory ();
				}
				return foldoutInfos;
			}
			set {foldoutInfos = value;}
		}

		public static void ConfigureCategory ()
		{
			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.Layout;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (RectTransform)),
				new ComponentInfos (typeof (Canvas)),
				new ComponentInfos (typeof (CanvasGroup), BoolOption.AddsASpace),
				new ComponentInfos (typeof (CanvasScaler), BoolOption.AddsASpace),
				new ComponentInfos (typeof (LayoutElement)),
				new ComponentInfos (ComponentsFinderStrings.ContentSizeFitter),
				new ComponentInfos (ComponentsFinderStrings.AspectRatioFitter),
				new ComponentInfos (typeof (HorizontalLayoutGroup)),
				new ComponentInfos (typeof (VerticalLayoutGroup)),
				new ComponentInfos (typeof (GridLayoutGroup))
			}.ToArray ();
		}

		public static void CreateCategory ()
		{
			_FoldoutInfos = CategoryCreator.CreateFoldout (_FoldoutInfos);
			StarButtonCreator.onDisableStar += CallDisableStar;
		}

		public static void CallDisableStar (ComponentInfos componentInfos)
		{
			StarButtonCreator.DisableStar (ref _FoldoutInfos._ComponentInfos, componentInfos);
			StarButtonCreator.onDisableStar -= CallDisableStar;
		}
	}
}
#endif
