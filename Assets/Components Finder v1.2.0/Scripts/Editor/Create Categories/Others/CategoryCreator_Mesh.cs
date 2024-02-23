/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace ComponentsFinder
{
	public class CategoryCreator_Mesh 
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
			_FoldoutInfos.Name = ComponentsFinderStrings._Mesh;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (MeshFilter)),
				new ComponentInfos (typeof (TextMesh), BoolOption.AddsASpace),
				new ComponentInfos (typeof (MeshRenderer)),
				new ComponentInfos (typeof (SkinnedMeshRenderer))
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
