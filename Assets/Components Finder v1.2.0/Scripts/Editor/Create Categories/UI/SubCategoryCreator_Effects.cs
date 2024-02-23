/*Script created by Pierre Stempin*/

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ComponentsFinder
{
	public class SubCategoryCreator_Effects
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
			_FoldoutInfos.Name = "Effects "; //Space at the end is necessary to avoid Unity confusion between this sub-category name and the effects category name
		
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (Shadow)),
				new ComponentInfos (typeof (Outline)),
				new ComponentInfos (typeof (PositionAsUV1))
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
