/*Script created by Pierre Stempin*/

using UnityEngine;

namespace ComponentsFinder
{
	public class CategoryCreator 
	{
		public static FoldoutInfos _FoldoutInfos;
		static ComponentInfos [] currentComponentInfos;

		public static FoldoutInfos CreateFoldout (FoldoutInfos foldoutInfos)
		{
			_FoldoutInfos = foldoutInfos;
			FoldoutCreator.CreateFoldout (ref _FoldoutInfos.IsFoldout, _FoldoutInfos.Name);

			if (_FoldoutInfos.IsFoldout) 
			{
				CreateFoldoutContent ();
			}

			return foldoutInfos;
		}

		private static void CreateFoldoutContent ()
		{
			for (int componentInfosID = 0; componentInfosID < _FoldoutInfos._ComponentInfos.Length; componentInfosID ++) 
			{
				CreateComponentDisplay (componentInfosID);
			}

			if (_FoldoutInfos.SpacesMediumAtEnd)
			{
				GUILayout.Space (WindowValues.EndCategorySpacingHeight);
			}
		}

		private static void CreateComponentDisplay (int componentInfosID)
		{
			if (_FoldoutInfos._ComponentInfos [componentInfosID].onCreateSubCategory != null)
			{
				RefreshIndentLevel.Add ();
				_FoldoutInfos._ComponentInfos [componentInfosID].onCreateSubCategory ();
				RefreshIndentLevel.Remove ();
			}
			else
			{
				_FoldoutInfos._ComponentInfos [componentInfosID].IsFavorite = UnityEditor.EditorPrefs.GetBool (_FoldoutInfos._ComponentInfos [componentInfosID].ScriptName, false);
				
				GUILayout.BeginHorizontal ();
				StarButtonCreator.CreateStarButton (ref _FoldoutInfos._ComponentInfos [componentInfosID]);
				ButtonCreator.CreateComponentButton (_FoldoutInfos._ComponentInfos [componentInfosID]);
				GUILayout.EndHorizontal ();
				
				if (_FoldoutInfos._ComponentInfos [componentInfosID].AddsASpace) 
				{
					GUILayout.Space (WindowValues.ShortSpacingHeight);
				}
			}
		}
	}
}
