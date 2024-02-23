/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace ComponentsFinder
{
	/// <summary>
	/// Does the main feature of the Components Finder : 
	/// the search by transforming a click on a specific button to a string search in the hierarchy window
	/// </summary>
	public class ComponentsSearcher
	{
		public const int filterMode_All = 0;
		public const int filterMode_Name = 1;
		public const int filterMode_Type = 2;

		static SearchableEditorWindow hierarchy;

		public static void SearchFilter (string componentToSearch) 
		{
			SearchFilter (componentToSearch, 0);
		}

		public static void SearchFilter (string filter, int filterMode) 
		{
			SearchFilter (filter, filterMode, ComponentsFinderStrings.SearchByType);
		}

		public static void SearchFilter (string filter, int filterMode, string prefix)
		{
			SearchableEditorWindow [] windows = (SearchableEditorWindow []) Resources.FindObjectsOfTypeAll (typeof (SearchableEditorWindow));

			foreach (SearchableEditorWindow window in windows) 
			{
				string currentName = window.GetType ().ToString ();
				string nameToCheck = ComponentsFinderStrings.UnityEditorSceneHierarchyWindow;

				if (currentName == nameToCheck) 
				{
					hierarchy = window;
					break;
				}
			}

			if (hierarchy != null)
			{
                //string searchMethod = ComponentsFinderStrings.SetSearchFilter;
                //BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

                //MethodInfo setSearchType = typeof (SearchableEditorWindow).GetMethod (searchMethod, bindingFlags); 
                //filter = prefix + filter;
                //object [] parameters = new object [] {filter, filterMode, false};
                //object[] parameters = new object[] { filter, filterMode };

                //setSearchType.Invoke (hierarchy, parameters);
                MeshRenderer[] MeshRenderer = GameObject.FindObjectsOfType<MeshRenderer>();
				Debug.LogError(MeshRenderer.Length);

                /*BoxCollider[] BoxCollider = GameObject.FindObjectsOfType<BoxCollider>();
                MeshCollider[] MeshCollider = GameObject.FindObjectsOfType<MeshCollider>();

				for (int i = 0; i < BoxCollider.Length; ++i)
				{
					MonoBehaviour.DestroyImmediate(BoxCollider[i]);
				}
                for (int i = 0; i < MeshCollider.Length; ++i)
                {
                    MonoBehaviour.DestroyImmediate(MeshCollider[i]);
                }*/
                for (int i = 0; i < MeshRenderer.Length; i++)
				{
					MeshRenderer[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    MeshRenderer[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
				MeshRenderer[i].motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                   MeshRenderer[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					MeshRenderer[i].receiveShadows = false;
                    //MonoBehaviour.DestroyImmediate(Renderers[i]);
                }
                //for (int i = 0; int i < Renderers.Length; int i++)
                //{
                //Renderers[i].
                //}

            }
		}
	}
}
