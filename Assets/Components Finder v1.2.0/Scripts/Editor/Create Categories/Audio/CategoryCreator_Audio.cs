/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace ComponentsFinder
{
	public class CategoryCreator_Audio
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
#if UNITY_5_5_OR_NEWER
			SetSubcategory ();
#endif

			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.Audio;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (AudioListener)),
				new ComponentInfos (typeof (AudioSource)),
				new ComponentInfos (typeof (AudioReverbZone), BoolOption.AddsASpace
				)

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				,
				new ComponentInfos (typeof (AudioLowPassFilter)),
				new ComponentInfos (typeof (AudioHighPassFilter)),
				new ComponentInfos (typeof (AudioEchoFilter)),
				new ComponentInfos (typeof (AudioDistortionFilter)),
				new ComponentInfos (typeof (AudioReverbFilter)),
				new ComponentInfos (typeof (AudioChorusFilter)),

	#if UNITY_5_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (onCreateSubCategory)
	#endif
#endif

			}.ToArray ();
		}

#if UNITY_5_5_OR_NEWER
		private static void SetSubcategory ()
		{
			if (onCreateSubCategory == null)
			{
				onCreateSubCategory += SubCategoryCreator_AudioSpatializer.CreateCategory;
			}
		}
#endif

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
