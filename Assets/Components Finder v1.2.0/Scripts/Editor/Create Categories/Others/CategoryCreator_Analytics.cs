/*Script created by Pierre Stempin*/

#if UNITY_5_5_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace ComponentsFinder
{
	public class CategoryCreator_Analytics 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Analytics;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (AnalyticsTracker)),

#if UNITY_2017_3_OR_NEWER
				new ComponentInfos (typeof (AnalyticsEventTracker))
#endif
			
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
