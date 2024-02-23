/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class FoldoutCreator 
	{
		public static void CreateFoldout (ref bool isFoldout, string foldoutName)
		{
			isFoldout = EditorPrefs.GetBool (foldoutName, true);
			isFoldout = EditorGUILayout.Foldout (isFoldout, foldoutName);
			EditorPrefs.SetBool (foldoutName, isFoldout);
		}
	}
}
