/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_Physics2D 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Physics2D;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (Rigidbody2D), BoolOption.AddsASpace),

#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (BoxCollider2D)),
				new ComponentInfos (typeof (CircleCollider2D)),
#else
				new ComponentInfos (typeof (CircleCollider2D)),
				new ComponentInfos (typeof (BoxCollider2D)),
#endif

				new ComponentInfos (typeof (EdgeCollider2D)),
				new ComponentInfos (typeof (PolygonCollider2D)
					
#if !UNITY_5_5_OR_NEWER
					, BoolOption.AddsASpace
#endif
					),

#if UNITY_5_5_OR_NEWER
				new ComponentInfos (typeof (CapsuleCollider2D)),
#endif

#if UNITY_5_6_OR_NEWER
				new ComponentInfos (typeof (CompositeCollider2D), BoolOption.AddsASpace),
#endif

#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				new ComponentInfos (typeof (SpringJoint2D)),
#endif

				new ComponentInfos (typeof (DistanceJoint2D)),

#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (FixedJoint2D)),
				new ComponentInfos (typeof (FrictionJoint2D)),
#endif

				new ComponentInfos (typeof (HingeJoint2D)),

#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (RelativeJoint2D)),
#endif

				new ComponentInfos (typeof (SliderJoint2D)),

#if !(UNITY_5 || UNITY_2017_1_OR_NEWER)
				new ComponentInfos (typeof (WheelJoint2D)),
#endif

#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (SpringJoint2D)),
				new ComponentInfos (typeof (TargetJoint2D)),
				new ComponentInfos (typeof (WheelJoint2D), BoolOption.AddsASpace),
#endif

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				new ComponentInfos (typeof (WheelJoint2D), BoolOption.AddsASpace),
#endif

#if UNITY_5 || UNITY_2017_1
				new ComponentInfos (typeof (ConstantForce2D), BoolOption.AddsASpace),
#endif

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (AreaEffector2D)),
#endif

#if UNITY_5_3 || UNITY_5_4_OR_NEWER
				new ComponentInfos (typeof (BuoyancyEffector2D)),
#endif

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (PointEffector2D)),
				new ComponentInfos (typeof (PlatformEffector2D)),
				new ComponentInfos (typeof (SurfaceEffector2D)

	#if UNITY_5 || UNITY_2017_1_OR_NEWER && !(UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
					, BoolOption.AddsASpace
	#endif
					),

	#if !(UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
				new ComponentInfos (typeof (ConstantForce2D))
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
