/*Script created by Pierre Stempin*/

#if UNITY_2017_2_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.WSA;

namespace ComponentsFinder
{
	public class CategoryCreator_XR 
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
			_FoldoutInfos.Name = "XR";
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (TrackedPoseDriver)),
				//new ComponentInfos (typeof (SpatialMappingCollider)),
				//new ComponentInfos (typeof (SpatialMappingRenderer))
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
