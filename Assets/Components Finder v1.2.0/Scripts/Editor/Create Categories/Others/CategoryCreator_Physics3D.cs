/*Script created by Pierre Stempin*/

using UnityEngine;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_Physics3D 
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
			_FoldoutInfos.Name = ComponentsFinderStrings.Physics3DInParenthesis;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (Rigidbody), BoolOption.Adds3D),
				new ComponentInfos (typeof (CharacterController), BoolOption.Adds3D, BoolOption.AddsASpace),
				new ComponentInfos (typeof (BoxCollider), BoolOption.Adds3D),
				new ComponentInfos (typeof (SphereCollider), BoolOption.Adds3D),
				new ComponentInfos (typeof (CapsuleCollider), BoolOption.Adds3D),
				new ComponentInfos (typeof (MeshCollider), BoolOption.Adds3D),
				new ComponentInfos (typeof (WheelCollider), BoolOption.Adds3D),
				new ComponentInfos (typeof (TerrainCollider), BoolOption.Adds3D, BoolOption.AddsASpace),

#if UNITY_5 || UNITY_2017_1_OR_NEWER
				new ComponentInfos (typeof (Cloth), BoolOption.Adds3D, BoolOption.AddsASpace),
#else
				new ComponentInfos (typeof (InteractiveCloth), BoolOption.Adds3D),
				new ComponentInfos (typeof (SkinnedCloth), BoolOption.Adds3D),
				new ComponentInfos (typeof (ClothRenderer), BoolOption.Adds3D, BoolOption.AddsASpace),
#endif

				new ComponentInfos (typeof (HingeJoint), BoolOption.Adds3D),
				new ComponentInfos (typeof (FixedJoint), BoolOption.Adds3D),
				new ComponentInfos (typeof (SpringJoint), BoolOption.Adds3D),
				new ComponentInfos (typeof (CharacterJoint), BoolOption.Adds3D),
				new ComponentInfos (typeof (ConfigurableJoint), BoolOption.Adds3D, BoolOption.AddsASpace),
				new ComponentInfos (typeof (ConstantForce), BoolOption.Adds3D)
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
