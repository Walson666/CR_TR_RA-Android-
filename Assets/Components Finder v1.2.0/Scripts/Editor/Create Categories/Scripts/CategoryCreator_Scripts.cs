/*Script created by Pierre Stempin*/

using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
using UnityEngine.EventSystems;
#endif

namespace ComponentsFinder
{
	public class CategoryCreator_Scripts 
	{
		public static Action onCreateSubCategory1;
		public static Action onCreateSubCategory2;
		public static Action onCreateSubCategoryUserScripts;

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
#if UNITY_5_4_OR_NEWER
			SetSubcategory1 ();
	#if !UNITY_2017_2_OR_NEWER
			SetSubcategory2 ();
	#endif
#endif
			SetSubcategoryUserScripts ();

			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = ComponentsFinderStrings.Scripts;
			_FoldoutInfos.SpacesMediumAtEnd = false;
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
#if UNITY_5_5_OR_NEWER
				new ComponentInfos (onCreateSubCategory1),
	#if !UNITY_2017_1_OR_NEWER
				new ComponentInfos (onCreateSubCategory2),
	#endif
#endif
				new ComponentInfos (onCreateSubCategoryUserScripts)
			}.ToArray ();
		}

#if UNITY_5_4_OR_NEWER
		private static void SetSubcategory1 ()
		{
			onCreateSubCategory1 = null;

			if (onCreateSubCategory1 == null)
			{
				onCreateSubCategory1 += SubCategoryCreator_1.CreateCategory;
			}
		}
	#if !UNITY_2017_2_OR_NEWER
		private static void SetSubcategory2 ()
		{
			onCreateSubCategory2 = null;

			if (onCreateSubCategory2 == null)
			{
				onCreateSubCategory2 += SubCategoryCreator_2.CreateCategory;
			}
		}
	#endif
#endif

		private static void SetSubcategoryUserScripts ()
		{
			onCreateSubCategoryUserScripts = null;

			if (onCreateSubCategoryUserScripts == null)
			{
				onCreateSubCategoryUserScripts += SubCategoryCreator_UserScripts.CreateUserScriptsCategory;
			}
		}

		public static void CreateCategory ()
		{
			_FoldoutInfos = CategoryCreator.CreateFoldout (_FoldoutInfos);
		}
	}
}
