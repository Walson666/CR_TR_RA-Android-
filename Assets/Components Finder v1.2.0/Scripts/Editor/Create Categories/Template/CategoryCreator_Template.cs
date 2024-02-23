/*Script created by Pierre Stempin*/

/*Name of the future Unity older release with the specific components.
* You can modify the platform defendant compilation 
* to be sure that your new category is retro-compatible with older unity versions.
* For more infos, you can go to the Unity documentation : https://docs.unity3d.com/Manual/PlatformDependentCompilation.html*/
#if UNITY_2017_3_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using System;

namespace ComponentsFinder
{
	/*This script is a template, 
	 * this script is not used as it in the tool, 
	 * it's an exemple for the end-user to show how to add a new component category
	 * Be sure that your new script will be inside a folder named "Editor" or one of his sub-folder*/
	public class CategoryCreator_Template 
	{
		/*Create one action for each subcategory of this category*/
		public static Action onCreateSubCategory1;
		public static Action onCreateSubCategory2;

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
			/*Set all the subcategories of this category here*/
			SetSubcategory1 ();
			SetSubcategory2 ();

			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = "Template Example"; //Category Title
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (onCreateSubCategory1), //Call the subCategory1 (place it where you need to call it in the list)

				/*Component examples*/
				new ComponentInfos (typeof (AudioSource)), //Default call, displays "AudioSource" and its icon
				new ComponentInfos (typeof (AudioSource), "Audio"), //Used for AudioSource component but displays the string "Audio"
				new ComponentInfos (typeof (AudioSource), BoolOption.Adds3D), //Used for AudioSource component but displays the string "Audio (3D)"
				new ComponentInfos (typeof (AudioSource), BoolOption.AddsASpace), //Used for AudioSource component and add a space in the inspector just after the button corresponding to this component
				new ComponentInfos (typeof (AudioSource), BoolOption.Adds3D, BoolOption.AddsASpace), //Combine the two last methods
				new ComponentInfos (typeof (AudioSource), "Audio", BoolOption.AddsASpace), //Used for AudioSource component but displays the string "Audio" and add a space in the inspector just after the button corresponding to this component

				new ComponentInfos (onCreateSubCategory2) //Call the subCategory2 (place it where you need to call it in the list)
			}.ToArray ();
		}

		/*Create one method for each subcategory of this category, following this current model*/
		private static void SetSubcategory1 ()
		{
			if (onCreateSubCategory1 == null)
			{
				onCreateSubCategory1 += SubCategoryCreator_1_Template.CreateCategory;
			}
		}

		private static void SetSubcategory2 ()
		{
			if (onCreateSubCategory2 == null)
			{
				onCreateSubCategory2 += SubCategoryCreator_2_Template.CreateCategory;
			}
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
