/*Script created by Pierre Stempin*/

#if UNITY_2017_2_OR_NEWER
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace ComponentsFinder
{
	public class CategoryCreator_Tilemap 
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
			//Space at the end is necessary to avoid Unity confusion between the foldout name and the type name
			_FoldoutInfos = new FoldoutInfos ();
			_FoldoutInfos.Name = "Tilemap "; 
			_FoldoutInfos._ComponentInfos = new List <ComponentInfos> () 
			{
				new ComponentInfos (typeof (Tilemap)),
				new ComponentInfos (typeof (TilemapRenderer)),
				new ComponentInfos (typeof (TilemapCollider2D))
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
