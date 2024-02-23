/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	/// <summary>
	/// Create the editor window of the Components Finder tool
	/// </summary>
	public class ComponentsFinder_Window : EditorWindow 
	{
		public static ComponentsFinder_Window _ComponentsFinder_Window;
		const string windowName = ComponentsFinderStrings._ComponentsFinder;

		/*To change the shortcut, change this line
		 * you can use ComponentsFinderStrings.ShiftName or / and
		 * ComponentsFinderStrings.AltName or / and
		 * ComponentsFinderStrings.ControlName
		For more infos, you can go to the unity documentation : https://docs.unity3d.com/ScriptReference/MenuItem.html */
		const string shortcutName = ComponentsFinderStrings.AltName + ComponentsFinderStrings.ShortcutName;

		const string windowPath = ComponentsFinderStrings.Window + ComponentsFinderStrings.Slash + windowName + ComponentsFinderStrings._Space + shortcutName;

		/// <summary>
		/// Creates the window.
		/// </summary>
 [MenuItem (windowPath)]
		public static void CreateWindow ()
		{
			ComponentsFinder_CreateWindow.GetWindow ();
			_ComponentsFinder_Window.Show ();
		}

		void OnGUI ()
		{
			ComponentsFinder_RefreshWindow.RefreshWindow ();
		}
	}
}
