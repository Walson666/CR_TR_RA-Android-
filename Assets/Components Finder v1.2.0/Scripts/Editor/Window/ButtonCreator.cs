/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class ButtonCreator 
	{
		public static void CreateComponentButton (ComponentInfos componentInfos)
		{
			Object obj = null;
			System.Type buttonType = componentInfos.T;

			GUIContent gUIContent = new GUIContent ();
			Texture texture = EditorGUIUtility.ObjectContent (obj, buttonType).image;
			gUIContent.image = texture;

			/* If you think that the "Find " part of the string in the button is too much",
			 * you can directly remove the "ComponentsFinderStrings.Find_ +" of the line.
			 * The space is added to have a better readability for the button*/
			string gUIText = ComponentsFinderStrings._Space + ComponentsFinderStrings.Find_ + componentInfos.DisplayedName;
			gUIContent.text = gUIText;

			if (GUILayout.Button (gUIContent))
			{
				if (buttonType != null)
				{
					ComponentsSearcher.SearchFilter (buttonType.Name);
				}
				else
				{
					ComponentsSearcher.SearchFilter (componentInfos.ScriptName);
				}
			}
		}
	}
}
