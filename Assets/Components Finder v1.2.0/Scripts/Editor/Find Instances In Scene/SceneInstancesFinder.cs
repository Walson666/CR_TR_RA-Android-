/*Script created by Pierre Stempin*/

using UnityEditor;
using UnityEngine;

namespace ComponentsFinder
{
	public class SceneInstancesFinder : MonoBehaviour
	{
		const string folderPath = "Assets/Find Instances In Scene";
		const string shortcutName = ComponentsFinderStrings.ShiftName + ComponentsFinderStrings.AltName + "F";
		const string menuPath = folderPath + " " + shortcutName;

		[MenuItem (menuPath, false, 20)]
		public static void FindInstances ()
		{
			if (Selection.activeObject != null)
			{
				Object selectedObject = Selection.activeObject;
				System.Type selectedType = selectedObject.GetType ();

				if (selectedType == typeof (MonoScript))
				{
					ComponentsSearcher.SearchFilter (selectedObject.name);
				}
			}
		}

		[MenuItem (menuPath, true, 20)]
		public static bool CheckFindInstances ()
		{
			Object selectedObject = Selection.activeObject;
			bool selectionIsScript = false;

			if (Selection.activeObject != null)
			{
				System.Type selectedType = selectedObject.GetType ();
				selectionIsScript = selectedType == typeof (MonoScript);
			}
			else
			{
				selectionIsScript = false;
			}

			return selectionIsScript;
		}
	}
}
