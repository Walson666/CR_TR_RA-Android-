/*Script created by Pierre Stempin*/

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_UI 
	{
		public static Action onCreateSubCategory;

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
			SetSubcategory ();

			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.UI;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (onCreateSubCategory),

#if !(UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER)
				new ComponentInfos (typeof (Image)),
				new ComponentInfos (typeof (Text)),
#else
				new ComponentInfos (typeof (Text)),
				new ComponentInfos (typeof (Image)),
#endif

				new ComponentInfos (typeof (RawImage)),
				new ComponentInfos (typeof (Mask), BoolOption.AddsASpace),
#if UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (RectMask2D), BoolOption.AddsASpace),
#elif UNITY_5_2 || UNITY_5_3
				new ComponentInfos (typeof (RectMask2D), "2DRectMask", BoolOption.AddsASpace),
#endif
				new ComponentInfos (typeof (Button)),
				new ComponentInfos (typeof (InputField)),

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (Toggle)),
				new ComponentInfos (typeof (ToggleGroup)),
#else
				new ComponentInfos (typeof (Scrollbar)),
				new ComponentInfos (typeof (ScrollRect)),
#endif

				new ComponentInfos (typeof (Slider)),

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (Scrollbar)),
				new ComponentInfos (typeof (Dropdown)),
				new ComponentInfos (typeof (ScrollRect), BoolOption.AddsASpace),
#else
				new ComponentInfos (typeof (Toggle)),
				new ComponentInfos (typeof (ToggleGroup), BoolOption.AddsASpace),
#endif

				new ComponentInfos (typeof (Selectable)),

			}.ToArray ();
		}

		private static void SetSubcategory ()
		{
			if (onCreateSubCategory == null)
			{
				onCreateSubCategory += SubCategoryCreator_Effects.CreateCategory;
			}
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
