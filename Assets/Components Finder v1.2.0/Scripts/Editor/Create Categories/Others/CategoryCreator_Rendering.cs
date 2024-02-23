/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace ComponentsFinder
{
	public class CategoryCreator_Rendering 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Rendering;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (Camera)),
				new ComponentInfos (typeof (Skybox)),

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (FlareLayer)),
#else
				new ComponentInfos (ComponentsFinderStrings._FlareLayer),
#endif

				//new ComponentInfos (typeof (GUILayer), BoolOption.AddsASpace), //obsolete component
				new ComponentInfos (typeof (Light)),

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (LightProbeGroup)),
#else
                new ComponentInfos (typeof (LightProbeGroup), BoolOption.AddsASpace),
#endif

#if UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (LightProbeProxyVolume)),
#endif

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (ReflectionProbe), BoolOption.AddsASpace),
#endif

				new ComponentInfos (typeof (OcclusionArea)),
				new ComponentInfos (typeof (OcclusionPortal)),
				new ComponentInfos (typeof (LODGroup), BoolOption.AddsASpace),
				new ComponentInfos (typeof (SpriteRenderer)),

#if UNITY_5_6_OR_NEWER
				new ComponentInfos (typeof (SortingGroup)),
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (CanvasRenderer), BoolOption.AddsASpace),
#endif

				//new ComponentInfos (typeof (GUITexture)), //obsolete component
				//new ComponentInfos (typeof (GUIText)) //obsolete component
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
