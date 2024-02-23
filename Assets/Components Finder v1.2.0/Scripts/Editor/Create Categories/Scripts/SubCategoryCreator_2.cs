/*Script created by Pierre Stempin*/

#if UNITY_5_4_OR_NEWER && !UNITY_2017_2_OR_NEWER
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class SubCategoryCreator_2
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

#if UNITY_5_6
            _FoldoutInfos.Name = "UnityEngine.TestTools.TestRunner.Callbacks";
#elif UNITY_5_5
            _FoldoutInfos.Name = "UnityEngine.PlayModeTestsRunner";
#elif UNITY_5_4
            _FoldoutInfos.Name = "Unity Engine.Purchasing";
#endif
			_FoldoutInfos.SpacesMediumAtEnd = false;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
#if UNITY_5_6
                new ComponentInfos ("PlayModeRunnerCallback"),
#elif UNITY_5_5
                new ComponentInfos ("PlaymodeTestsController"),
#elif UNITY_5_4
				new ComponentInfos ("AsyncUtil"),
#endif

#if UNITY_5_5
				new ComponentInfos ("ResultsRenderer"),
				new ComponentInfos ("TestRunnerLoggerCallback"),
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
