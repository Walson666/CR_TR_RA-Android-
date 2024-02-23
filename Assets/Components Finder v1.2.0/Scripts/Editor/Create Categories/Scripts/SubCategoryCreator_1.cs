/*Script created by Pierre Stempin*/

#if UNITY_5_4_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace ComponentsFinder
{
	public class SubCategoryCreator_1
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
#if UNITY_5_5_OR_NEWER
			_FoldoutInfos.Name = ComponentsFinderStrings.UnityEngine_EventSystems;
#elif UNITY_5_4
            _FoldoutInfos.Name = "UnityEngine.Advertisements";
#endif
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
#if UNITY_5_5_OR_NEWER
				new ComponentInfos (typeof (BaseInput)),
				//new ComponentInfos (typeof (HoloLensInput))
#elif UNITY_5_4
				new ComponentInfos ("UnityAdsEditorPlaceholder")
#endif
			}.ToArray ();
		}

		public static void CreateCategory ()
		{
			_FoldoutInfos = SubCategoryCreator.CreateFoldout (_FoldoutInfos);
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
