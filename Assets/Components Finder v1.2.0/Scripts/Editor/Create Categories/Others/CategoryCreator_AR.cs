/*Script created by Pierre Stempin*/

#if UNITY_5_5_OR_NEWER
using UnityEngine;
using System.Collections.Generic;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA;
#else
using UnityEngine.VR.WSA;
#endif

namespace ComponentsFinder
{
	public class CategoryCreator_AR 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.AR;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
#if UNITY_2017_1_OR_NEWER
				//new ComponentInfos (typeof (WorldAnchor)),
#endif

#if !UNITY_2017_2_OR_NEWER
				new ComponentInfos (typeof (SpatialMappingCollider)),
				new ComponentInfos (typeof (SpatialMappingRenderer))
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
