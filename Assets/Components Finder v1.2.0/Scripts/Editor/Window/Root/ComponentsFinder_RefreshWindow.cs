/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace ComponentsFinder
{
	public class ComponentsFinder_RefreshWindow
	{
		static Vector2 scrollPosition = Vector2.zero;

		/// <summary>
		/// Refreshs the window.
		/// </summary>
		public static void RefreshWindow ()
		{
			SetIconSize ();
			CreateScrollViewContent ();
		}

		private static void SetIconSize ()
		{
			const float iconSize = 13.0f; //set the size of the components icon (13*13 pixels)
			Vector2 iconVector = new Vector2 (iconSize, iconSize);
			EditorGUIUtility.SetIconSize (iconVector);
		}

		private static void CreateScrollViewContent ()
		{
			WindowValues.CurrentPositionY = 0f; //reset
			scrollPosition = GUILayout.BeginScrollView (scrollPosition); //scroll start
			CreateCategories ();
			GUILayout.EndScrollView (); //scroll end
		}

		/// <summary>
		/// Creates the categories.
		/// </summary>
		public static void CreateCategories ()
		{
			CategoryCreator_Favorites.CreateCategory ();
			CategoryCreator_Mesh.CreateCategory ();
			CategoryCreator_Effects.CreateCategory ();
			CategoryCreator_Physics3D.CreateCategory ();
			CategoryCreator_Physics2D.CreateCategory ();
			CategoryCreator_Navigation.CreateCategory ();
			CategoryCreator_Audio.CreateCategory ();

#if UNITY_5_6_OR_NEWER
			CategoryCreator_Video.CreateCategory ();
#endif

			CategoryCreator_Rendering.CreateCategory ();

#if UNITY_2017_2_OR_NEWER
			CategoryCreator_Tilemap.CreateCategory ();
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
			CategoryCreator_Layout.CreateCategory ();
#endif

#if UNITY_2017_1_OR_NEWER
			CategoryCreator_Playables.CreateCategory ();
			CategoryCreator_AR.CreateCategory ();
#endif

			CategoryCreator_Miscellaneous.CreateCategory ();
			CategoryCreator_Scripts.CreateCategory ();

#if UNITY_5_5_OR_NEWER
			CategoryCreator_Analytics.CreateCategory ();
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
			CategoryCreator_Event.CreateCategory ();
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4_OR_NEWER
			CategoryCreator_Network.CreateCategory ();
#endif

#if UNITY_2017_2_OR_NEWER
			CategoryCreator_XR.CreateCategory ();
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
			CategoryCreator_UI.CreateCategory ();
#endif

#if UNITY_5_5_OR_NEWER && !UNITY_2017_1_OR_NEWER
			CategoryCreator_AR.CreateCategory ();
#endif

			CategoryCreator_Others.CreateCategory ();

			//You can add your own categrories here
			/*CategoryCreator_Template.CreateCategory ();*/
		}
	}
}
