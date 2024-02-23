/*Script created by Pierre Stempin*/

#if UNITY_5_5_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace ComponentsFinder
{
	public class SubCategoryCreator_AudioSpatializer
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
			_FoldoutInfos.Name = "Audio Spatializer";
			_FoldoutInfos.SpacesMediumAtEnd = false;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				//new ComponentInfos (typeof (AudioSpatializerMicrosoft))
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
