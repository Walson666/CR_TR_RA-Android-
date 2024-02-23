/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_Miscellaneous 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Miscellaneous;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{

#if UNITY_2018_1_OR_NEWER
				new ComponentInfos (typeof (Animator)),
				/*new ComponentInfos (typeof (PositionConstraint)),
				new ComponentInfos (typeof (RotationConstraint)),
				new ComponentInfos (typeof (ScaleConstraint)),
				new ComponentInfos (typeof (ParentConstraint)),
				new ComponentInfos (typeof (AimConstraint)),*/
				new ComponentInfos (typeof (Terrain)),
				new ComponentInfos (typeof (BillboardRenderer)),
				//new ComponentInfos (typeof (NetworkView)), //obsolete component
				new ComponentInfos (typeof (Animation)),
				new ComponentInfos (typeof (Grid)),
				new ComponentInfos (typeof (WindZone)),
				new ComponentInfos (typeof (SpriteMask)),
				//new ComponentInfos (typeof (SpriteShapeRenderer))
#elif UNITY_2017_2_OR_NEWER
				new ComponentInfos (typeof (BillboardRenderer)),
				new ComponentInfos (typeof (NetworkView)), //obsolete component
				new ComponentInfos (typeof (Terrain)),
				new ComponentInfos (typeof (Animator)),
				new ComponentInfos (typeof (Animation)),
				new ComponentInfos (typeof (Grid)),
				new ComponentInfos (typeof (WindZone)),
				new ComponentInfos (typeof (SpriteMask))
#elif UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (Animator)),
				new ComponentInfos (typeof (Animation)),
				new ComponentInfos (typeof (NetworkView)), //obsolete component
				new ComponentInfos (typeof (WindZone)),
				new ComponentInfos (typeof (Terrain)),
				new ComponentInfos (typeof (BillboardRenderer)),
    #if UNITY_5_1
                new ComponentInfos ("CloudServiceHandlerBehaviour"),
    #endif
#else
                new ComponentInfos (typeof (Animator)),
                new ComponentInfos (typeof (Animation)),
                new ComponentInfos (typeof (NetworkView)), //obsolete component
				new ComponentInfos ("WindZone"),
	#if !(UNITY_4_6 || UNITY_4_7)
				new ComponentInfos ("UIRenderer"),
				new ComponentInfos ("Canvas"),
				new ComponentInfos ("RectTransform")
	#endif
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
