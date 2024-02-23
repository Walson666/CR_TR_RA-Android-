/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class ScriptableObject_Creator 
	{
		//[MenuItem ("Assets/Create/Favorites")] //in case of
		public static void Create ()
		{
			Favorites asset = ScriptableObject.CreateInstance <Favorites> ();
			AssetDatabase.CreateAsset (asset, "Assets/Scr.asset");
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
	}
}
