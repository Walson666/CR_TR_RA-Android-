/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class ComponentsFinder_CreateWindow 
	{
		const string windowDisplayedName = ComponentsFinderStrings.Finder;

		public static void GetWindow ()
		{
			ComponentsFinder_Window._ComponentsFinder_Window = EditorWindow.GetWindow (typeof (ComponentsFinder_Window)) as ComponentsFinder_Window;
			ComponentsFinder_Window._ComponentsFinder_Window.minSize = new Vector2 (WindowValues.MinLength, WindowValues.MinHeight);

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
			ComponentsFinder_Window._ComponentsFinder_Window.titleContent.text = windowDisplayedName;
#else
			ComponentsFinder_Window._ComponentsFinder_Window.title = "Finder";
#endif
		}
	}
}
