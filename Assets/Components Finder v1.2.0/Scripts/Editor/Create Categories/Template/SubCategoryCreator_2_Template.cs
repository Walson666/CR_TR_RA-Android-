/*Script created by Pierre Stempin*/

#if UNITY_2017_3_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace ComponentsFinder
{
	/*This is a subcategory of the Template category, create one script like this per subcategory
	* this script is a template, 
	* this script is not used as it in the tool, 
	* it's an exemple for the end-user to show how to add a new component sub category
	* Be sure that your new script will be inside a folder named "Editor" or one of his sub-folder*/
	public class SubCategoryCreator_2_Template
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
			_FoldoutInfos.Name = "Audio Sub Category 2"; //Sub Category Title
			_FoldoutInfos.SpacesMediumAtEnd = false;

			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				/*Component examples*/
				new ComponentInfos (typeof (GameObject)),
				new ComponentInfos (typeof (Transform))
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
